using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeNormalAttack : NormalAttackSkill<SlimeNormalAttack>
{
    SlimeNormalAttack() { }
    public SlimeNormalAttack(CharacterBase character) : base(character) { }
    public override float timeInterval => 0.5f;

    public override void use()
    {
        //TODO
    }
}