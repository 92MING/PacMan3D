using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理遊戲進行狀態，遊戲加載等
/// </summary>
public class GameManager: Manager<GameManager>
{
    [SerializeField] private bool _isPlaying = false;
    public static bool IsPlaying => instance._isPlaying;
    public static void Play(GameMap map)
    {
        if (IsPlaying)
        {
            Debug.LogWarning("Game is already playing");
            return;
        }
    }
}
