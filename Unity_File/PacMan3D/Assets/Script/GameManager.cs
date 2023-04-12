using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 管理遊戲進行狀態，遊戲加載等
/// </summary>
public class GameManager: Manager<GameManager>
{
    [SerializeField] private Color32 _cameraBackgroundColor = new Color32(143, 187, 255, 255);
    
    private static bool _isPlaying = false;
    public static bool IsPlaying => _isPlaying; //是否在遊戲中（停止或進行中均為true）
    private static bool _isPaused = false;
    public static bool IsPaused => _isPaused; //是否暫停

    public static Camera gameCamera; // 遊戲目前主相機
    public static CharacterBase playerCharacter; // 玩家控制中的角色

    private void Awake()
    {
        base.Awake();
        if (Camera.main == null)
        {
            var obj = new GameObject("GameCamera");
            gameCamera = obj.AddComponent<Camera>();
        }
        else
        {
            Camera.main.gameObject.name = "GameCamera";
            gameCamera = Camera.main;
        }
        gameCamera.clearFlags = CameraClearFlags.SolidColor;
        gameCamera.backgroundColor = _cameraBackgroundColor;
        gameObject.AddComponent<StandaloneInputModule>();
    }

    public static void Play(GameMap map, CharacterBase playerCharacter)
    {
        if (IsPlaying)
        {
            Debug.LogWarning("Game is already playing, can't play again");
            return;
        }
        // TODO
    }

    public static void Pause()
    {
        if (!IsPlaying)
        {
            Debug.LogWarning("Game is not playing, can't pause");
            return;
        }
        // TODO
    }

    public static void TryCharacter(GameObject charObj)
    {
        if (IsPlaying)
        {
            Debug.LogWarning("Game is already playing, can't do this");
            return;
        }
        MapManager.LoadMapToGame(MapManager.testMap);
    }

    public static void EnterMapEditMode(GameMap map)
    {
        
    }
}
