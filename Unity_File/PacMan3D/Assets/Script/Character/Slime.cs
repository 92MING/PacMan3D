using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Character<SlimeNormalAttack, SlimeSkill, Slime>
{
    public override int maxHP => 100;
    public override int atk => 10;
    public override int def => 5;
    public override int spd => 2;
}
