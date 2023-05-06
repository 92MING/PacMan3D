using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using UnityEngine.Events;

public enum CharacterAnimation
{
    IDLE,
    WALK_FRONT,
    WALK_BACK,
    WALK_LEFT,
    WALK_RIGHT,
    ATTACK,
    SKILL,
    SENSE,
    GET_HIT,
    STUN,
    DIE,
    VICTORY,
}

/// <summary>
/// 角色基類
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[System.Serializable]
public abstract class CharacterBase : MonoBehaviour
{
    public static LinkedList<Type> AllCharacterType = new LinkedList<Type>();
    public Sprite icon => ResourcesManager.GetSprite(this.GetType().Name);
    public UnityEvent<int> OnHpChanged = new UnityEvent<int>();
    public UnityEvent<int> OnCoinChanged = new UnityEvent<int>();

    public abstract int maxHP { get; }
    public abstract int atk { get; }
    public abstract int def { get; }
    public abstract int spd { get; }
    public virtual float gainEnergySpd => 1.0f; //获取能量速度

    protected int _hp;
    public int hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            if (_hp <= 0)
            {
                _hp = 0;
            } 
            else if (hp > maxHP)
            {
                hp = maxHP;
            }
            OnHpChanged.Invoke(_hp);
        }
    }

    protected int _energy; //能量， 用于释放技能。[0, 100]
    public int energy {
        get { return _energy; }
        set { _energy = ((value < 0) ? 0 : ((value > 100) ? 100 : value)); }
    }

    protected Rigidbody _rigidbody;
    public Rigidbody rigidBody => _rigidbody;
    protected Animator _animator;
    public Animator animator => _animator;
    protected CharacterAnimation _currentAni = CharacterAnimation.IDLE;
    public CharacterAnimation currentAni => _currentAni;

    protected HashSet<MapObject> _colliders = new HashSet<MapObject>();

    public virtual float jumpForce => 5.0f; // 跳跃力
    public bool touchingFloor
    {
        get
        {
            foreach (var maoObj in _colliders)
            {
                if (maoObj.type == MapObjectType.EMPTY) return true;
            }
            return false;
        }
    }
    protected Vector2 _currentInput = Vector2.zero;    // h, v
    protected Vector2 _currentMouseInput = Vector2.zero; //h, v
    protected Vector3 _controlDirection = Vector3.zero;

    protected bool _invisible = false;
    public bool invisible => _invisible; // 是否隱形
    protected int _coins = 0;
    public int coins 
    {
        get { return _coins; }
        set 
        { 
            _coins = ((value < 0) ? 0 : value);
            OnCoinChanged.Invoke(_coins);
        }
    } // 金幣數量

    /// <summary>
    /// 在开始前，加载所有角色类到static LinkedList 中
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void LoadAllCharacterType()
    {
        foreach (var clsType in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (clsType.IsSubclassOf(typeof(CharacterBase)) && !clsType.IsAbstract)
            {
                AllCharacterType.AddLast(clsType);
            }
        }
    }

    protected virtual void Awake()
    {
        if (!TryGetComponent(out _rigidbody))
        {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
        }
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.mass = 1.0f;
        _animator = GetComponentInChildren<Animator>();
        _hp = maxHP;
        GameManager.OnGamePause.AddListener(disableAnimator);
        GameManager.OnGamePause.AddListener(enableAnimator);
    }
    protected virtual void OnDestroy()
    {
        GameManager.OnGamePause.RemoveListener(disableAnimator);
        GameManager.OnGamePause.RemoveListener(enableAnimator);
    }
    protected void disableAnimator()
    {
        IEnumerator WaitAndStopAnimation(Animator animator)
        {
            yield return null; // 等待一帧的时间
            animator.speed = 0f; // 将当前动画的速度设置为0
        }
        StartCoroutine(WaitAndStopAnimation(GetComponentInChildren<Animator>()));
    }
    protected void enableAnimator()
    {
        IEnumerator WaitAndStopAnimation(Animator animator)
        {
            yield return null; // 等待一帧的时间
            animator.speed = 1f; // 将当前动画的速度设置为0
        }
        StartCoroutine(WaitAndStopAnimation(GetComponentInChildren<Animator>()));
    }
    protected void Update()
    {
        if (GameManager.isPlaying && !GameManager.isPaused)
        {
            if (!GameManager.isPlaying || GameManager.isPaused || GameManager.playerCharacter != this)
            {
                //AI 控制

            }
            else
            {
                mouseViewControl();
                basicMovementControl();
                jumpControl();
                attackAndSkillControl();
            }
        }
    }
    protected void FixedUpdate()
    {
        if (GameManager.isPlaying && !GameManager.isPaused && !_died)
        {
            //更新位置与动画
            if (_currentInput.x > 0.2f || _currentInput.x < -0.2f || _currentInput.y > 0.2f || _currentInput.y < -0.2f)
            {
                transform.position += _controlDirection * Time.deltaTime * spd;
                if (Mathf.Abs(_currentInput.y) >= Mathf.Abs(_currentInput.x))
                {
                    switchAnimation((_currentInput.y >= 0) ? CharacterAnimation.WALK_FRONT : CharacterAnimation.WALK_BACK);
                }
                else
                {
                    switchAnimation((_currentInput.x >= 0) ? CharacterAnimation.WALK_RIGHT : CharacterAnimation.WALK_LEFT);
                }
            }
            else
            {
                switchAnimation(CharacterAnimation.IDLE);
            }
        }
    }

    //controls
    protected virtual void mouseViewControl()
    {
        _currentMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        // 更新横向视角
        var mouseControl = _currentMouseInput * SystemManager.mouseSensitivety;
        transform.Rotate(axis: transform.up, mouseControl.x, Space.Self);

        //更新垂直视角
        GameManager.camVerticalRotationRatio += mouseControl.y / 128f;
    }
    protected virtual void basicMovementControl()
    {
        if (_died) return;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        _currentInput = new Vector2(h, v);
        _controlDirection = new Vector3(h, 0, v);
        _controlDirection = transform.TransformDirection(_controlDirection.normalized);
    }
    protected virtual void jumpControl()
    {
        if (_died) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (touchingFloor) rigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
    protected abstract void attackAndSkillControl();
    public void switchAnimation(CharacterAnimation ani)
    {
        if (_died) return;
        if (ani == currentAni) return;
        _animator.SetInteger("state", (int)ani);
        _currentAni = ani;
    }

    //退出、进入游戏模式
    protected RigidbodyConstraints oldConstraint = RigidbodyConstraints.FreezeRotation;
    public void enterGameMode()
    {
        rigidBody.useGravity = true;
        rigidBody.isKinematic = false;
        rigidBody.constraints = oldConstraint;
    }
    public void exitGameMode()
    {
        rigidBody.useGravity = false;
        rigidBody.isKinematic = true;
        oldConstraint = rigidBody.constraints;
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
    }
    
    //放置在某个地图格
    public void placeAtMapCell(Vector2Int cellPpos)
    {
        var mapObj = MapManager.GetMapCellAt(cellPpos);
        if (mapObj != null) placeAtMapCell(mapObj);
    }
    public void placeAtMapCell(MapObject mapObj)
    {
        if (mapObj.type != MapObjectType.EMPTY) return; //只能放在地上。 Empty代表地板.
        transform.position = mapObj.transform.position + Vector3.up / 1.98f;
        transform.forward = mapObj.originDirection.GetReal3Ddirection();
    }
    
    public virtual void gainCoin(CoinType type)
    {
        switch (type)
        {
            case CoinType.Gold:
                coins += 50;
                break;
            case CoinType.Silver:
                coins += 5;
                break;
            case CoinType.Brass:
                coins += 1;
                break;
            default:
                break;
        }
    }
    public virtual void takeDamage(int damage, CharacterBase comesFrom)
    {
        var _damage = (damage - def);
        if (_damage <= 0) _damage = 1;
        hp -= _damage;
        if (hp == 0)
        {
            die(comesFrom);
        }
    }
    public virtual void reborn()
    {
        hp = maxHP;
        _died = false;
        GetComponentInChildren<Renderer>().enabled = true;
        foreach (var col in GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
        }
        enterGameMode();
        placeAtMapCell(MapManager.currentMap.playerRebornPos);
    }

    protected bool _died = false;
    public virtual void die(CharacterBase killer) 
    {
        switchAnimation(CharacterAnimation.DIE);
        GetComponentInChildren<Renderer>().enabled = false;
        foreach (var col in GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
        exitGameMode();
        _died = true;
        IEnumerator wait()
        {
            yield return new WaitForSeconds(4f);
            reborn();
        }
        StartCoroutine(wait());
    }
    
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<MapObject>(out var mapObj))
        {
            _colliders.Add(mapObj);
        }
    }
    protected void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<MapObject>(out var mapObj))
        {
            _colliders.Remove(mapObj);
        }
    }
}

