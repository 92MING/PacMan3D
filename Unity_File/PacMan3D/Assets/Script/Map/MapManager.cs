using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 編輯、創建、加載地圖資料
/// </summary>
public class MapManager : Manager<MapManager>
{
    public static Dictionary<string, GameMap> AllMaps = new Dictionary<string, GameMap>(); //id, map
    public static GameMap testMap => AllMaps.TryGetValue("0", out var map)? map: null; // empty map for test

    public static readonly float mapScale = 1.0f; //length scale to each cell

    private static GameObject _mapObj; //在游戏场景里面的"Map.负责装载所有地图Cell物体
    public static GameObject mapObj => _mapObj;

    private static GameMap _currentMap = null; //当前地图
    public static GameMap currentMap => _currentMap;
    private static MapObjectBase[,] _mapCells; //目前地图的所有Cell
    public static MapObjectBase currentPlayerRebornCell => GetMapCellAt(currentMap.playerRebornPos);
    public static GameObject currentPlayerRebornCellObj => GetMapCellObjAt(currentMap.playerRebornPos);

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void LoadAllLocalMaps()
    {
        // 'Map' gameObj for containing all map cells
        _mapObj = new GameObject("Map");

        //load all map json file from Resources/Map
        var mapJsons = Resources.LoadAll<TextAsset>("Map");
        foreach (var mapJson in mapJsons)
        {
            var map = LoadMapFromJson(mapJson.text);
        }
    }

    /// <summary>
    /// get map cell at position(in game scene)
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static MapObjectBase GetMapCellAt(Vector2Int pos)
    {
        if (currentMap is null) return null;
        else if (pos.x < 0 || pos.y < 0 || pos.x >= currentMap.mapSize.x || pos.y >= currentMap.mapSize.y) return null; // out of map
        return _mapCells[pos.x, pos.y];
    }
    public static GameObject GetMapCellObjAt(Vector2Int pos)
    {
        var cell = GetMapCellAt(pos);
        return (cell is null) ? null : cell.gameObject;
    }

    /// <summary>
    /// load map from json string.
    /// </summary>
    /// <param name="json"> json string </param>
    /// <param name="forceUpdate"> update even this map has loaded </param>
    /// <param name="addToAllMap"> add to MapManager.AllMaps </param>
    /// <returns>GameMap</returns>
    public static GameMap LoadMapFromJson(string json, bool forceUpdate=false, bool addToAllMap=true){
        MapJson mapJson = JsonUtility.FromJson<MapJson>(json);
        if (AllMaps.ContainsKey(mapJson.id) && !forceUpdate) return null; // map alreaedy loaded
        GameMap newMap = new GameMap();
        newMap.id = mapJson.id;
        newMap.createrID = mapJson.id;
        newMap.name = mapJson.name;
        var sizeStr = mapJson.mapSize.Split('x');
        newMap.mapSize = new Vector2Int(int.Parse(sizeStr[0]), int.Parse(sizeStr[1]));
        newMap.mapCells = new MapComponent[(int)newMap.mapSize.x, (int)newMap.mapSize.y];
        int count = 0;
        foreach (var cell in mapJson.mapCells){
            var newCell = new MapComponent();
            newCell.type = (MapObjectType)cell.type;
            newCell.direction = (MapObjectDirection)cell.direction;
            newCell.objName = cell.objName;
            newCell.pos = new Vector2Int(count % newMap.mapSize.x, count / newMap.mapSize.x);
            newMap.mapCells[count % newMap.mapSize.x, count / newMap.mapSize.x] = newCell;
            count++;
        }
        if (addToAllMap) AllMaps.Add(newMap.id, newMap);
        return newMap;
    }

    /// <summary>
    /// Create a temporate map which is not added into "AllMaps" and not saved into json file.
    /// Map id is left as null.
    /// </summary>
    /// <param name="mapName"></param>
    /// <param name="mapSize"></param>
    /// <param name="creatorID"></param>
    /// <returns></returns>
    public static GameMap CreateTempMap(Vector2Int mapSize, string mapName=null, string creatorID =null)
    {
        //create a new map
        GameMap newMap = new GameMap();
        newMap.id = null;
        newMap.createrID = creatorID;
        newMap.name = mapName;
        newMap.mapSize = mapSize;
        newMap.mapCells = new MapComponent[(int)mapSize.x, (int)mapSize.y];
        for (int i = 0; i < mapSize.x * mapSize.y; i++)
        {
            var component = new MapComponent();
            component.type = MapObjectType.EMPTY;
            newMap.mapCells[i % mapSize.x, i / mapSize.x] = component;
        }
        return newMap;
    }

    /// <summary>
    /// Save map to json file, and add to "AllMaps". 
    /// If map.id is null, a new mapID will be generated.
    /// You could choose whether it should update to server.
    /// </summary>
    /// <param name="map"></param>
    /// <param name="updateToServer"></param>    
    public static void SaveMap(GameMap map, bool updateToServer = true)
    {
        if (map.id is null) map.id = Guid.NewGuid().ToString("N");

        if (!AllMaps.ContainsKey(map.id)) AllMaps.Add(map.id, map);
        else AllMaps[map.id] = map;
        
        if (updateToServer) NetworkManager.SaveMapToServer(map.serialize());
    }

    /// <summary>
    /// Load the given map to game. If replaceCurrent is set, even there is map exsiting,
    /// the current one will be destroyed and the new is loaded instead.
    /// </summary>
    /// <param name="map"></param>
    /// <param name="replaceCurrent"></param>
    public static void LoadMapToGame(GameMap map, bool replaceCurrent=true)
    {
        if (currentMap != null && !replaceCurrent) return;
        DestroyCurrentMap();
        _currentMap = map;
        _mapCells = new MapObjectBase[map.mapSize.x, map.mapSize.y];
        int count = 0;
        foreach (var cell in map.mapCells)
        {
            var newCell = MapObjectBase.Create(cell.objName, new Vector2Int(count % map.mapSize.x, count / map.mapSize.x), cell.direction, map.mapSize);
            _mapCells[count % map.mapSize.x, count / map.mapSize.x] = newCell;
            count++;
        }
    }

    /// <summary>
    /// destroy current map(all cells). Return if no current map. 
    /// </summary>
    public static void DestroyCurrentMap()
    {
        if (currentMap is null) return;
        _currentMap = null;
        _mapCells = null;
        foreach (Transform child in mapObj.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
