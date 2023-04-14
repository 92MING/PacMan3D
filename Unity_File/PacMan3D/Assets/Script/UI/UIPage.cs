using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class UIPage : MonoBehaviour
{
    [HideInInspector] public UIManager.SwitchMode switchMode = UIManager.SwitchMode.NULL; //目前页面的切换模式。切换完毕后变回NULL

    public UnityEvent OnEnter = new UnityEvent();
    public UnityEvent OnExit = new UnityEvent();
    public UnityEvent OnComeBack = new UnityEvent();
    public UnityEvent<float> OnSwitching = new UnityEvent<float>();

    private CanvasGroup _canvasGroup;
    protected virtual void Awake()
    {
        if (!TryGetComponent(out _canvasGroup))
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void setVisible(bool set)
    {
        if (set)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;
        }
        else
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
        }
    }
}
