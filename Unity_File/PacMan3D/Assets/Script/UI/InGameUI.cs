using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] InGameTopBar topBar;
    [SerializeField] InGamePausePanel pausePanel;

    private void Update()
    {
        if (GameManager.isPlaying && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameManager.isPaused)
            {
                GameManager.Pause();
                pausePanel.gameObject.SetActive(true);
            }
            else
            {
                GameManager.Continue();
                pausePanel.gameObject.SetActive(false);
            }
        }
    }

    public void enterGameMode(CharacterBase[] characters, bool setTimeRemain=true, float timeRemain=90)
    {
        topBar.clearTopBar();
        foreach(var c in characters)
        {
            topBar.AddCharInfoBox(c);
        }
        if (setTimeRemain)
        {
            topBar.enableTimeCountText();
            topBar.SetTimeRemain(90);
            topBar.UpdateTimeText();
        }
        else
        {
            topBar.disableTimeCountText();
        }
    }
    
}
