using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Player_Page : UIPage
{
    private void Awake()
    {
        base.Awake();
        UIManager.playerPlayerPage = this;
    }
}
