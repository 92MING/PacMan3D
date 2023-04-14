using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public interface IBombBase { }

public abstract class BombBase<ExplosionCls, ChildCls> : BulletBase<ExplosionCls, ChildCls>, IBombBase
    where ExplosionCls: Explosion
    where ChildCls: BombBase<ExplosionCls, ChildCls>
{
    public virtual float damageRadius => 2.5f;
    public virtual float explodeForce => 3.0f;
    public virtual float timeToExplode => 3.25f;
    protected float _timeRemain;

    public override bool useGravity => true;
    public override bool teammateDamage => true;
    public override bool teammateTrigger => false;
    public static PhysicMaterial physicMaterial => ResourcesManager.GetPhysicMaterial(typeof(ChildCls).Name);

    public override void OnGot()
    {
        base.OnGot();
        _timeRemain = timeToExplode;
    }
    protected override void Awake()
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

        var collider = transform.Find("collider")?.GetComponent<Collider>();
        if (collider is null)
        {
            Debug.LogWarning(this.GetType().Name + " no collider found. Bomb has to have a child obj call 'collider' with collider component");
        }
        else
        {
            collider.isTrigger = false;
            collider.material = physicMaterial;
            collider.gameObject.layer = LayerMask.NameToLayer("BombCollider");
        }

        var trigger = transform.Find("trigger")?.GetComponent<Collider>();
        if (trigger is null)
        {
            Debug.LogWarning(this.GetType().Name + " no trigger found. Bomb has to have a child obj call 'trigger' with collider component");
        }
        else
        {
            trigger.isTrigger = true;
            trigger.gameObject.layer = LayerMask.NameToLayer("BombTrigger");
        }
    }
    protected override void FixedUpdate()
    {
        if (GameManager.isPlaying && !GameManager.isPaused)
        {
            base.FixedUpdate();
            if (_timeRemain > 0)
            {
                _timeRemain -= Time.fixedDeltaTime;
                if (_timeRemain <= 0)
                {
                    explosion(transform.position, findAffectedCharacters());
                }
            }
        }   
    }

    protected CharacterBase[] findAffectedCharacters()
    {
        if (!teammateDamage)
        {
            var affectedChars = Physics.OverlapSphere(transform.position, damageRadius, LayerMask.GetMask("Character")).Select(c => c.GetComponent<CharacterBase>()).Where(c => c != null).ToList();
            {
                foreach (var character in affectedChars)
                {
                    if (character == owner) affectedChars.Remove(character);
                }
            }
            return affectedChars.ToArray();
        }
        else
        {
            return Physics.OverlapSphere(transform.position, damageRadius, LayerMask.GetMask("Character")).Select(c => c.GetComponent<CharacterBase>()).Where(c => c != null).ToArray();
        }
    }
    protected override void explosion(Vector3 position, params CharacterBase[] affectedChars)
    {
        Explosion.Explode<ExplosionCls>(position);
        var _damage = (owner != null) ? damage * owner.atk : damage;
        foreach (var character in affectedChars)
        {
            character.rigidBody.AddForce((character.transform.position - transform.position).normalized * explodeForce, ForceMode.Impulse);
            character.takeDamage(_damage, owner);
        }
        this.ReturnToPool();
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        if (!teammateTrigger && other.TryGetComponent<CharacterBase>(out var comp))
        {
            if (comp == owner) return;
            explosion(transform.position, findAffectedCharacters()); 
        }
        else
        {
            explosion(transform.position, findAffectedCharacters());
        }
    }
}
