using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class InGameCharInfoBox : MonoBehaviour
{
    public static GameObject prefab => ResourcesManager.GetPrefab("InGameCharInfoBox");
    [SerializeField] private Slider hp;
    [SerializeField] private Image icon;
    [SerializeField] private Text nameBox;
    [SerializeField] private Text coin;
    private CharacterBase _character;
    private string _coinText = null;
    private string coinText
    {
        get
        {
            if (_coinText is null)
            {
                SystemManager.TryGetTranslation("Coin", out _coinText);
            }
            return _coinText;
        }
    }

    private void Awake()
    {
        SystemManager.OnLanguageChanged.AddListener((lang) => { SystemManager.TryGetTranslation("Coin", out var coinText); });
    }
    
    public void OnCoinChanged(int newCoin)
    {
        if (_character != null) coin.text = coinText + ":" + newCoin.ToString();
    }
    public void OnHpChanged(int newHP)
    {
        if (_character != null) hp.value = newHP / (float)_character.maxHP; 
    }
    private void BindListenerToChar()
    {
        if (_character != null)
        {
            _character.OnCoinChanged.AddListener(OnCoinChanged);
            _character.OnHpChanged.AddListener(OnHpChanged);
        }
    }
    private void UnbindListenerToChar()
    {
        if (_character != null)
        {
            _character.OnCoinChanged.RemoveListener(OnCoinChanged);
            _character.OnHpChanged.RemoveListener(OnHpChanged);
        }
    }

    public void BindCharacter(CharacterBase character)
    {
        if (character is null) return;
        _character = character;
        nameBox.text = character.name;
        icon.sprite = character.icon;
        coin.text = coinText + ":" + character.coins.ToString();
        hp.value = character.hp / (float)character.maxHP;
        BindListenerToChar();
    }
    public void UnbindCharacter()
    {
        if (_character != null)
        {
            UnbindListenerToChar();
            _character = null;
        }
    }

    private void OnEnable()
    {
        BindListenerToChar();
    }
    private void OnDisable()
    {
        UnbindListenerToChar();
    }
    private void OnDestroy()
    {
        UnbindListenerToChar();
    }

}