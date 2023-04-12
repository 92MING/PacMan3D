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
            var dir = MapComponent.GetDirectionFromEnumDirection(direction);
            obj.transform.forward = new Vector3(dir.x, 0, dir.y);
            var component = obj.AddComponent(MapObjClasses[objName]);
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
    public abstract MapObjectType type { get; }
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

public class MonsterRebornPoint: MapObject<MonsterRebornPoint>
{
    public override MapObjectType type => MapObjectType.MONSTER;
}

public class PlayerRebornPoint: MapObject<PlayerRebornPoint>
{
    public override MapObjectType type => MapObjectType.PLAYER;
}

