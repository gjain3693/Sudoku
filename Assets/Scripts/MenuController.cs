using UnityEngine;

public class MenuController : MonoBehaviour
{
    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMenu();
        }
        else
        {
            Debug.LogWarning("AudioManager instance not found");
        }
    }
}