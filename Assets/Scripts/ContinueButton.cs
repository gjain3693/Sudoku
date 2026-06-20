using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Button))] // ✅ Safety: ensures button exists
public class ContinueButton : MonoBehaviour
{
    public Text timeText;
    public Text levelText;

    string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }

    void Start()
    {
        Debug.Log("START:::" + Config.GameDataExist());

        Button btn = GetComponent<Button>(); // ✅ cache reference

        if (!Config.GameDataExist())
        {
            if (btn != null)
                btn.interactable = false;

            if (timeText != null)
                timeText.text = " ";

            if (levelText != null)
                levelText.text = " ";
        }
        else
        {
            float deltaTime = Config.ReadGameTime();

            // ✅ Safety: prevent negative/invalid time crash
            if (deltaTime < 0)
                deltaTime = 0;

            deltaTime += Time.deltaTime;

            TimeSpan span = TimeSpan.FromSeconds(deltaTime);

            string hour = LeadingZero(span.Hours);
            string minute = LeadingZero(span.Minutes);
            string seconds = LeadingZero(span.Seconds);

            if (timeText != null)
                timeText.text = hour + ":" + minute + ":" + seconds;

            if (levelText != null)
                levelText.text = Config.ReadBoardLevel();
        }
    }

    public void SetGameData()
    {
        string level = Config.ReadBoardLevel();
        Debug.Log("LONG::::::" + level);

        // ✅ Safety: prevent null/empty crash
        if (!string.IsNullOrEmpty(level))
        {
            GameSettings.Instance.SetGameMode(level);
        }
        else
        {
            Debug.LogWarning("Level data missing");
        }
    }
}