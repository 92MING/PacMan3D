using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class MapObject : MonoBehaviour
{
    private static Dictionary<string, Type> MapObjClasses = new Dictionary<string, Type>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void GetAllMapObjectClass()
    {
        foreach (var clsType in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (clsType.IsSubclassOf(typeof(MapObject)) && !clsType.IsAbstract)
            {
                MapObjClasses.Add(clsType.Name, clsType);
            }
        }
    }
    
    public static MapObject CreateMapObject(string name, Vector3 pos, Vector2 direction)
    {
        if (MapObjClasses.ContainsKey(name))
        {
            var obj = new GameObject(name);
            obj.transform.position = pos;
            obj.transform.rotation = Quaternion.LookRotation(direction);
            return obj.AddComponent(MapObjClasses[name]) as MapObject;
        }
        return null;
    }

}
