using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子弹基类
/// </summary>
public abstract class BulletBase<ExplosionCls, ChildCls>: PoolMonoObject 
    where ExplosionCls: Explosion
    where ChildCls: BulletBase<ExplosionCls, ChildCls>
{
    public static new int poolSize => 5;
    public static new GameObject prefab => ResourcesManager.GetPrefab(typeof(ChildCls).Name);

    public CharacterBase owner; //子弹的拥有者
    public virtual bool teammateDamage => false; 
    public virtual bool teammateTrigger => false;
    public abstract int damage { get; } //子弹伤害

    public virtual float mass => 0.3f;
    public virtual bool useGravity => false;
    public virtual RigidbodyConstraints constaints => RigidbodyConstraints.FreezeRotation;
    protected Rigidbody _rigidbody;
    
    protected virtual void Awake()
    {
        if (!TryGetComponent<Rigidbody>(out var body))
        {
            body = gameObject.AddComponent<Rigidbody>();
        }
        body.useGravity = useGravity;
        body.constraints = constaints;
        body.isKinematic = false;
        body.mass = mass;
        _rigidbody = body;

        var trigger = transform.Find("trigger")?.GetComponent<Collider>();
        if (trigger is null)
        {
            Debug.LogWarning(this.GetType().Name + " no trigger found. Bullet has to have a child obj call 'trigger' with collider component");
        }
        else
        {
            trigger.isTrigger = true;
            trigger.gameObject.layer = LayerMask.NameToLayer("BulletTrigger");
        }
    }
    protected virtual void FixedUpdate()
    {
        if (GameManager.isPlaying && !GameManager.isPaused)
        {
            if (GameManager.OutOfGameArea(transform.position)) this.ReturnToPool();
        }
        
    }

    public override void OnGot()
    {
        base.OnGot();
        GameManager.OnGamePause.AddListener(DeactivePhysics);
        GameManager.OnGameContinue.AddListener(ActivePhysics);
    }
    public override void OnReturn()
    {
        base.OnReturn();
        GameManager.OnGamePause.RemoveListener(DeactivePhysics);
        GameManager.OnGameContinue.RemoveListener(ActivePhysics);
    }
    
    protected void ActivePhysics()
    {
        if (_rigidbody != null)
        {
            _rigidbody.useGravity = useGravity;
            _rigidbody.isKinematic = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    protected void DeactivePhysics()
    {
        if (_rigidbody != null)
        {
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
            _rigidbody.constraints = constaints;
        }
    }

    protected virtual void explosion(Vector3 position, params CharacterBase[] affectedChars)
    {
        Explosion.Explode<ExplosionCls>(position);
        var _damage = (owner != null) ? damage * owner.atk : damage;
        foreach (var character in affectedChars)
        {
            character.takeDamage(_damage, owner);
        }
        this.ReturnToPool();
    }

    public static void Shoot(Vector3 originPos, Vector3 direction, float force, CharacterBase owner)
    {
        var bullet = PoolManager.GetInstance<ChildCls>();
        bullet.transform.position = originPos;
        bullet.owner = owner;
        bullet._rigidbody.AddForce(direction.normalized * force, ForceMode.Impulse);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CharacterBase>(out var affectedChar))
        {
            explosion(transform.position, affectedChar);
        }
        else
        {
            explosion(transform.position);
        }
    }
}
