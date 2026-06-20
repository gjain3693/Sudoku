using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour
{
    public List<GameObject> error_images;
    public int lives = 0;
    public int error_number = 0;
    public GameObject gameOverPopup;

    public static Lives instance;

    private void Awake()
    {
        // ✅ Proper singleton (don’t destroy wrong object)
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Start()
    {
        if (error_images == null || error_images.Count == 0)
        {
            Debug.LogWarning("Error images not assigned");
            return;
        }

        lives = error_images.Count;
        error_number = 0;

        if (GameSettings.Instance != null && GameSettings.Instance.GetContinuePreviousGame())
        {
            error_number = Config.ErrorNumber();
            lives = error_images.Count - error_number;

            for (int error = 0; error < error_number && error < error_images.Count; error++)
            {
                if (error_images[error] != null)
                    error_images[error].SetActive(true);
            }
        }
    }

    private void WrongNumber()
    {
        if (error_images == null) return;

        if (error_number < error_images.Count)
        {
            if (error_images[error_number] != null)
                error_images[error_number].SetActive(true);

            error_number++;
            lives--;
        }

        CheckForGameOver();
    }

    private void CheckForGameOver()
    {
        if (lives <= 0)
        {
            GameEvents.OnGameOverMethod();

            if (gameOverPopup != null)
                gameOverPopup.SetActive(true);
        }
    }

    private void OnEnable()
    {
        GameEvents.OnWrongNumber += WrongNumber;
    }

    private void OnDisable()
    {
        GameEvents.OnWrongNumber -= WrongNumber;
    }

    public int GetErrorNumber()
    {
        return error_number;
    }

    public void ResetLives()
    {
        if (error_images == null) return;

        foreach (var error in error_images)
        {
            if (error != null)
                error.SetActive(false);
        }

        error_number = 0;
        lives = error_images.Count;
    }
}