using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPage : UIPage
{
    private GameObject _showingCharacter = null;
    private static Vector3 CharacterRightPos = new Vector3(8, 0, -6);
    private static Vector3 CharacterMiddlePos = new Vector3(-2.5f, 0, -6);
    private static Vector3 CharacterLeftPos = new Vector3(-8, 0, -6);

    [SerializeField] private Text hpText;
    [SerializeField] private Text atkText;
    [SerializeField] private Text defText;
    [SerializeField] private Text spdText;
    [SerializeField] private Text nameText;

    [SerializeField] private Button prevCharacterButton;
    [SerializeField] private Button nextCharacterButton;

    private void Awake()
    {
        base.Awake();
        UIManager.characterPage = this;
    }
    private void Start()
    {
        OnEnter.AddListener(() =>
        {
            var firstCharacterType = CharacterBase.AllCharacterType.First.Value;
            if (_showingCharacter is null) {
                _showingCharacter = Instantiate(ResourcesManager.GetPrefab(firstCharacterType.Name));
            }
            _showingCharacter.transform.position = CharacterRightPos;
            var characterComponent = _showingCharacter.GetComponent<CharacterBase>();
            hpText.text = characterComponent.maxHP.ToString();
            atkText.text = characterComponent.maxAttack.ToString();
            defText.text = characterComponent.maxDefence.ToString();
            spdText.text = characterComponent.maxSpeed.ToString();
            nameText.text = firstCharacterType.Name;
        });
        OnSwitching.AddListener((progress) =>
        {
            Vector3 startPos = switchMode == UIManager.SwitchMode.ENTER ? CharacterRightPos : switchMode == UIManager.SwitchMode.RETURN ? CharacterLeftPos : CharacterMiddlePos;
            Vector3 endPos = switchMode == UIManager.SwitchMode.EXIT ? CharacterLeftPos : switchMode == UIManager.SwitchMode.RETURN_EXIT ? CharacterRightPos : CharacterMiddlePos;
            _showingCharacter.transform.position = Vector3.Lerp(startPos, endPos, progress);
            _showingCharacter.transform.LookAt(GameManager.gameCamera.transform);
        });
        OnExit.AddListener(() =>
        {
            Destroy(_showingCharacter);
            _showingCharacter = null;
        });
    }

    public static void switchCharacter()
    {
        //TODO
    }
}
