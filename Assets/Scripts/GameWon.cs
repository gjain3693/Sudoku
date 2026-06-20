using UnityEngine;
using UnityEngine.UI;

public class GameWon : MonoBehaviour
{
    public GameObject WinPopup;
    public Text ClockText;

    public void Start()
    {
        if (WinPopup != null)
            WinPopup.SetActive(false);

        if (ClockText != null && Clock.instance != null)
        {
            var timeText = Clock.instance.GetCurrentTimeText();
            if (timeText != null)
                ClockText.text = timeText.text;
        }
    }

    private void OnBoardCompleted()
    {
        if (WinPopup != null)
            WinPopup.SetActive(true);

        if (ClockText != null && Clock.instance != null)
        {
            var timeText = Clock.instance.GetCurrentTimeText();
            if (timeText != null)
                ClockText.text = timeText.text;
        }
    }

    private void OnEnable()
    {
        GameEvents.onBoardCompleted += OnBoardCompleted;
    }

    private void OnDisable()
    {
        GameEvents.onBoardCompleted -= OnBoardCompleted;
    }
}