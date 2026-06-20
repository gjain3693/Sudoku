using UnityEngine;
using UnityEngine.UI;
using System;

public class Clock : MonoBehaviour
{
    private int hour = 0;
    private int min = 0;
    private int sec = 0;
    private Text textClock;
    private float deltaTime;
    private bool stopClock = false;
    public static Clock instance;

    private void Awake()
    {
        // ✅ SAFE: correct singleton destroy (no logic change)
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // ✅ SAFE: prevent null crash
        textClock = GetComponent<Text>();
        if (textClock == null)
        {
            Debug.LogWarning("Clock: Text component missing");
        }

        // ✅ SAFE: null check for GameSettings
        if (GameSettings.Instance != null)
        {
            Debug.Log("LIVES:::::" + GameSettings.Instance.GetContinuePreviousGame());

            if (GameSettings.Instance.GetContinuePreviousGame())
            {
                deltaTime = Config.ReadGameTime();

                // ✅ SAFE: prevent invalid time
                if (deltaTime < 0)
                    deltaTime = 0;

                Debug.Log("Game time:::" + deltaTime);
            }
            else
            {
                deltaTime = 0;
            }
        }
        else
        {
            // fallback safety
            deltaTime = 0;
            Debug.LogWarning("GameSettings.Instance is null");
        }
    }

    private void Start()
    {
        stopClock = false;
    }

    private void Update()
    {
        // ✅ SAFE: null check
        if (GameSettings.Instance == null) return;

        if (GameSettings.Instance.GetPaused() == false && stopClock == false)
        {
            deltaTime += Time.deltaTime;

            TimeSpan span = TimeSpan.FromSeconds(deltaTime);

            string _hour = LeadingZero(span.Hours);
            string _minutes = LeadingZero(span.Minutes);
            string _seconds = LeadingZero(span.Seconds);

            // ✅ SAFE: prevent null crash
            if (textClock != null)
                textClock.text = _hour + ":" + _minutes + ":" + _seconds;
        }
    }

    private string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }

    public void OnGameOver()
    {
        stopClock = true;
    }

    private void OnEnable()
    {
        GameEvents.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= OnGameOver;
    }

    public static string GetCurrentTime()
    {
        // ✅ SAFE: prevent null crash
        if (instance == null)
            return "0";

        return instance.deltaTime.ToString();
    }

    public Text GetCurrentTimeText()
    {
        return textClock;
    }

    public void StartClock()
    {
        stopClock = false;
    }
}