using UnityEngine;
using UnityEngine.UI;

public class InGameTopBar : MonoBehaviour
{
    [SerializeField] public GameObject contentObj;
    [SerializeField] public Text timeRemainText;
    [SerializeField] public Text timeRemainCount;
    
    private float _timeRemain = 0.0f;
    public float timeRemain
    {
        get
        {
            return _timeRemain;
        }
        set
        {
            _timeRemain = value;
            if (_timeRemain < 0) _timeRemain = 0;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.isPlaying && !GameManager.isPaused)
        {
            timeRemain -= Time.fixedDeltaTime;
            UpdateTimeText();
        }
    }

    public void clearTopBar()
    {
        for (int i = 0; i < contentObj.transform.childCount; i++)
        {
            if (contentObj.transform.GetChild(i).TryGetComponent<InGameCharInfoBox>(out var box))
            {
                Destroy(contentObj.transform.GetChild(i).gameObject);
            }
        }
    }
    public void SetTimeRemain(int seconds = 90)
    {
        timeRemain = seconds;
    }
    public void UpdateTimeText()
    {
        timeRemainCount.text = _timeRemain.ToString("F2");
    }
    public void disableTimeCountText()
    {
        timeRemainText.gameObject.SetActive(false);
        timeRemainCount.gameObject.SetActive(false);
    }
    public void enableTimeCountText()
    {
        timeRemainText.gameObject.SetActive(true);
        timeRemainCount.gameObject.SetActive(true);
    }

    public void AddCharInfoBox(CharacterBase character)
    {
        var obj = Instantiate(InGameCharInfoBox.prefab, contentObj.transform);
        var box = obj.GetComponent<InGameCharInfoBox>();
        box.BindCharacter(character);
    }

}
