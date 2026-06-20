using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoteButton : Selectable, IPointerClickHandler
{
    public Sprite onImage;
    public Sprite offImage;

    private bool active;
    private Image buttonImage;

    void Start()
    {
        active = false;

        // ✅ Cache Image safely
        buttonImage = GetComponent<Image>();

        if (buttonImage == null)
        {
            Debug.LogWarning("NoteButton: Image component missing");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        active = !active;

        if (buttonImage != null)
        {
            if (active)
            {
                if (onImage != null)
                    buttonImage.sprite = onImage;
            }
            else
            {
                if (offImage != null)
                    buttonImage.sprite = offImage;
            }
        }

        GameEvents.OnNotesActiveMethod(active);
    }
}