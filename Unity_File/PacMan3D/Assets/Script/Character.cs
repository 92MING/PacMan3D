using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色基類
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[System.Serializable]
public abstract class Character: MonoBehaviour
{
    public abstract int maxHP { get; }
    public abstract int maxAttack { get; }
    public abstract int maxDefence { get; }
    public abstract int maxSpeed { get; }

    private int _hp;
    public int hp => _hp;
    private int _attack;
    public int attack => _attack;
    private int _defence;
    public int defence => _defence;
    private int _speed;
    public int speed => _speed;
    private Rigidbody2D _rigidbody;

    void Awake()
    {
        if (!TryGetComponent(out _rigidbody))
        {
            _rigidbody = new Rigidbody2D();
        }
        _hp = maxHP;
        _attack = maxAttack;
        _defence = maxDefence;
        _speed = maxSpeed;
    }
}
