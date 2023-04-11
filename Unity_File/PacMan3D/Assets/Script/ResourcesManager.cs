using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : Manager<ResourcesManager>
{
    private static Dictionary<string, GameObject> _prefabDictionary = new Dictionary<string, GameObject>();
    public static GameObject GetPrefab(string prefabName)
    { 
        if (_prefabDictionary.TryGetValue(prefabName, out GameObject prefab)) return prefab;
        else
        {
            prefab = Resources.Load<GameObject>("Prefab/" + prefabName);
            _prefabDictionary.Add(prefabName, prefab);
            return prefab;
        }
    }
}