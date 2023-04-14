using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using MongoDB.Driver;
using MongoDB.Bson;

[System.Serializable]
public enum MapElement
{
    Floor, Wall1, Wall2, Player, Monster
}

public class MapEditPage : UIPage
{
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;

    [SerializeField] private Transform mapPreviewTransform;

    [Header("Map Element")]
    [SerializeField] private Button floorButton;
    [SerializeField] private Button wall1Button;
    [SerializeField] private Button wall2Button;
    [SerializeField] private Button playerButton;
    [SerializeField] private Button monsterButton;
    [SerializeField] private Transform selectArrowTransform;

    [Header("Prefab")]
    [SerializeField] private GameObject mapBoxGameobjectPrefab;

    [Header("Var")]
    // MongoDB collection name
    const string collectionName = "maps";
    public MapElement mapElement;

    MongoClient client = new MongoClient(NetworkManager.connectionString);
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;

    protected override void Awake()
    {
        base.Awake();
        UIManager.mapEditPage = this;

        saveButton.onClick.AddListener(() => SaveButtonFunction());
        loadButton.onClick.AddListener(() => LoadButtonFunction());

        floorButton.onClick.AddListener(delegate { mapElement = MapElement.Floor; SetMapElement(floorButton); });
        wall1Button.onClick.AddListener(delegate { mapElement = MapElement.Wall1; SetMapElement(wall1Button); });
        wall2Button.onClick.AddListener(delegate { mapElement = MapElement.Wall2; SetMapElement(wall2Button); });
        playerButton.onClick.AddListener(delegate { mapElement = MapElement.Player; SetMapElement(playerButton); });
        monsterButton.onClick.AddListener(delegate { mapElement = MapElement.Monster; SetMapElement(monsterButton); });

        CreateEditMap();
    }

    //Map Preview Function
    private void CreateEditMap()
    {
        foreach (Transform child in mapPreviewTransform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < 400; i++)
        {
            GameObject mapBoxGameobject = GameObject.Instantiate(mapBoxGameobjectPrefab);
            mapBoxGameobject.transform.SetParent(mapPreviewTransform, false);
        }
    }

    //Button Function
    private void SaveButtonFunction()
    {
        //TODO: Unlock this when UploadFile done
        //StartCoroutine(UploadFile());

        string id = NetworkManager.DownloadDataToDB(collectionName).Count.ToString();
        GameMap gameMap = new GameMap(id: id, name: "no Name", mapSize: new Vector2Int(20, 20), creatorID: null);
        //TODO
        gameMap.mapCells = new MapComponent [20,20];

        int count = 0;
        foreach(Transform child in mapPreviewTransform)
        {
            MapBox mapBox = child.GetComponent<MapBox>();
            MapElement mapElement = mapBox.mapElement;

            MapComponent mapCellJson = new MapComponent((MapObjectType) 2, Vector2Int.zero, 0, "MetalWall1", null);
            int type = 2;
            string objName = "MetalWall1";

            if (mapElement == MapElement.Wall1)
            {
                mapCellJson.type = (MapObjectType) 2;
                mapCellJson.objName = "MetalWall1";

                type = 2;
                objName = "MetalWall1";
            }
            else if (mapElement == MapElement.Wall2)
            {
                mapCellJson.type = (MapObjectType) 2;
                mapCellJson.objName = "MetalWall2";

                type = 2;
                objName = "MetalWall2";
            }
            else if (mapElement == MapElement.Player)
            {
                mapCellJson.type = (MapObjectType)4;
                mapCellJson.objName = "smile"; //TODO

                type = 4;
                objName = "smile";
            }
            else if (mapElement == MapElement.Monster)
            {
                mapCellJson.type = (MapObjectType) 3;
                mapCellJson.objName = "monster";//TODO

                type = 3;
                objName = "monster";
            }
            else
            {
                mapCellJson.type = (MapObjectType) 1;
                mapCellJson.objName = "Floor";

                type = 1;
                objName = "Floor";
            }

            gameMap.mapCells[count / 20, count % 20] = new MapComponent((MapObjectType) type, Vector2Int.zero, 0, objName, null);
            

            count++;
        }

        string jsonStr = gameMap.serialize().jsonStr();
        jsonStr = JsonUtility.ToJson(gameMap.serialize());
        Debug.Log($"{jsonStr}");
          
        NetworkManager.UploadDataToDB(collectionName, jsonStr);
    }

    private void LoadButtonFunction()
    {
        NetworkManager.DownloadDataToDB(collectionName);
    }

    //Other Functions
    private void SetMapElement(Button button)
    {
        selectArrowTransform.position = button.transform.position + new Vector3(70f, 0, 0);
    }

    /*
    IEnumerator UploadFile()
    {
        WWWForm form = new WWWForm();
        form.AddField("myField", "myData");



        //TODO
        string urlLink = ""; //Fill this link later

        using (UnityWebRequest www = UnityWebRequest.Post(urlLink, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
    */
}
