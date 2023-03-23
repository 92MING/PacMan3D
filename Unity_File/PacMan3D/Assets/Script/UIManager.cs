using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// 管理UI
/// </summary>
[Serializable]
public class UIManager : Manager<UIManager>
{
    private GameObject _UIPrefab;
    private Canvas _canvas;
    public static UnityEvent<string> OnLanguageChanged = new UnityEvent<string>();
    
    private void Awake()
    {
        _UIPrefab = UIManager.LoadPrefab("UIPrefab");
        var obj = Instantiate(_UIPrefab);
        obj.name = "UI";
        _canvas = obj.GetComponent<Canvas>();
    }
}
