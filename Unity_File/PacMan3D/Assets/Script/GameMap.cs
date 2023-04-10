using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

[System.Serializable]
public enum MapComponentType
{
    NULL, EMPTY, OBJECT, MONSTER, PLAYER
}

[System.Serializable]
public enum MapComponentDirection
{
    UP, DOWN, LEFT, RIGHT
}

[System.Serializable]
public class MapComponent 
{
    public MapComponentType type;
    public string objName = null; // specific object name, eg "Wall", "Monster1"
    public MapComponentDirection direction;

    public Vector2 getRealDirection()
    {
        return GetDirectionFromEnumDirection(direction);
    }
    public static Vector2 GetDirectionFromEnumDirection(MapComponentDirection direction)
    {
        switch (direction)
        {
            case MapComponentDirection.UP:
                return Vector2Int.up;
            case MapComponentDirection.DOWN:
                return Vector2Int.down;
            case MapComponentDirection.LEFT:
                return Vector2Int.left;
            case MapComponentDirection.RIGHT:
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

[System.Serializable]
public class MapCellJson
{
    public int type;
    public string objName;
    public int direction;
}

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
