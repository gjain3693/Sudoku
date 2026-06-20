using UnityEngine;
using static GameEvents;

public class GameEvents : MonoBehaviour
{
    public delegate void UpdateSquareNumber(int number);
    public static event UpdateSquareNumber OnUpdateSquareNumber;

    public delegate void CheckBoardCompleted();
    public static event CheckBoardCompleted onCheckBoardCompleted;

    public static void CheckBoardCompletedMethod()
    {
        onCheckBoardCompleted?.Invoke(); // ✅ safe
    }

    public static void UpdateSquareNumberMethod(int number)
    {
        OnUpdateSquareNumber?.Invoke(number); // ✅ safe
    }

    public delegate void SquareSelected(int sqaure_Index);
    public static event SquareSelected OnSqaureSelected;

    public static void SquareSelectedMethod(int square_index)
    {
        OnSqaureSelected?.Invoke(square_index); // ✅ safe
    }

    public delegate void WrongNumber();
    public static event WrongNumber OnWrongNumber;

    public static void OnWrongNumberMethod()
    {
        OnWrongNumber?.Invoke(); // ✅ safe
    }

    public delegate void GameOver();
    public static event GameOver OnGameOver;

    public static void OnGameOverMethod()
    {
        OnGameOver?.Invoke(); // ✅ safe
    }

    public delegate void NotesActive(bool active);
    public static event NotesActive onNotesActive;

    public static void OnNotesActiveMethod(bool active)
    {
        onNotesActive?.Invoke(active); // ✅ safe
    }

    public delegate void ClearNumber();
    public static event ClearNumber onClearNumber;

    public static void OnClearNumberMethod()
    {
        onClearNumber?.Invoke(); // ✅ safe
    }

    public delegate void BoardCompleted();
    public static event BoardCompleted onBoardCompleted;

    public static void onBoardCompletedMethod()
    {
        onBoardCompleted?.Invoke(); // ✅ safe
    }

    public delegate void GiveAHint();
    public static event GiveAHint onGiveAHint;

    public static void OnGiveAHintMethod()
    {
        onGiveAHint?.Invoke(); // already safe
    }

    public delegate void GiveAHintAdOpening();
    public static event GiveAHintAdOpening onGiveAHintAdOpening;

    public static void OnGiveAHintAdOpeningMethod()
    {
        onGiveAHintAdOpening?.Invoke(); // ✅ safe
    }
}