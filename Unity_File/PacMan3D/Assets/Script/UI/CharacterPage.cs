using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CharacterPage : UIPage
{
    private GameObject _showingCharObj = null;
    public GameObject showingCharObj => _showingCharObj;
    public CharacterBase showingChar => _showingCharObj.GetComponent<CharacterBase>();
    public System.Type showingCharType => showingChar.GetType();
    public int showingCharIndex => CharacterBase.AllCharacterType.Select((type, index) => new { type, index }).FirstOrDefault(pair => pair.type == showingCharType).index;

    private static Vector3 CharacterRightPos = new Vector3(8, 0, -6); //画面以外，右边的位置
    private static Vector3 CharacterMiddlePos = new Vector3(-2.5f, 0, -6); //画面中间（显示中）的位置
    private static Vector3 CharacterLeftPos = new Vector3(-8, 0, -6); //画面以外，左边的位置

    [SerializeField] private Text hpText;
    [SerializeField] private Text atkText;
    [SerializeField] private Text defText;
    [SerializeField] private Text spdText;
    [SerializeField] private Text nameText;

    [SerializeField] private Button prevCharacterButton;
    [SerializeField] private Button nextCharacterButton;
    [SerializeField] private Button tryButton;

    private void Awake()
    {
        base.Awake();
        UIManager.characterPage = this;
        prevCharacterButton.onClick.AddListener(switchNextCharacter);
        nextCharacterButton.onClick.AddListener(switchPrevCharacter);
        tryButton.onClick.AddListener(tryCharacter);
    }
    private void Start()
    {
        OnEnter.AddListener(() =>
        {
            var firstCharacterType = CharacterBase.AllCharacterType.First.Value;
            if (_showingCharObj is null) {
                _showingCharObj = Instantiate(ResourcesManager.GetPrefab(firstCharacterType.Name));
            }
            _showingCharObj.transform.position = CharacterRightPos;
            var characterComponent = _showingCharObj.GetComponent<CharacterBase>();
            characterComponent.exitGameMode();
            hpText.text = characterComponent.maxHP.ToString();
            atkText.text = characterComponent.atk.ToString();
            defText.text = characterComponent.def.ToString();
            spdText.text = characterComponent.spd.ToString();
            nameText.text = firstCharacterType.Name;
        });
        OnSwitching.AddListener((progress) =>
        {
            Vector3 startPos = switchMode == UIManager.SwitchMode.ENTER ? CharacterRightPos : switchMode == UIManager.SwitchMode.RETURN ? CharacterLeftPos : CharacterMiddlePos;
            Vector3 endPos = switchMode == UIManager.SwitchMode.EXIT ? CharacterLeftPos : switchMode == UIManager.SwitchMode.RETURN_EXIT ? CharacterRightPos : CharacterMiddlePos;
            _showingCharObj.transform.position = Vector3.Lerp(startPos, endPos, progress);
            _showingCharObj.transform.LookAt(GameManager.gameCamera.transform);
        });
        OnExit.AddListener(() =>
        {
            Destroy(_showingCharObj);
            _showingCharObj = null;
        });
    }

    public void switchNextCharacter()
    {
        var currentIndex = showingCharIndex;
        if (currentIndex == CharacterBase.AllCharacterType.Count - 1) return;
        Destroy(showingCharObj);
        _showingCharObj = Instantiate(ResourcesManager.GetPrefab(CharacterBase.AllCharacterType.ElementAt(currentIndex + 1).Name));
        _showingCharObj.transform.position = CharacterMiddlePos;
    }
    public void switchPrevCharacter()
    {
        var currentIndex = showingCharIndex;
        if (currentIndex == 0) return;
        Destroy(showingCharObj);
        _showingCharObj = Instantiate(ResourcesManager.GetPrefab(CharacterBase.AllCharacterType.ElementAt(currentIndex - 1).Name));
        _showingCharObj.transform.position = CharacterMiddlePos;
    }
    
    public void tryCharacter()
    {
        if (showingChar is null) return;
        GameManager.TryCharacter(showingCharObj);
    }
}
