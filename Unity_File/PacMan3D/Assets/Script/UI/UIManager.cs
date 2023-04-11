using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 管理UI
/// </summary>
[Serializable]
public class UIManager : Manager<UIManager>
{
    public enum UIPages
    {
        MAIN_PAGE,
        PLAYER_AI_PAGE,
        PLAYER_PLAYER_PAGE,
        CHARACTER_PAGE,
        MAP_EDIT_PAGE,
        SETTING_PAGE,
        HONOR_PAGE,
        ABOUT_PAGE,
    }

    private GameObject _UIPrefab;
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    private Stack<UIPages> _pageRecord = new Stack<UIPages>();
        
    public static UIPages currentPage => instance._pageRecord.Peek();
    public static UIPages secondCurrentPage => instance._pageRecord.Count > 1 ? instance._pageRecord.ToArray()[1] : UIPages.MAIN_PAGE;
    public static MainPage mainPage;
    public static Player_AI_Page playerAIPage;
    public static Player_Player_Page playerPlayerPage;
    public static CharacterPage characterPage;
    public static MapEditPage mapEditPage;
    public static SettingPage settingPage;
    public static HonorPage honorPage;
    public static AboutPage aboutPage;
    public static Button returnButton;

    void Awake()
    {
        base.Awake();
        _UIPrefab = ResourcesManager.GetPrefab("UIPrefab");
        var obj = Instantiate(_UIPrefab);
        obj.name = "UI";
        _canvas = obj.GetComponent<Canvas>();
        _canvasGroup = obj.GetComponent<CanvasGroup>();
        returnButton = obj.transform.Find("return").GetComponent<Button>();
        foreach (Transform child in obj.transform)
        {
            child.gameObject.SetActive(true);
        }
    }
    void Start()
    {
        playerAIPage.setVisible(false);
        playerPlayerPage.setVisible(false);
        characterPage.setVisible(false);
        mapEditPage.setVisible(false);
        settingPage.setVisible(false);
        honorPage.setVisible(false);
        aboutPage.setVisible(false);
        mainPage.setVisible(true);
        returnButton.onClick.AddListener(backToPrevPage);
        returnButton.gameObject.SetActive(false);
        mainPage.OnEnter.AddListener(() => { returnButton.gameObject.SetActive(false); });
        mainPage.OnComeBack.AddListener(() => { returnButton.gameObject.SetActive(false); });
        mainPage.OnExit.AddListener(() => { returnButton.gameObject.SetActive(true); });
        _pageRecord.Push(UIPages.MAIN_PAGE);
    }
    
    private static UIPage _getPageInsByEnum(UIPages targetPage)
    {
        switch (targetPage)
        {
            case UIPages.MAIN_PAGE:
                return mainPage;
            case UIPages.PLAYER_AI_PAGE:
                return playerAIPage;
            case UIPages.PLAYER_PLAYER_PAGE:
                return playerPlayerPage;
            case UIPages.CHARACTER_PAGE:
                return characterPage;
            case UIPages.MAP_EDIT_PAGE:
                return mapEditPage;
            case UIPages.SETTING_PAGE:
                return settingPage;
            case UIPages.HONOR_PAGE:
                return honorPage;
            case UIPages.ABOUT_PAGE:
                return aboutPage;
            default:
                return null;
        }
    }
    public enum SwitchMode
    {
        ENTER, EXIT, RETURN, RETURN_EXIT, NULL
    }
    private static void _switchPage(UIPage page, SwitchMode mode)
    {
        var rectTransform = page.GetComponent<RectTransform>();

        var start = new Vector3(mode == SwitchMode.RETURN ? -0.5f : mode == SwitchMode.ENTER ? 1.5f : 0.5f, 0.5f, rectTransform.position.z);
        var end = new Vector3(mode == SwitchMode.EXIT ? -0.5f : mode==SwitchMode.RETURN_EXIT? 1.5f: 0.5f, 0.5f, rectTransform.position.z);
        var screen = new Vector3(Screen.width, Screen.height, 1);
        start.x *= screen.x;
        start.y *= screen.y;
        end.x *= screen.x;
        end.y *= screen.y;
        var pos = start;
        rectTransform.position = pos;
        if (mode != SwitchMode.EXIT && mode != SwitchMode.RETURN_EXIT) page.setVisible(true);
        returnButton.interactable = false;

        if (mode == SwitchMode.ENTER)
        {
            page.OnEnter.Invoke();
        }
        else
        {
            page.OnComeBack.Invoke();
        }
        page.switchMode = mode;
        
        var ani = DOTween.To(() => pos, x => pos = x, end, 1.0f);
        ani.OnUpdate(() =>
        {
            rectTransform.position = pos;
            var progress = Math.Abs(pos.x - start.x) / Math.Abs(end.x - start.x);
            page.OnSwitching.Invoke(progress);
        });
        ani.OnComplete(() =>
        {
            page.setVisible(mode == SwitchMode.EXIT ? false : true);
            page.OnSwitching.Invoke(1.0f);
            page.switchMode = SwitchMode.NULL;
            returnButton.interactable = true;
            if (mode == SwitchMode.RETURN_EXIT || mode == SwitchMode.EXIT)
            {
                page.OnExit.Invoke();
                page.setVisible(false);
            }
        });


    }
    public static void switchToPage(UIPages targetPage)
    {
        if (currentPage == targetPage) return;
        if (instance._pageRecord.Count > 0)
        {
            var _curPage = _getPageInsByEnum(currentPage);
            _switchPage(_curPage, SwitchMode.EXIT);
        }
        var pageIns = _getPageInsByEnum(targetPage);
        instance._pageRecord.Push(targetPage);
        _switchPage(pageIns, SwitchMode.ENTER);
    }
    public static void backToPrevPage()
    {
        if (instance._pageRecord.Count <=1) return;
        var _curPage = _getPageInsByEnum(currentPage);
        _switchPage(_curPage, SwitchMode.RETURN_EXIT);
        
        instance._pageRecord.Pop();
        
        var _prevPage = _getPageInsByEnum(currentPage);
        _switchPage(_prevPage, SwitchMode.RETURN);
    }
}
