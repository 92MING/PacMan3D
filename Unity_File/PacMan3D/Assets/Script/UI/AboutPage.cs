using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutPage : UIPage
{
    private void Awake()
    {
        base.Awake();
        UIManager.aboutPage = this;
    }
}