/// <summary>
/// 角色泛型基類。输入普通攻击类、技能类，以及角色类本身。
/// </summary>
public abstract class Character<NormalAtkCls, SkillCls> : CharacterBase 
    where SkillCls : Skill<SkillCls>
    where NormalAtkCls : NormalAttackSkill<NormalAtkCls>
{
    protected static GameObject _prefab; //角色模型。 
    public GameObject prefab => ResourcesManager.GetPrefab(this.GetType().Name);
    
    protected NormalAtkCls _normalAtkSkill;
    public NormalAtkCls normalAtkSkill => _normalAtkSkill;
    protected SkillCls _specialSkill;
    public SkillCls specialSkill => _specialSkill;

    protected override void attackAndSkillControl()
    {
        if (!_invisible) //隐身时不可攻击
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (specialSkill != null)
                {
                    if (specialSkill.releaseType == SkillReleaseType.Immediate)
                    {
                        specialSkill.use(); //其他行为与检查已于use完成
                    }
                    else if (specialSkill.releaseType == SkillReleaseType.PressAndClick)
                    {
                        specialSkill.prepare(); //其他行为与检查已于prepare完成
                    }
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (specialSkill != null)
                {
                    if (specialSkill.releaseType == SkillReleaseType.PressAndClick && specialSkill.prepared)
                    {
                        specialSkill.use();
                    }
                    else
                    {
                        normalAtkSkill.use();
                    }
                }
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _normalAtkSkill = Activator.CreateInstance(typeof(NormalAtkCls), new object[] { this }) as NormalAtkCls;
        _specialSkill = Activator.CreateInstance(typeof(SkillCls), new object[] { this }) as SkillCls;
    }
   

}