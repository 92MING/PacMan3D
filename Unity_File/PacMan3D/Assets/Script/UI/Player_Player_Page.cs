using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Player_Page : UIPage
{
    public int pageIndex = 0;

    public int pageNumber = 3;

    public Button previousButton;
    public Button nextButton;

    public Image mapPreviewImage;
    public Text mapNameText;

    public List<Sprite> mapPreviewSpriteList;

    protected override void Awake()
    {
        base.Awake();
        UIManager.playerPlayerPage = this;

        previousButton.onClick.AddListener(() => PreviousButtonFunction());
        nextButton.onClick.AddListener(() => NextButtonFunction());
    }

    private void PreviousButtonFunction()
    {
        if (pageIndex == 0)
            return;

        pageIndex--;
        mapPreviewImage.sprite = mapPreviewSpriteList[pageIndex];
        mapNameText.text = $"Map {pageIndex + 1}";
    }
    private void NextButtonFunction()
    {
        if (pageIndex == pageNumber - 1)
            return;

        pageIndex++;
        mapPreviewImage.sprite = mapPreviewSpriteList[pageIndex];
        mapNameText.text = $"Map {pageIndex + 1}";
    }
}