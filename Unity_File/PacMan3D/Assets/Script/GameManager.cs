using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 管理遊戲進行狀態，遊戲加載等
/// </summary>
public class GameManager: Manager<GameManager>
{
    private static bool _isPlaying = false;
    public static bool isPlaying => _isPlaying; //是否在遊戲中（停止或進行中均為true）
    private static bool _isPaused = false;
    public static bool isPaused => _isPaused; //是否暫停

    public static Camera gameCamera; // 遊戲目前主相機
    private static CharacterBase _playerCharacter;
    public static CharacterBase playerCharacter => _playerCharacter; // 玩家控制中的角色
    private static GameObject _focusingObj;
    public static GameObject focusingObj => _focusingObj; // 目前鏡頭對焦的物件
    private Color32 _cameraBackgroundColor = new Color32(143, 187, 255, 255);
    public static readonly Vector3 cameraDefaultPos = new Vector3(0, 1, -10);

    public static readonly float defaultFOV = 60.0f;
    public static readonly float gamingFOV = 85.0f;
    public static readonly float defaultDepressionAngle = 5.0f; //相机默认俯角
    private static float _camVerticalRotationRatio = 0.0f; //相机垂直视角比例, [-0.9,0.9]
    public static float camVerticalRotationRatio
    {
        get
        {
            return _camVerticalRotationRatio;
        }
        set
        {
            if (value < -0.9f) _camVerticalRotationRatio = -0.9f;
            else if (value > 0.9f) _camVerticalRotationRatio = 0.9f;
            else _camVerticalRotationRatio = value;
        }
    }

    private static Vector3 _camPos_Horizontal => (focusingObj!=null)? focusingObj.transform.position - focusingObj.transform.forward * 1.5f + focusingObj.transform.up * 1.25f : Vector3.forward;
    private static Vector3 _camPos_LookAtSky => (focusingObj != null) ? focusingObj.transform.position + focusingObj.transform.up: Vector3.up;
    private static Vector3 _camPos_LookAtGround => (focusingObj != null) ? focusingObj.transform.position + focusingObj.transform.up * 1.8f : Vector3.up*1.8f;

    private static Vector3 _camForward_Horizontal => (focusingObj != null) ? focusingObj.transform.forward - focusingObj.transform.up * Mathf.Atan(defaultDepressionAngle * Mathf.Deg2Rad): Vector3.forward;
    private static Vector3 _camForward_LookAtSky => (focusingObj != null) ? focusingObj.transform.up : Vector3.up;
    private static Vector3 _camForward_LookAtGround => (focusingObj != null) ? -focusingObj.transform.up : Vector3.down;

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
        gameCamera.transform.position = cameraDefaultPos;
        gameObject.AddComponent<StandaloneInputModule>();
    }
    private void FixedUpdate()
    {
        if (isPlaying)
        {
            if (focusingObj != null)
            {
                if (camVerticalRotationRatio >= 0) //向上看
                {
                    gameCamera.transform.position = Vector3.Lerp(_camPos_Horizontal, _camPos_LookAtSky, camVerticalRotationRatio);
                    gameCamera.transform.forward = Vector3.Lerp(_camForward_Horizontal, _camForward_LookAtSky, camVerticalRotationRatio);
                }
                else //向下看
                {
                    gameCamera.transform.position = Vector3.Lerp(_camPos_Horizontal, _camPos_LookAtGround, Mathf.Abs(camVerticalRotationRatio));
                    gameCamera.transform.forward = Vector3.Lerp(_camForward_Horizontal, _camForward_LookAtGround, Mathf.Abs(camVerticalRotationRatio));
                }
                
            }
        }
    }
    
    /// <summary>
    /// 开始游戏
    /// </summary>
    /// <param name="map"></param>
    /// <param name="playerCharacter"></param>
    public static void Play(GameMap map, CharacterBase playerCharacter)
    {
        if (isPlaying)
        {
            Debug.LogWarning("Game is already playing, can't play again");
            return;
        }
        // TODO
    }

    /// <summary>
    /// 暂停游戏（如果在游戏中）
    /// </summary>
    public static void Pause()
    {
        if (!isPlaying)
        {
            Debug.LogWarning("Game is not playing, can't pause");
            return;
        }
        // TODO
    }

    /// <summary>
    /// 试用角色
    /// </summary>
    /// <param name="charObj"></param>
    public static void TryCharacter(GameObject charObj)
    {
        if (charObj is null)
        {
            Debug.LogWarning("Character Gameobject is null! Cant try character!");
            return;
        }
        if (!charObj.TryGetComponent<CharacterBase>(out var character))
        {
            Debug.LogWarning("No character on the gameobject! Cant try character!");
            return;
        }

        EnterGameMode(character, MapManager.testMap); 
    }
    public static void TryCharacter<CharCls>() where CharCls: CharacterBase
    {
        var charObj = Instantiate(ResourcesManager.GetPrefab(typeof(CharCls).Name));
        TryCharacter(charObj);
    }

    /// <summary>
    /// 进入游戏模式，凡是涉及进入游戏的，最后都是调用这个方法
    /// </summary>
    /// <param name="character"></param>
    /// <param name="map"></param>
    private static void EnterGameMode(CharacterBase character, GameMap map)
    {
        if (isPlaying)
        {
            Debug.LogWarning("Game is already playing, can't enter game mode");
            return;
        }
        
        UIManager.enterGameMode(); //UI 切换到游戏模式
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        MapManager.LoadMapToGame(MapManager.testMap); //加载地图

        SetPlayerCharacter(character);
        character.enterGameMode();
        character.placeAtMapCell(MapManager.currentMap.playerRebornPos);
        _isPlaying = true;
    }
    private static void QuitGameMode()
    {
        //TODO
    }

    public static void SetPlayerCharacter(CharacterBase character)
    {
        SetCameraFocusTo(character.gameObject);
        _playerCharacter = character;
    }
    public static void SetCameraFocusTo(GameObject obj, bool setGameFOV=true)
    {
        if (obj == focusingObj) return;
        _focusingObj = obj;
        if (setGameFOV) gameCamera.fieldOfView = gamingFOV;
        else gameCamera.fieldOfView = defaultFOV;
    }
    public static void CameraCancelFocus()
    {
        _focusingObj = null;
        gameCamera.transform.position = cameraDefaultPos;
        gameCamera.fieldOfView = defaultFOV;
    }
    
    public static void EnterMapEditMode(GameMap map)
    {
        
    }
}
