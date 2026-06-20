using UnityEngine;

public class PrivacyPolicy : MonoBehaviour
{
    public enum ActionType
    {
        OpenPrivacyPolicy,
        AcceptPrivacy,
        ClosePopup
    }

    public ActionType actionType;

    public GameObject popupToClose; // assign if needed

    private string policyURL = "https://github.com/gjain3693/PrivacyPolicy-Sudoku";

    public void Execute()
    {
        switch (actionType)
        {
            case ActionType.OpenPrivacyPolicy:
                Application.OpenURL(policyURL);
                break;

            case ActionType.AcceptPrivacy:
                PlayerPrefs.SetInt("PrivacyAccepted", 1);
                PlayerPrefs.Save();

                if (popupToClose != null)
                    popupToClose.SetActive(false);

                Time.timeScale = 1f;
                break;

            case ActionType.ClosePopup:
                if (popupToClose != null)
                    popupToClose.SetActive(false);
                break;
        }
    }
}
