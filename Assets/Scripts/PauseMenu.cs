using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Text time_text;

    public void DisplayTime()
    {
        if (time_text == null)
        {
            Debug.LogWarning("PauseMenu: time_text not assigned");
            return;
        }

        if (Clock.instance != null)
        {
            var clockText = Clock.instance.GetCurrentTimeText();
            if (clockText != null)
            {
                time_text.text = clockText.text;
            }
        }
    }
}