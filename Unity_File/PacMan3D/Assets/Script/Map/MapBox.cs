using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapBox : MonoBehaviour
{
    public MapElement mapElement;

    public Sprite floorSprite;
    public Sprite wall1Sprite;
    public Sprite wall2Sprite;
    public Sprite playerSprite;
    public Sprite monsterSprite;

    void Awake()
    {
        transform.GetComponent<Button>().onClick.AddListener(delegate 
        {
            mapElement = GameObject.Find("mapEditPage").GetComponent<MapEditPage>().mapElement;
            transform.GetComponent<Image>().sprite = MapEleemntSprite();
        });

        //mapElement = MapElement.Floor;
        //transform.GetComponent<Image>().sprite = MapEleemntSprite();
    }

    private Sprite MapEleemntSprite()
    {
        if (mapElement == MapElement.Wall1)
        {
            return wall1Sprite;
        }
        if (mapElement == MapElement.Wall2)
        {
            return wall2Sprite;
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
