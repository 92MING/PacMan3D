using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

public class TranslateEnumDropdown<Enum> : Dropdown where Enum : System.Enum
{
    readonly System.Array _values = System.Enum.GetValues(typeof(Enum));
    readonly string[] _keys = System.Enum.GetNames(typeof(Enum));
    
    public Enum currentChoice => (Enum)_values.GetValue(value);
    public string currentChoiceKey => _keys[value];
    public string currentChoiceText => options[value].text;

    protected override void Awake()
    {
        base.Awake();
        SystemManager.OnLanguageChanged.AddListener((lang) => { updateOptionTexts(); });
        options.Clear();
        for (int i = 0; i < _values.Length; i++)
        {
            var opt = new Dropdown.OptionData();
            SystemManager.TryGetTranslation(_keys[i], out var translatedText);
            opt.text = translatedText;
            options.Add(opt);
        }
    }

    void updateOptionTexts()
    {
        for (int i=0; i<_values.Length; i++)
        {
            SystemManager.TryGetTranslation(_keys[i], out var translatedText);
            options[i].text = translatedText;
        }
    }
}
