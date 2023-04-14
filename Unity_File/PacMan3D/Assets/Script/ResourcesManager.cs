using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : Manager<ResourcesManager>
{
    private static Dictionary<string, GameObject> _prefabDictionary = new Dictionary<string, GameObject>();
    private static Dictionary<string, Material> _materialDictionary = new Dictionary<string, Material>();
    private static Dictionary<string, PhysicMaterial> _phyMatDictionary = new Dictionary<string, PhysicMaterial>();
    private static Dictionary<string, Sprite> _spriteDictionary = new Dictionary<string, Sprite>();

    private static T LoadResource<T>(string name, ref Dictionary<string, T> dict, params string[] files) where T: Object
    {
        string path;
        if (files.Length == 0) path = typeof(T).Name + "/" + name;
        else path = string.Join("/", files) + "/" + name;
        if (path.EndsWith("/")) path = path.Substring(path.Length - 1);

        if (dict.TryGetValue(name, out T obj)) return obj;
        else
        {
            obj = Resources.Load<T>(path);
            if (obj is null)
            {
                Debug.LogWarning("No such path: " + path);
                return null;
            }
            dict.Add(name, obj);
            return obj;
        }
    }

    public static GameObject GetPrefab(string prefabName)
    {
        return LoadResource<GameObject>(prefabName, ref _prefabDictionary, "Prefab");
    }
    public static Material GetMaterial(string materialName)
    {
        return LoadResource<Material>(materialName, ref _materialDictionary);
    }
    public static Material GetMapObjMaterial(string objName, string materialName)
    {
        return GetMaterial(objName + "/" + materialName);
    }
    public static PhysicMaterial GetPhysicMaterial(string matName)
    {
        return LoadResource<PhysicMaterial>(matName, ref _phyMatDictionary);
    }
    public static Sprite GetSprite(string spriteName)
    {
        return LoadResource<Sprite>(spriteName,ref _spriteDictionary);
    }
}