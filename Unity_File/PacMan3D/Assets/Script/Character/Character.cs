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
public abstract class CharacterBase: MonoBehaviour
{
    public static LinkedList<Type> AllCharacterType = new LinkedList<Type>();

    public abstract int maxHP { get; }
    public abstract int maxAttack { get; }
    public abstract int maxDefence { get; }
    public abstract int maxSpeed { get; }
    public virtual float gainEnergySpeed => 1.0f; //获取能量速度

    protected int _hp;
    public int hp => _hp;
    protected int _attack;
    public int attack => _attack;
    protected int _defence;
    public int defence => _defence;
    protected int _speed;
    public int speed => _speed;
    protected int _energy; //能量， 用于释放技能。[0, 100]
    public int energy { 
        get { return _energy; } 
        set { _energy = ( (value <0) ? 0 : ((value> 100) ? 100: value) ); } 
    }

    protected Rigidbody _rigidbody;
    public Rigidbody rigidBody => _rigidbody;
    protected Animator _animator;
    public Animator animator => _animator;

    protected float forceAmount = 5f; // 移動力
    protected bool _invisible = false;
    public bool invisible => _invisible; // 是否隱形

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
        _animator = GetComponentInChildren<Animator>();
        _hp = maxHP;
        _attack = maxAttack;
        _defence = maxDefence;
        _speed = maxSpeed;
    }

    protected void switchAnimation(CharacterAnimation ani)
    {
        _animator.SetInteger("state", (int)ani);
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

    void FixedUpdate()
    {
        if (!GameManager.IsPlaying || GameManager.IsPaused || GameManager.playerCharacter != this) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        var forceDirection = new Vector3(h, 0, v);
        if (h > 0.05f || h < 0.05f || v > 0.05f || v < -0.05f) rigidBody.AddForce(forceDirection * forceAmount);
        rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, maxSpeed);
        
        if (!_invisible)
        {
            
        }
    }
   

}
