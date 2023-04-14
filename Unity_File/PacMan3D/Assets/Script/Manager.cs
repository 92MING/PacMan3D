using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 管理器基類
/// </summary>
public class ManagerBase: MonoBehaviour 
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnBeforeSceneLoadRuntimeMethod()
    {
        foreach(var clsType in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (clsType.IsSubclassOf(typeof(ManagerBase)) && !clsType.IsGenericType)
            {
                var obj = new GameObject(clsType.Name);
                obj.AddComponent(clsType);
                DontDestroyOnLoad(obj);
            }
        }
    }
}

/// <summary>
/// 管理器泛型基類。输入管理器类本身。
/// </summary>
/// <typeparam name="Cls"></typeparam>
public class Manager<Cls> : ManagerBase where Cls : Manager<Cls>, new()
{
    protected static Cls _instance = null;
    public static Cls instance => _instance;
    
    protected virtual void Awake()
    {
        if (_instance is null) _instance = this as Cls;
        else Destroy(gameObject);
    }  
} 
