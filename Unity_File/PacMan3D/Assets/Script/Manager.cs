using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SingleManager: MonoBehaviour 
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnBeforeSceneLoadRuntimeMethod()
    {
        foreach(var clsType in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (clsType.IsSubclassOf(typeof(SingleManager)) && !clsType.IsGenericType)
            {
                var obj = new GameObject(clsType.Name);
                obj.AddComponent(clsType);
                DontDestroyOnLoad(obj);
            }
        }
    }
}

/// <summary>
/// 管理器基類
/// </summary>
/// <typeparam name="Cls"></typeparam>
public class Manager<Cls> : SingleManager where Cls : Manager<Cls>, new()
{
    protected static Cls _instance = null;
    public static Cls instance
    {
        get
        {
            if (_instance is null) _instance = new Cls();
            return _instance;
        }
    }
    protected void Awake()
    {
        _instance = this as Cls;
    }
    
    public static GameObject LoadPrefab(string prefabName)
    {
        return Resources.Load<GameObject>("Prefab/" + prefabName);
    }
} 
