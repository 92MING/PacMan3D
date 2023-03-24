using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPage : UIPage
{
    private GameObject _showingCharacter = null;
    private static Vector3 CharacterRightPos = new Vector3(8, 0, -6);
    private static Vector3 CharacterMiddlePos = new Vector3(-2.5f, 0, -6);
    private static Vector3 CharacterLeftPos = new Vector3(-8, 0, -6);

    private void Awake()
    {
        base.Awake();
        UIManager.characterPage = this;
    }
    private void Start()
    {
        OnEnter.AddListener(() =>
        {
            if (_showingCharacter is null)
                _showingCharacter = Instantiate(Character.AllCharacterPrefab[Character.AllCharacterType.First.Value.Name]);
            _showingCharacter.transform.position = CharacterRightPos;
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
}
