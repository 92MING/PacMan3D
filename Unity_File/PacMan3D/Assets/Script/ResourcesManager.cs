using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : Manager<ResourcesManager>
{
    private static Dictionary<string, GameObject> _prefabDictionary = new Dictionary<string, GameObject>();
    private static Dictionary<string, Material> _materialDictionary = new Dictionary<string, Material>();
    
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

    public static Material GetMaterial(string materialName)
    {
        if (_materialDictionary.TryGetValue(materialName, out Material material)) return material;
        else
        {
            material = Resources.Load<Material>("Material/" + materialName);
            _materialDictionary.Add(materialName, material);
            return material;
        }
    }
}