using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum MapElement
{
    Floor, Wall, Player, Monster
}

public class MapEditPage : UIPage
{
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;

    [SerializeField] private Transform mapPreviewTransform;

    [Header("Map Element")]
    [SerializeField] private Button floorButton;
    [SerializeField] private Button wallButton;
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

        SetMapElement(floorButton);

        floorButton.onClick.AddListener(delegate { mapElement = MapElement.Floor; SetMapElement(floorButton); });
        wallButton.onClick.AddListener(delegate { mapElement = MapElement.Wall; SetMapElement(wallButton); });
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

    }

    private void LoadButtonFunction()
    {
        
    }

    //Other Functions
    private void SetMapElement(Button button)
    {
        if (button == floorButton)
        {
            selectArrowTransform.localPosition = new Vector3(54.8f, 147, 0);
        }
        else if (button == wallButton)
        {
            selectArrowTransform.localPosition = new Vector3(54.8f, -106.8f + 177, 0);
        }
        else if (button == playerButton)
        {
            selectArrowTransform.localPosition = new Vector3(54.8f, -183.6f + 177, 0);
        }
        else if (button == monsterButton)
        {
            selectArrowTransform.localPosition = new Vector3(54.8f, -260.4f + 177, 0);
        }
    }
}
