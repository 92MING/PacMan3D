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

    protected override void Awake()
    {
        base.Awake();
        UIManager.characterPage = this;
    }
    private void Start()
    {
        prevCharacterButton.onClick.AddListener(switchPrevCharacter);
        nextCharacterButton.onClick.AddListener(switchNextCharacter);
        prevCharacterButton.gameObject.SetActive(false);
        tryButton.onClick.AddListener(tryCharacter);
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
            string _name;
            SystemManager.TryGetTranslation(firstCharacterType.Name, out _name);
            nameText.text = _name;
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
    public void resetShowCharPos()
    {
        if (showingChar is null) return;
        showingChar.transform.position = CharacterMiddlePos;
        _showingCharObj.transform.LookAt(GameManager.gameCamera.transform);
    }
    public void switchNextCharacter()
    {
        var currentIndex = showingCharIndex;
        if (currentIndex == CharacterBase.AllCharacterType.Count - 1) return;

        prevCharacterButton.gameObject.SetActive(true);
        if (currentIndex == CharacterBase.AllCharacterType.Count - 2)
        {
            nextCharacterButton.gameObject.SetActive(false);
        }

        Destroy(showingCharObj);
        _showingCharObj = Instantiate(ResourcesManager.GetPrefab(CharacterBase.AllCharacterType.ElementAt(currentIndex + 1).Name));
        _showingCharObj.transform.position = CharacterMiddlePos;
        _showingCharObj.transform.LookAt(GameManager.gameCamera.transform);
        var characterComponent = _showingCharObj.GetComponent<CharacterBase>();
        hpText.text = characterComponent.maxHP.ToString();
        atkText.text = characterComponent.atk.ToString();
        defText.text = characterComponent.def.ToString();
        spdText.text = characterComponent.spd.ToString();
        string _name;
        SystemManager.TryGetTranslation(characterComponent.GetType().Name, out _name);
        nameText.text = _name;
    }
    public void switchPrevCharacter()
    {
        var currentIndex = showingCharIndex;
        if (currentIndex == 0) return;

        nextCharacterButton.gameObject.SetActive(true);
        if (currentIndex == 1)
        {
            prevCharacterButton.gameObject.SetActive(false);
        }

        Destroy(showingCharObj);
        _showingCharObj = Instantiate(ResourcesManager.GetPrefab(CharacterBase.AllCharacterType.ElementAt(currentIndex - 1).Name));
        _showingCharObj.transform.position = CharacterMiddlePos;
        _showingCharObj.transform.LookAt(GameManager.gameCamera.transform);
        var characterComponent = _showingCharObj.GetComponent<CharacterBase>();
        hpText.text = characterComponent.maxHP.ToString();
        atkText.text = characterComponent.atk.ToString();
        defText.text = characterComponent.def.ToString();
        spdText.text = characterComponent.spd.ToString();
        string _name;
        SystemManager.TryGetTranslation(characterComponent.GetType().Name, out _name);
        nameText.text = _name;
    }
    
    public void tryCharacter()
    {
        if (showingChar is null) return;
        GameManager.TryCharacter(showingCharObj);
    }
}
