using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

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
    public MapElement mapElement;

    private void Awake()
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
    }

    private void LoadButtonFunction()
    {
        
    }

    //Other Functions
    private void SetMapElement(Button button)
    {
        selectArrowTransform.position = button.transform.position + new Vector3(70f, 0, 0);
    }

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
}
