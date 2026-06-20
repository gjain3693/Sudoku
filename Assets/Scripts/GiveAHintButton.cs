using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GiveAHintButton : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();

        if (button == null)
        {
            Debug.LogWarning("GiveAHintButton: Button component missing");
            return;
        }

        button.onClick.AddListener(OnButtonClicked);
        button.interactable = true;
    }

    private void OnButtonClicked()
    {
        if (Application.isEditor)
        {
            GameEvents.OnGiveAHintMethod();
        }
        else
        {
            if (AdManager.Instance != null)
            {
                AdManager.Instance.ShowRewardAd();
            }
            else
            {
                Debug.LogWarning("AdManager instance not found");
            }
        }
    }

    private void OnEnable()
    {
        GameEvents.onGiveAHintAdOpening += onGiveAHintAdOpening;
    }

    private void OnDisable()
    {
        GameEvents.onGiveAHintAdOpening -= onGiveAHintAdOpening;

        // ✅ Prevent duplicate listeners if object reused
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }
    }

    private void onGiveAHintAdOpening()
    {
        if (button != null)
        {
            button.interactable = false;
        }
    }
}