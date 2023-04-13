using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Map Object base class
/// </summary>
public abstract class MapObjectBase : MonoBehaviour
{
    protected static Dictionary<string, Type> MapObjClasses = new Dictionary<string, Type>(); //所有地图物件类
    public abstract MapObjectType type { get; }
    public Vector2Int originPos;
    public MapObjectDirection originDirection;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void GetAllMapObjectClass()
    {
        foreach (var clsType in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (clsType.IsSubclassOf(typeof(MapObjectBase)) && !clsType.IsAbstract)
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
    public static MapObjectBase Create(string objName, Vector2Int mapPos, MapObjectDirection direction, Vector2Int mapSize)
    {
        if (MapObjectBase.MapObjClasses.ContainsKey(objName))
        {
            var obj = Instantiate(ResourcesManager.GetPrefab(objName), MapManager.mapObj.transform);
            obj.transform.position = MapComponent.GetMapCellPositionByMapPos(mapPos, mapSize);
            obj.transform.forward = direction.GetReal3Ddirection();
            
            var component = obj.AddComponent(MapObjClasses[objName]) as MapObjectBase;
            component.originPos = mapPos;
            component.originDirection = direction;
            if (component.type == MapObjectType.EMPTY) //is floor
            {
                obj.transform.position = obj.transform.position + Vector3Int.down;
            }
            return component as MapObjectBase;
        }
        return null;
    }
}

/// <summary>
/// Map Object base generic class. Input the class itself.
/// </summary>
/// <typeparam name="ChildCls"></typeparam>
public abstract class MapObject<ChildCls> : MapObjectBase where ChildCls : MapObject<ChildCls>
{
    public static string objName => typeof(ChildCls).Name; //物件名稱，实际上就是类名
    public static GameObject prefab => ResourcesManager.GetPrefab(objName);
}

public abstract class NoneMapObject : MapObject<NoneMapObject>
{
    public override MapObjectType type => MapObjectType.NULL;
}

public abstract class EmptyMapObject<ChildCls> : MapObject<ChildCls> where ChildCls : EmptyMapObject<ChildCls>
{
    public override MapObjectType type => MapObjectType.EMPTY;
    public virtual bool walkable => true;  // default, override if needed
}

public abstract class StaticMapObject<ChildCls> : MapObject<ChildCls> where ChildCls : StaticMapObject<ChildCls>
{
    public override MapObjectType type => MapObjectType.STATIC;
    public virtual bool breakable => false;     // default, override if needed
    public virtual bool eatable => false;   // default, override if needed
}

