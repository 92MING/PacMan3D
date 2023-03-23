using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPage : UIPage
{
    private void Awake()
    {
        base.Awake();
        UIManager.settingPage = this;
    }
}
