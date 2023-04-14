using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class Explosion: PoolMonoObject
{
    public static new int poolSize => 5; //default pool size
    
    protected ParticleSystem _particle = null;
    public ParticleSystem particle
    {
        get
        {
            if (_particle is null)
            {
                _particle = GetComponent<ParticleSystem>();
            }
            return _particle;
        }
    }

    protected virtual void Update()
    {
        if (GameManager.isPlaying && !GameManager.isPaused)
        {
            if (_particle != null && !_particle.IsAlive())
            {
                this.ReturnToPool();
            }
        }
    }

    public override void OnGot()
    {
        base.OnGot();
        particle.Play();
        GameManager.OnGamePause.AddListener(particle.Pause);
        GameManager.OnGameContinue.AddListener(particle.Play);
    }
    public override void OnReturn()
    {
        base.OnReturn();
        particle.Stop();
        GameManager.OnGamePause.RemoveListener(particle.Pause);
        GameManager.OnGameContinue.RemoveListener(particle.Play);
    }

    //可以透过ExplosionBase.Explode 生成
    public static void Explode<T>(Vector3 position) where T: Explosion
    {
        var explosion = PoolManager.GetInstance(typeof(T)) as Explosion;
        explosion.transform.position = position;
    }
}

public abstract class ExplosionGeneric<ChildCls>: Explosion where ChildCls: ExplosionGeneric<ChildCls>
{
    public static new GameObject prefab => ResourcesManager.GetPrefab(typeof(ChildCls).Name);
}
