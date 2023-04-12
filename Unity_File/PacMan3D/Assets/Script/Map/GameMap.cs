using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图物件的类型
/// </summary>
[System.Serializable]
public enum MapObjectType
{
    NULL, // 空，不可行走
    EMPTY, //地板，可行走，且会生成coin
    STATIC, //静物，e.g 墙壁
    MONSTER, //怪物重生点
    PLAYER //玩家重生点，只能有一个
}

/// <summary>
/// 地图物件在地图上的初始方向
/// </summary>
[System.Serializable]
public enum MapObjectDirection
{
    UP, DOWN, LEFT, RIGHT
}

[System.Serializable]
public class MapComponent
{
    public MapObjectType type;
    public string objName = null; // specific object name, eg "Wall", "Monster1"
    public MapObjectDirection direction;

    public Vector2 getRealDirection()
    {
        return GetDirectionFromEnumDirection(direction);
    }
    public static Vector2 GetDirectionFromEnumDirection(MapObjectDirection direction)
    {
        switch (direction)
        {
            case MapObjectDirection.UP:
                return Vector2Int.up;
            case MapObjectDirection.DOWN:
                return Vector2Int.down;
            case MapObjectDirection.LEFT:
                return Vector2Int.left;
            case MapObjectDirection.RIGHT:
                return Vector2Int.right;
        }
        return Vector2Int.up; //default
    }
    public static Vector3 GetMapCellPositionByMapPos(Vector2Int mapPos, Vector2 mapSize)
    {
        var pos = new Vector3(mapPos.x - mapSize.x / 2.0f, mapPos.y - mapSize.y / 2.0f, 0);
        pos *= MapManager.mapScale;
        return pos;
    }
}

[System.Serializable]
public class GameMap
{
    public string id = null;
    public string createrID;
    public string name;
    
    public Vector2Int mapSize;
    public MapComponent[,] mapCells; //x,y

    public MapJson serialize()
    {
        var jsonMap = new MapJson();
        jsonMap.id = id;
        jsonMap.name =name;
        jsonMap.creatorID = createrID;
        jsonMap.mapSize = string.Format("{0}x{1}", mapSize.x, mapSize.y);
        jsonMap.mapCells = new MapCellJson[(int)mapSize.x * (int)mapSize.y];
        int count = 0;
        foreach (var cell in mapCells)
        {
            var newCell = new MapCellJson();
            newCell.type = (int)cell.type;
            newCell.direction = (int)cell.direction;
            newCell.objName = cell.objName;
            count++;
        }
        return jsonMap;
    }
}

/// <summary>
/// For changing map cell to json
/// </summary>
[System.Serializable]
public class MapCellJson
{
    public int type;
    public string objName;
    public int direction;
}

/// <summary>
/// For changing map to json
/// </summary>
[System.Serializable]
public class MapJson
{
    public string id;
    public string creatorID;
    public string name;
    public string mapSize;
    public MapCellJson[] mapCells;

    public string jsonStr()
    {
        return JsonUtility.ToJson(this);
    }
}
