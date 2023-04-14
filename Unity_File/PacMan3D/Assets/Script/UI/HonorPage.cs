using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HonorPage : UIPage
{
    protected override void Awake()
    {
        base.Awake();
        UIManager.honorPage = this;
    }
}
