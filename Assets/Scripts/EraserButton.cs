using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))] // ✅ ensures button exists
public class EraserButton : Selectable, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // ✅ Safe invoke (in case GameEvents is not initialized yet)
        if (Application.isPlaying)
        {
            GameEvents.OnClearNumberMethod();
        }
    }
}