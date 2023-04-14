using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGamePausePanel : MonoBehaviour
{
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        exitButton.onClick.AddListener(() => GameManager.Exit());
    }
}
