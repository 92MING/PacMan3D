using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSkill : PressAndClickSkill<SlimeSkill>
{
    SlimeSkill() { }
    public SlimeSkill(CharacterBase character) : base(character) { }
    public override int energyConsume => 50;

    public override void _use()
    {
        //TODO
    }
}