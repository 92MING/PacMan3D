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
    ATK,
    SKILL,
    SENSE,
    GET_HIT,
    STUN,
    DIE,
    VICTORY,
}


/// <summary>
/// 角色基類的基類
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[System.Serializable]
public abstract class CharacterBase: MonoBehaviour
{
    protected int _hp;
    public int hp => _hp;
    protected int _attack;
    public int attack => _attack;
    protected int _defence;
    public int defence => _defence;
    protected int _speed;
    public int speed => _speed;
    protected int _energy; //能量， 用于释放技能
    public int energy => _energy;

    protected Rigidbody _rigidbody;
    protected Animator _animator;

    protected float forceAmount = 5f; // 移動力
    protected bool _invisible = false;
    public bool invisible => _invisible; // 是否隱形

    protected void switchAnimation(CharacterAnimation ani)
    {
        _animator.SetInteger("state", (int)ani);
    }
    protected virtual void forward()
    {
        _rigidbody.AddForce(Vector3.forward * forceAmount, ForceMode.Impulse);
        switchAnimation(CharacterAnimation.WALK_FRONT);
    }
    protected virtual void backward()
    {
        _rigidbody.AddForce(Vector3.back * forceAmount, ForceMode.Impulse);
        switchAnimation(CharacterAnimation.WALK_BACK);
    }
    protected virtual void left()
    {
        _rigidbody.AddForce(Vector3.left * forceAmount, ForceMode.Impulse);
        switchAnimation(CharacterAnimation.WALK_LEFT);
    }
    protected virtual void right()
    {
        _rigidbody.AddForce(Vector3.right * forceAmount, ForceMode.Impulse);
        switchAnimation(CharacterAnimation.WALK_RIGHT);
    }
    protected virtual void jump()
    {
        _rigidbody.AddForce(Vector3.up * forceAmount, ForceMode.Impulse);
    }
}

/// <summary>
/// 角色基類
/// </summary>
public abstract class Character<NormalAtkCls, SkillCls> : CharacterBase 
    where SkillCls : Skill<SkillCls>, new()
    where NormalAtkCls : NormalAttackSkill<NormalAtkCls>, new()
{
    public static LinkedList<Type> AllCharacterType = new LinkedList<Type>();
    public static Dictionary<string, GameObject> AllCharacterPrefab = new Dictionary<string, GameObject>(); //character class name, Prefab

    public abstract int maxHP { get; }
    public abstract int maxAttack { get; }
    public abstract int maxDefence { get; }
    public abstract int maxSpeed { get; }
    public virtual float gainEnergySpeed => 1.0f; //获取能量速度
    
    protected NormalAtkCls _normalAtkSkill;
    public NormalAtkCls normalAtkSkill => _normalAtkSkill;
    protected SkillCls _specialSkill;
    public SkillCls specialSkill => _specialSkill;

    void Awake()
    {
        if (!TryGetComponent(out _rigidbody))
        {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
        }
        _animator = GetComponentInChildren<Animator>();
        _hp = maxHP;
        _attack = maxAttack;
        _defence = maxDefence;
        _speed = maxSpeed;
        _normalAtkSkill = new NormalAtkCls();
        _normalAtkSkill.thisChar = this;
        _specialSkill = new SkillCls();
        _specialSkill.thisChar = this;
    }

    void Update()
    {
        if (!GameManager.IsPlaying || GameManager.IsPaused || GameManager.playerCharacter != this) return;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            forward();
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            left();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            backward();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            right();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            jump();
        }
        
        if (!_invisible)
        {
            if (Input.GetMouseButtonDown(0))
            {
                normalAttack();   
            }
            else if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.K))
            {
                useSkill();
            }
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void LoadAllCharacterType()
    {
        foreach (var clsType in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (clsType.IsSubclassOf(typeof(CharacterBase)) && !clsType.IsAbstract)
            {
                AllCharacterType.AddLast(clsType);
                AllCharacterPrefab.Add(clsType.Name, GameManager.LoadPrefab(clsType.Name));
            }
        }
    }
    
    protected virtual void normalAttack()
    {
        switchAnimation(CharacterAnimation.ATK);
    }
    protected virtual void useSkill()
    {
        switchAnimation(CharacterAnimation.SKILL);
    }

}
