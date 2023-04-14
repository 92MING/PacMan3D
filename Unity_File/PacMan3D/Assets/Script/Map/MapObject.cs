using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Map Object base class
/// </summary>
public abstract class MapObject : PoolMonoObject
{
    protected static Dictionary<string, Type> AllMapObjCls = new Dictionary<string, Type>(); //所有地图物件类

    public abstract MapObjectType type { get; }
    public Vector2Int originPos;
    public MapObjectDirection originDirection;

    protected string _materialName = null;
    public string materialName => _materialName;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void GetAllMapObjectClass()
    {
        foreach (var clsType in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (clsType.IsSubclassOf(typeof(MapObject)) && !clsType.IsAbstract)
            {
                AllMapObjCls.Add(clsType.Name, clsType);
            }
        }
    }

    public virtual void setMaterial(string materialName)
    {
        if (materialName is null || materialName == "") return;
        if (materialName == _materialName) return;
        _materialName = materialName;
        var mat = ResourcesManager.GetMapObjMaterial(this.GetType().Name, materialName);
        if (mat != null)
        {
            var renderer = GetComponentInChildren<Renderer>(true);
            renderer.material = mat;
        }
    }
    protected virtual void OnCreate() { } // 重写来定义生成时的行为

    /// <summary>
    /// Create a map object in game. If pos or direction is not provided, it will use the pos and direction itself.
    /// </summary>
    /// <param name="objName"></param>
    /// <param name="mapPos"></param>
    /// <param name="direction"></param>
    /// <param name="mapSize"></param>
    /// <returns></returns>
    public static MapObject Create(string objName, Vector2Int mapPos, MapObjectDirection direction, Vector2Int mapSize, string materialName=null)
    {
        if (MapObject.AllMapObjCls.ContainsKey(objName))
        {
            var mapObj = (PoolManager.GetInstance(AllMapObjCls[objName]) as MapObject);
            var obj = mapObj.gameObject;
            obj.name = objName + mapPos.ToString();
            obj.transform.parent = MapManager.mapObj.transform;
            obj.transform.position = MapComponent.GetMapCellPositionByMapPos(mapPos, mapSize);
            obj.transform.forward = direction.GetReal3Ddirection();

            mapObj.originPos = mapPos;
            mapObj.originDirection = direction;
            if (materialName != null) mapObj.setMaterial(materialName);
            mapObj.OnCreate();
            return mapObj;
        }
        else
        {
            Debug.LogWarning("No objName: " + objName);
            return null;
        }
        
    }
}

public abstract class MapObjectGeneric<ChildCls> :MapObject where ChildCls: MapObjectGeneric<ChildCls>
{
    public static string objName => typeof(ChildCls).Name;
    public static new GameObject prefab => ResourcesManager.GetPrefab(objName);
}

/// <summary>
/// 空物件，包括地板等
/// </summary>
/// <typeparam name="ChildCls"></typeparam>
public abstract class EmptyMapObject<ChildCls> : MapObjectGeneric<ChildCls> where ChildCls : EmptyMapObject<ChildCls>
{
    public override MapObjectType type => MapObjectType.EMPTY;
}

/// <summary>
/// 静态物件，包括墙壁等
/// </summary>
/// <typeparam name="ChildCls"></typeparam>
public abstract class StaticMapObject<ChildCls> : MapObjectGeneric<ChildCls> where ChildCls : StaticMapObject<ChildCls>
{
    public override MapObjectType type => MapObjectType.STATIC;
}


public interface IGenerateCoinMapObject { }
public static class IGenerateCoinMapObjectExtent
{
    public static void OnCoinEaten(this IGenerateCoinMapObject obj)
    {
        obj.GetType().GetMethod("_OnCoinEaten", BindingFlags.NonPublic| BindingFlags.Instance).Invoke(obj, null);
    }
}
public abstract class GenerateCoinMapObject<ChildCls> : EmptyMapObject<ChildCls>, IGenerateCoinMapObject
    where ChildCls : GenerateCoinMapObject<ChildCls>
{
    protected float _timeInterval = 0.0f;
    protected float randomInterval => UnityEngine.Random.Range(2.0f, 12.0f);
    protected bool _hasCoin = false;

    public override void OnGot()
    {
        _timeInterval = randomInterval;
    }

    protected virtual void FixedUpdate()
    {
        if (GameManager.isPlaying && !GameManager.isPaused)
        {
            if (!_hasCoin)
            {
                _timeInterval -= Time.fixedDeltaTime;
                if (_timeInterval <= 0)
                {
                    if (GameManager.currentCoinNum < MapManager.maxCoinNumForCurrentMap)
                    {
                        var value = UnityEngine.Random.Range(1, 11);
                        if (value <= 5) GenerateCoin();
                        else
                        {
                            _timeInterval = randomInterval;
                        }
                    }
                }
            }
        }
    }

    public virtual void GenerateCoin()
    {
        var coin = PoolManager.GetInstance<Coin>();
        coin.thisMapObj = this;
        coin.transform.position = transform.position + Vector3.up;

        var value = UnityEngine.Random.Range(1, 101);
        if (value <= 89) coin.setCoinType(CoinType.Brass);
        else if (value <= 98) coin.setCoinType(CoinType.Silver);
        else coin.setCoinType(CoinType.Gold);

        _hasCoin = true;
    }
    /// <summary>
    /// real OnCoinEaten
    /// </summary>
    protected virtual void _OnCoinEaten()
    {
        _hasCoin = false;
        _timeInterval = randomInterval;
    }
}
