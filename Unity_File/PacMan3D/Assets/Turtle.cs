using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : Character<SlimeNormalAttack, SlimeSkill>
{
    public override int maxHP => 200;
    public override int atk => 5;
    public override int def => 10;
    public override int spd => 2;
}
