using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

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

    public abstract int maxHP { get; }
    public abstract int atk { get; }
    public abstract int def { get; }
    public abstract int spd { get; }
    public virtual float gainEnergySpd => 1.0f; //获取能量速度

    protected int _hp;
    public int hp => _hp;

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

    protected HashSet<MapObjectBase> _colliders = new HashSet<MapObjectBase>();

    public virtual float jumpForce => 240.0f; // 跳跃力
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
    public int coins => _coins; // 金幣數量

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

    protected void Awake()
    {
        if (!TryGetComponent(out _rigidbody))
        {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
        }
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.mass = 1.0f;
        _animator = GetComponentInChildren<Animator>();
        _hp = maxHP;
    }

    protected virtual void mouseViewControl()
    {
        _currentMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        // 更新横向视角
        var mouseControl = _currentMouseInput * SystemManager.mouseSensitivety;
        transform.Rotate(axis: transform.up, mouseControl.x, Space.Self);

        //更新垂直视角
        GameManager.camVerticalRotationRatio += mouseControl.y /128f ;
    }
    protected virtual void basicMovementControl()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        _currentInput = new Vector2(h, v);
        _controlDirection = new Vector3(h, 0, v);
        _controlDirection = transform.TransformDirection(_controlDirection.normalized);
    }
    protected virtual void jumpControl()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (touchingFloor) rigidBody.AddForce(transform.up * jumpForce);
        }
    }
    protected virtual void attackAndSkillControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switchAnimation(CharacterAnimation.ATTACK);
        }
    }
    protected void Update()
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
    protected void FixedUpdate()
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

    protected void switchAnimation(CharacterAnimation ani)
    {
        if (ani == currentAni) return;
        _animator.SetInteger("state", (int)ani);
        _currentAni = ani;
    }

    //退出、进入游戏模式
    public void enterGameMode()
    {
        rigidBody.useGravity = true;
        rigidBody.isKinematic = false;
    }
    public void exitGameMode()
    {
        rigidBody.useGravity = false;
        rigidBody.isKinematic = true;
    }
    
    //放置在某个地图格
    public void placeAtMapCell(Vector2Int cellPpos)
    {
        var mapObj = MapManager.GetMapCellAt(cellPpos);
        if (mapObj != null) placeAtMapCell(mapObj);
    }
    public void placeAtMapCell(MapObjectBase mapObj)
    {
        if (mapObj.type != MapObjectType.EMPTY) return; //只能放在地上。 Empty代表地板.
        transform.position = mapObj.transform.position + Vector3.up / 1.98f;
        transform.forward = mapObj.originDirection.GetReal3Ddirection();
    }

    public void reborn()
    {
        //TODO    
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<MapObjectBase>(out var mapObj))
        {
            _colliders.Add(mapObj);
        }
    }
    protected void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<MapObjectBase>(out var mapObj))
        {
            _colliders.Remove(mapObj);
        }
    }
}

/// <summary>
/// 角色泛型基類。输入普通攻击类、技能类，以及角色类本身。
/// </summary>
public abstract class Character<NormalAtkCls, SkillCls, ChildCls> : CharacterBase 
    where SkillCls : Skill<SkillCls>
    where NormalAtkCls : NormalAttackSkill<NormalAtkCls>
    where ChildCls: Character<NormalAtkCls, SkillCls, ChildCls>
{
    protected static GameObject _prefab; //角色模型。 
    public static GameObject prefab => ResourcesManager.GetPrefab(typeof(ChildCls).Name);
    
    protected NormalAtkCls _normalAtkSkill;
    public NormalAtkCls normalAtkSkill => _normalAtkSkill;
    protected SkillCls _specialSkill;
    public SkillCls specialSkill => _specialSkill;

    void Awake()
    {
        base.Awake();
        _normalAtkSkill = Activator.CreateInstance(typeof(NormalAtkCls), new object[] { this }) as NormalAtkCls;
        _specialSkill = Activator.CreateInstance(typeof(SkillCls), new object[] { this }) as SkillCls;
    }
   

}
