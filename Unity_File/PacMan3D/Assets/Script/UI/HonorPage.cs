using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HonorPage : UIPage
{
    private void Awake()
    {
        base.Awake();
        UIManager.honorPage = this;
    }
}
