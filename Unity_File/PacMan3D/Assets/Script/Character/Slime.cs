using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Character<>
{
    public override int maxHP => 100;
    public override int maxAttack => 10;
    public override int maxDefence => 5;
    public override int maxSpeed => 1;
    
    protected override void normalAttack()
    {
        base.normalAttack();
    }
    protected override void useSkill()
    {
        base.useSkill();
    }
}
