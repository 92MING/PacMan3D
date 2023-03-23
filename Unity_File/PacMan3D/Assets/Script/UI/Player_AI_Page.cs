using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AI_Page : UIPage
{
    private void Awake()
    {
        base.Awake();
        UIManager.playerAIPage = this;
    }
}
