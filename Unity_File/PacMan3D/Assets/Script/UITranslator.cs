using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UITranslator : MonoBehaviour
{
    private Text _text;
    [SerializeField] private string _key;
    private void Awake()
    {
        _text = GetComponent<Text>();
        UIManager.OnLanguageChanged.AddListener(OnLanguageChanged);
    }

    private void OnLanguageChanged(string lang)
    {
        //TODO
    }
}
