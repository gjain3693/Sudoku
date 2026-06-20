using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void LoadScene(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            SceneManager.LoadScene(name);
        }
        else
        {
            Debug.LogWarning("LoadScene: Scene name is empty");
        }
    }

    public void LoadEasyGame(string name)
    {
        if (GameSettings.Instance != null)
        {
            GameSettings.Instance.SetGameMode(GameSettings.EGameMode.NOVICE);
        }
        SceneManager.LoadScene(name);
    }

    public void LoadIntermidiateGame(string name)
    {
        if (GameSettings.Instance != null)
        {
            GameSettings.Instance.SetGameMode(GameSettings.EGameMode.INTERMIDIATE);
        }
        SceneManager.LoadScene(name);
    }

    public void LoadAdvanceGame(string name)
    {
        if (GameSettings.Instance != null)
        {
            GameSettings.Instance.SetGameMode(GameSettings.EGameMode.ADVANCE);
        }
        SceneManager.LoadScene(name);
    }

    public void LoadExpertGame(string name)
    {
        if (GameSettings.Instance != null)
        {
            GameSettings.Instance.SetGameMode(GameSettings.EGameMode.EXPERT);
        }
        SceneManager.LoadScene(name);
    }

    public void ActivateGameObject(GameObject gameObject)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(true);
        }
    }

    public void DeactivateGameObject(GameObject gameObject)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }

    public void setPause(bool Paused)
    {
        if (GameSettings.Instance != null)
        {
            GameSettings.Instance.SetPaused(Paused);
        }
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ContinuePreviousGame(bool continueGame)
    {
        if (GameSettings.Instance != null)
        {
            GameSettings.Instance.SetContinuePreviousGame(continueGame);
        }
    }

    public void ExitAfterWon()
    {
        if (GameSettings.Instance != null)
        {
            GameSettings.Instance.SetExitAfterWon(true);
        }
    }

    public void ContinueAfterGameOver()
    {
        if (AdManager.Instance != null)
        {
            AdManager.Instance.ShowInterstitialAd();
        }
        else
        {
            Debug.LogWarning("AdManager not available");
        }

        if (Lives.instance != null)
        {
            Lives.instance.ResetLives();
        }
        else
        {
            Debug.LogWarning("Lives instance not available");
        }
    }
}