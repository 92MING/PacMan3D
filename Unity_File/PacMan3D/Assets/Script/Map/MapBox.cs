using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapBox : MonoBehaviour
{
    public MapElement mapElement;

    public Sprite floorSprite;
    public Sprite wallSprite;
    public Sprite playerSprite;
    public Sprite monsterSprite;

    void Awake()
    {
        transform.GetComponent<Button>().onClick.AddListener(delegate 
        {
            mapElement = GameObject.Find("mapEditPage").GetComponent<MapEditPage>().mapElement;
            transform.GetComponent<Image>().sprite = MapEleemntSprite();
        });
    }

    private Sprite MapEleemntSprite()
    {
        if (mapElement == MapElement.Wall)
        {
            return wallSprite;
        }
        if (mapElement == MapElement.Player)
        {
            return playerSprite;
        }
        if (mapElement == MapElement.Monster)
        {
            return monsterSprite;
        }
        
        return floorSprite;
        
    }
}
