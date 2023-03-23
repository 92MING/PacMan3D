using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditPage : UIPage
{
    private void Awake()
    {
        base.Awake();
        UIManager.mapEditPage = this;
    }
}
