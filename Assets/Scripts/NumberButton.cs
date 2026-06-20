using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NewMonoBehaviourScript : Selectable,
    IPointerClickHandler,
    ISubmitHandler,
    IPointerUpHandler,
    IPointerExitHandler
{
    public int value = 0;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        // ✅ Safety: prevent invalid values (optional but safe)
        if (value < 0 || value > 9)
        {
            Debug.LogWarning("Invalid value on number button: " + value);
            return;
        }

        GameEvents.UpdateSquareNumberMethod(value);
    }

    public void OnSubmit(BaseEventData baseEventData)
    {
        // (kept empty intentionally)
    }
}