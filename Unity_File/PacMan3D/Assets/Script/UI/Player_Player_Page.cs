using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Player_Page : UIPage
{
    protected override void Awake()
    {
        base.Awake();
        UIManager.playerPlayerPage = this;
    }
}
