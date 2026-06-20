using UnityEngine;

public class GameSettings : MonoBehaviour
{
public enum EGameMode
    {
        NOT_SET,
        NOVICE,
        INTERMIDIATE,
        ADVANCE,
        EXPERT
    }

    public static GameSettings Instance;

    private void Awake()
    {
        paused = false;
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private EGameMode _gameMode;
    private bool continuePreviousGame = false;
    private bool exitAfterWon = false;
    private bool paused = false;

    public void SetExitAfterWon(bool set)
    {
        exitAfterWon = set;
        continuePreviousGame = false;
    }

    public bool GetExitAfterWon()
    {
        return exitAfterWon;
    }

    public void SetContinuePreviousGame(bool continueGame)
    {
        continuePreviousGame = continueGame;
    }

    public bool GetContinuePreviousGame()
    {
        return continuePreviousGame;
    }

    public void SetPaused(bool _paused)
    {
        paused = _paused;
    }

    public bool GetPaused()
    {
        return paused;
    }

    private void Start()
    {
        _gameMode = EGameMode.NOT_SET;
        continuePreviousGame = false;
    }

    public void SetGameMode(EGameMode mode)
    {
        _gameMode = mode;
    }

    public void SetGameMode(string mode)
    {
        if (mode.Equals("Novice"))
        {
            SetGameMode(EGameMode.NOVICE);
        }
        else if(mode.Equals("Intermidiate"))
        {
            SetGameMode(EGameMode.INTERMIDIATE);
        }

        else if(mode.Equals("Advance"))
        {
            SetGameMode(EGameMode.ADVANCE);
        }
        else if(mode.Equals("Expert"))
        {
            SetGameMode(EGameMode.EXPERT);
        }
        else
        {
            SetGameMode(EGameMode.NOT_SET);
        }

    }

    public string GetGameMode()
    {
        switch(_gameMode)
        {
            case EGameMode.NOVICE:return "Novice";
            case EGameMode.INTERMIDIATE: return "Intermidiate";
            case EGameMode.ADVANCE: return "Advance";
            case EGameMode.EXPERT: return "Expert";
        }
        Debug.LogError("ERROR: Mode not Set");
        return "";
    }
}
