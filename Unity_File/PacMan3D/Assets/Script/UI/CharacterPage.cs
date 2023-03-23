using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPage : UIPage
{
    private void Awake()
    {
        base.Awake();
        UIManager.characterPage = this;
    }
}
