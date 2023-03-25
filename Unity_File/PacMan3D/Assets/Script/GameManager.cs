using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 管理遊戲進行狀態，遊戲加載等
/// </summary>
public class GameManager: Manager<GameManager>
{
    private bool _isPlaying = false;
    [SerializeField] private Color32 _cameraBackgroundColor = new Color32(143, 187, 255, 255);
    public static bool IsPlaying => instance._isPlaying;
    public static Camera gameCamera;

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

    public static void Play(GameMap map, Character character)
    {
        if (IsPlaying)
        {
            Debug.LogWarning("Game is already playing");
            return;
        }
    }
}
