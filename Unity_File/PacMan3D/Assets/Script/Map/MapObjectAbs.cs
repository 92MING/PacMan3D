using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class MapObjectAbs : MonoBehaviour
{
    protected static Dictionary<string, Type> MapObjClasses = new Dictionary<string, Type>();
    protected static Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>(); 
    
    public MapComponentDirection originDirection;
    public Vector2Int originMapPos;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void GetAllMapObjectClass()
    {
        foreach (var clsType in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (clsType.IsSubclassOf(typeof(MapObjectAbs)) && !clsType.IsAbstract)
            {
                MapObjClasses.Add(clsType.Name, clsType);
            }
        }
    }

    /// <summary>
    /// Create a map object in game. If pos or direction is not provided, it will use the pos and direction itself.
    /// </summary>
    /// <param name="objName"></param>
    /// <param name="mapPos"></param>
    /// <param name="direction"></param>
    /// <param name="mapSize"></param>
    /// <returns></returns>
    public static MapObjectAbs Create(string objName, Vector2Int mapPos, MapComponentDirection direction, Vector2Int mapSize)
    {
        if (MapObjectAbs.MapObjClasses.ContainsKey(objName))
        {
            if (!prefabs.ContainsKey(objName))
            {
                prefabs.Add(objName, Resources.Load<GameObject>("Prefab/MapObject/" + objName));
            }
            var obj = Instantiate(prefabs[objName], MapManager.mapObj.transform);
            obj.transform.position = MapComponent.GetMapCellPositionByMapPos(mapPos, mapSize);
            var dir = MapComponent.GetDirectionFromEnumDirection(direction);
            obj.transform.forward = new Vector3(dir.x, 0, dir.y);
            var component = obj.AddComponent(MapObjClasses[objName]);
            return component as MapObjectAbs;
        }
        return null;
    }
}

public abstract class MapObject<childCls> : MapObjectAbs where childCls : MapObject<childCls>, new()
{
    public bool breakable => false; // default, override if needed
    public bool eatable => false; // default, override if needed
    public abstract MapComponentType type { get; }

    protected static string _objName = null;
    public static string objName
    {
        get
        {
            if (_objName is null)
            {
                _objName = typeof(childCls).Name;
            }
            return _objName;
        }
    }
    
    public static GameObject prefab
    {
        get
        {
            if (!prefabs.ContainsKey(objName))
            {
                prefabs.Add(objName, Resources.Load<GameObject>("Prefab/MapObject/" + objName));
            }
            return prefabs[objName];
        }
    }

}
