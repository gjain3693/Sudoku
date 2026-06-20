using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class GridSquare : Selectable, IPointerClickHandler, IPointerExitHandler, ISubmitHandler, IPointerUpHandler
{
    public GameObject number_text;
    public List<GameObject> numberNotes;

    private bool note_active;
    private int number_ = 0;
    private int correct_number = 0;
    private bool selected = false;
    private int squareIndex = -1;
    private bool hasDefaultValue = false;
    private bool hasWrongValue = false;

    public int GetNumber() => number_;
    public bool IsCorrectNumberSet() => number_ == correct_number;
    public bool HasWrongValue() => hasWrongValue;

    public void SetHasDefaultValue(bool default_value) => hasDefaultValue = default_value;
    public bool GetHasDefaultValue() => hasDefaultValue;
    public bool isSelected() => selected;
    public void setSquareIndex(int index) => squareIndex = index;

    public void SetCorrectNumber(int number)
    {
        correct_number = number;
        hasWrongValue = false;

        if (number_ != 0 && number_ != correct_number)
        {
            hasWrongValue = true;
            SetSquareColor(Color.red);
        }
    }

    public void SetCorrectNumber()
    {
        number_ = correct_number;
        SetNoteNumberValue(0);
        DisplayText();
    }

    void Start()
    {
        selected = false;
        note_active = false;

        if (GameSettings.Instance != null && GameSettings.Instance.GetContinuePreviousGame() == false)
        {
            SetNoteNumberValue(0);
        }
        else
        {
            SetClearEmptyNotes();
        }
    }

    public List<string> GetSquareNotes()
    {
        List<string> notes = new List<string>();

        foreach (var number in numberNotes)
        {
            if (number != null)
            {
                var txt = number.GetComponent<Text>();
                notes.Add(txt != null ? txt.text : " ");
            }
        }

        return notes;
    }

    private void SetClearEmptyNotes()
    {
        foreach (var number in numberNotes)
        {
            if (number == null) continue;

            var txt = number.GetComponent<Text>();
            if (txt != null && txt.text == "0")
                txt.text = " ";
        }
    }

    private void SetNoteNumberValue(int value)
    {
        foreach (var number in numberNotes)
        {
            if (number == null) continue;

            var txt = number.GetComponent<Text>();
            if (txt != null)
                txt.text = value <= 0 ? " " : value.ToString();
        }
    }

    private void SetNoteSingleNumberValue(int value, bool force_update = false)
    {
        if (!note_active && !force_update) return;

        if (value <= 0 || value > numberNotes.Count) return; // ✅ CRITICAL FIX

        var obj = numberNotes[value - 1];
        if (obj == null) return;

        var txt = obj.GetComponent<Text>();
        if (txt == null) return;

        if (txt.text == " " || force_update)
            txt.text = value.ToString();
        else
            txt.text = " ";
    }

    public void SetGridNotes(List<int> notes)
    {
        foreach (var note in notes)
        {
            SetNoteSingleNumberValue(note, true);
        }
    }

    public void OnNoteActive(bool active)
    {
        note_active = active;
    }

    public void DisplayText()
    {
        if (number_text == null) return;

        var txt = number_text.GetComponent<Text>();
        if (txt == null) return;

        txt.text = number_ <= 0 ? " " : number_.ToString();

        if (hasDefaultValue)
            txt.fontStyle = FontStyle.Bold;
    }

    public void SetNumber(int number)
    {
        number_ = number;
        DisplayText();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        selected = true;
        GameEvents.SquareSelectedMethod(squareIndex);
    }

    public void OnSubmit(BaseEventData baseEventData) { }

    private void OnEnable()
    {
        GameEvents.OnUpdateSquareNumber += OnSetNumber;
        GameEvents.OnSqaureSelected += OnSquareSelected;
        GameEvents.onNotesActive += OnNoteActive;
        GameEvents.onClearNumber += onClearNumber;
        GameEvents.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnUpdateSquareNumber -= OnSetNumber;
        GameEvents.OnSqaureSelected -= OnSquareSelected;
        GameEvents.onNotesActive -= OnNoteActive;
        GameEvents.onClearNumber -= onClearNumber;
        GameEvents.OnGameOver -= OnGameOver;
    }

    private void OnGameOver()
    {
        if (number_ != 0 && number_ != correct_number)
        {
            hasWrongValue = false;
            number_ = 0;
            DisplayText();
        }

        SetSquareColor(Color.white);
    }

    public void onClearNumber()
    {
        if (selected && !hasDefaultValue)
        {
            number_ = 0;
            hasWrongValue = false;
            SetSquareColor(Color.white);
            SetNoteNumberValue(0);
            DisplayText();
        }
    }

    public void SetCorrectValueOnHint()
    {
        SetSquareNumber(correct_number);
    }

    public void OnSetNumber(int number)
    {
        if (selected && hasDefaultValue == false)
        {
            SetSquareNumber(number);
        }
    }

    private void SetSquareNumber(int number)
    {
        if (note_active && !hasWrongValue)
        {
            SetNoteSingleNumberValue(number);
        }
        else
        {
            SetNoteNumberValue(0);
            SetNumber(number);

            if (number != correct_number)
            {
                var colors = this.colors;
                colors.normalColor = Color.red;
                this.colors = colors;

                hasWrongValue = true;

                if (AudioManager.Instance != null) // ✅ safety
                    AudioManager.Instance.PlayError();

                GameEvents.OnWrongNumberMethod();
            }
            else
            {
                hasWrongValue = false;
                hasDefaultValue = true;

                var colors = this.colors;
                colors.normalColor = Color.white;
                this.colors = colors;
            }
        }

        GameEvents.CheckBoardCompletedMethod();
    }

    public void OnSquareSelected(int squareIndex_)
    {
        if (squareIndex != squareIndex_)
        {
            selected = false;
        }
    }

    public void SetSquareColor(Color col)
    {
        var colors = this.colors;
        colors.normalColor = col;
        this.colors = colors;
    }
}