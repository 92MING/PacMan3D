using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditPage : UIPage
{
    protected override void Awake()
    {
        base.Awake();
        UIManager.mapEditPage = this;
    }
}
