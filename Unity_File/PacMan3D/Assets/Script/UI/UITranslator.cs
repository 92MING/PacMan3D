using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UITranslator : MonoBehaviour
{
    private Text _text;
    [SerializeField] private string _key;
    [SerializeField] private string _defaultText;
    [SerializeField] private string _beforeWord;
    [SerializeField] private string _afterWord;
    private void Awake()
    {
        _text = GetComponent<Text>();
        SystemManager.OnLanguageChanged.AddListener(OnLanguageChanged);
    }
    private void Start()
    {
        OnLanguageChanged(null);
    }

    private void OnLanguageChanged(string lang)
    {
        if (!SystemManager.TryGetTranslation(_key, out string translation)){
            if (_defaultText != null && _defaultText != "")
                translation = _defaultText;
            else
                translation = _key;
        }
        _text.text = _beforeWord + translation + _afterWord;
    }
}
