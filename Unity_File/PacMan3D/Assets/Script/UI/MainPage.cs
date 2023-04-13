using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class MainPage : UIPage
{
    public Button AI_Player_Game_Button;
    public Button Player_Player_Game_Button;
    public Button Character_Button;
    public Button Map_Edit_Button;
    public Button Setting_Button;
    public Button Honor_Button;
    public Button About_Button;
    
    void Awake()
    {
        base.Awake();
        UIManager.mainPage = this;
        AI_Player_Game_Button.onClick.AddListener(() => { UIManager.switchToPage(UIManager.UIPages.PLAYER_AI_PAGE); });
        Player_Player_Game_Button.onClick.AddListener(() => { UIManager.switchToPage(UIManager.UIPages.PLAYER_PLAYER_PAGE); });
        Character_Button.onClick.AddListener(() => { UIManager.switchToPage(UIManager.UIPages.CHARACTER_PAGE); });
        Map_Edit_Button.onClick.AddListener(() => { UIManager.switchToPage(UIManager.UIPages.MAP_EDIT_PAGE); });
        Setting_Button.onClick.AddListener(() => { UIManager.switchToPage(UIManager.UIPages.SETTING_PAGE); });
        Honor_Button.onClick.AddListener(() => { UIManager.switchToPage(UIManager.UIPages.HONOR_PAGE); });
        About_Button.onClick.AddListener(() => { UIManager.switchToPage(UIManager.UIPages.ABOUT_PAGE); });
    }
}
