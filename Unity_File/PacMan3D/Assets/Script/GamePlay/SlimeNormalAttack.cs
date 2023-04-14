using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBomb: BombBase<SmallExplosion, SlimeBomb> 
{
    public override int damage => 3;
}

public class SlimeNormalAttack : NormalAttackSkill<SlimeNormalAttack>
{
    SlimeNormalAttack() { }
    public SlimeNormalAttack(CharacterBase character) : base(character) { }
    public override float timeInterval => 1.75f;

    private float _bombShootForce => 2.5f;

    public override void _use()
    {
        Vector3 bombDir = Vector3.forward;
        if (GameManager.playerCharacter == thisChar)
        {
            bombDir = (GameManager.gameCamera.transform.forward + GameManager.gameCamera.transform.up * 0.5f).normalized;
        }
        else
        {
            //control by AI 
        }
        SlimeBomb.Shoot(thisChar.transform.position + (thisChar.transform.up + thisChar.transform.forward).normalized * 0.4f, bombDir, _bombShootForce, thisChar);
    }
}