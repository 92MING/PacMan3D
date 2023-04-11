using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeNormalAttack : NormalAttackSkill<SlimeNormalAttack>
{
    public SlimeNormalAttack(CharacterBase character) : base(character) { }
    public override float timeInterval => 0.5f;
    
    public override void use()
    {
        //TODO
    }
}

public class SlimeSkill: PressAndClickSkill<SlimeSkill>
{
    public SlimeSkill(CharacterBase character) : base(character) { }
    public override int energyConsume => 50;
    
    public override void use()
    {
        //TODO
    }
}

public class Slime : Character<SlimeNormalAttack, SlimeSkill, Slime>
{
    public override int maxHP => 100;
    public override int maxAttack => 10;
    public override int maxDefence => 5;
    public override int maxSpeed => 1;
}
