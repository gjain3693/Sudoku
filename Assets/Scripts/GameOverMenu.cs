using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public Text textClock;

    private void Start()
    {
        if (textClock == null)
        {
            Debug.LogWarning("textClock is not assigned");
            return;
        }

        if (Clock.instance != null && Clock.instance.GetCurrentTimeText() != null)
        {
            textClock.text = Clock.instance.GetCurrentTimeText().text;
        }
        else
        {
            Debug.LogWarning("Clock instance or text is missing");
            textClock.text = "00:00:00"; // safe fallback
        }
    }
}