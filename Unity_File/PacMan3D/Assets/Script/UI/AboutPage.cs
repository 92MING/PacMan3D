using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutPage : UIPage
{
    protected override void Awake()
    {
        base.Awake();
        UIManager.aboutPage = this;
    }
}
