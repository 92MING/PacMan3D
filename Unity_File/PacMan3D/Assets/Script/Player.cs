using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家角色
/// </summary>
public class Player : Character
{
    private int _maxHP = 10;
    private int _maxAttack = 1;
    private int _maxDefence = 1;
    private int _maxSpeed = 1;
    public override int maxHP => _maxHP;
    public override int maxAttack => _maxAttack;
    public override int maxDefence => _maxDefence;
    public override int maxSpeed => _maxSpeed;
}
