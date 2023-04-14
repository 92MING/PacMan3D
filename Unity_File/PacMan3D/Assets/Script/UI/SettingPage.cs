using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageDropDown : TranslateEnumDropdown<Language> { }

public class SettingPage : UIPage
{
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;

    private LanguageDropDown _languageDropDown;
    [SerializeField] private RectTransform _langDropdown_template;
    [SerializeField] private Text _langDropdown_captionText;
    [SerializeField] private Text _langDropdown_itemText;
    [SerializeField] private GameObject _langDropdown_object;

    protected override void Awake()
    {
        base.Awake();
        UIManager.settingPage = this;
        if (_langDropdown_object.TryGetComponent<Dropdown>(out var dropdown))
        {
            DestroyImmediate(dropdown);
        }
        _languageDropDown = _langDropdown_object.AddComponent<LanguageDropDown>();
        _languageDropDown.template = _langDropdown_template;
        _languageDropDown.captionText = _langDropdown_captionText;
        _languageDropDown.itemText = _langDropdown_itemText;
    }
}
