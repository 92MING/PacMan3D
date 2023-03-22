using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum MapComponentType
{
    NULL, EMPTY, OBJECT, MONSTER, PLAYER
}
[System.Serializable]
public class MapComponent {
    public MapComponentType type;
    public string className;
}

[System.Serializable]
public class GameMap
{
    public string id;
    public string createrID;
    
    public Vector2 mapSize;
    public LinkedList<MapComponent> mapComponents;
}
