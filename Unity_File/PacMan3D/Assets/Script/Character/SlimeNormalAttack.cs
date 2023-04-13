using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeNormalAttack : NormalAttackSkill<SlimeNormalAttack>
{
    SlimeNormalAttack() { }
    public SlimeNormalAttack(CharacterBase character) : base(character) { }
    public override float timeInterval => 1.0f;

    private float _bombForce => 4.0f;
    private GameObject _bombPrefab => ResourcesManager.GetPrefab("Bomb");

    public override void _use()
    {
        var bomb = UnityEngine.Object.Instantiate(_bombPrefab, thisChar.transform.position + (thisChar.transform.up + thisChar.transform.forward).normalized * 0.5f, Quaternion.identity);
        bomb.GetComponent<Bomb>().owner = thisChar;
        bomb.GetComponent<Rigidbody>().AddForce((thisChar.transform.forward + thisChar.transform.up*0.5f).normalized * _bombForce, ForceMode.Impulse);
    }
}