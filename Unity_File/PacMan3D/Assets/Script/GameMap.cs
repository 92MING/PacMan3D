using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum MapComponent
{
    wall, bean, monsterInitPos, playerInitPos
}

[System.Serializable]
public class GameMap
{
    public string id;
    public string createrID;
    public Vector2 mapSize;
    public MapComponent[,] mapComponent;
}
