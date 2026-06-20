using System.Collections.Generic;
using UnityEngine;

public class SudokuGrid : MonoBehaviour
{
    public int columns = 0;
    public int rows = 0;
    public float square_offset = 0;
    public GameObject grid_square;
    public Vector2 startPosition = new Vector2(0.0f, 0.0f);
    public float square_scale = 1.0f;
    private List<GameObject> grid_squares = new List<GameObject>();
    private int selected_grid_data = -1;
    public float square_gap = 0.1f;
    public Color lineHighlightColor = Color.blue;

    void Start()
    {
        if (grid_square.GetComponent<GridSquare>() == null)
            Debug.LogError("This Game Object need to have GridSquare script attached!!");

        CreateGrid();

        if (GameSettings.Instance.GetContinuePreviousGame())
        {
            SetGridFromFile();
        }
        else
        {
            SetGridNumber(GameSettings.Instance.GetGameMode());
        }
    }

    void SetGridFromFile()
    {
        string level = GameSettings.Instance.GetGameMode();
        selected_grid_data = Config.ReadGameBoardLevel();

        var data = Config.ReadGridData();
        SetGridSquareData(data);
        SetGridNotes(Config.GetGridNotes());
    }

    private void SetGridNotes(Dictionary<int, List<int>> notes)
    {
        foreach (var note in notes)
        {
            grid_squares[note.Key].GetComponent<GridSquare>().SetGridNotes(note.Value);
        }
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetSquarePosition();
    }

    private void SetGridNumber(string level)
    {
        selected_grid_data = Random.Range(0, SudokuData.Instance.sudoku_game[level].Count);
        var data = SudokuData.Instance.sudoku_game[level][selected_grid_data];
        SetGridSquareData(data);
    }

    private void SpawnGridSquares()
    {
        int squareIndex = 0;

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                grid_squares.Add(Instantiate(grid_square) as GameObject);
                grid_squares[grid_squares.Count - 1].GetComponent<GridSquare>().setSquareIndex(squareIndex);
                grid_squares[grid_squares.Count - 1].transform.parent = this.transform;
                grid_squares[grid_squares.Count - 1].transform.localScale =
                    new Vector3(square_scale, square_scale, square_scale);
                squareIndex++;
            }
        }
    }

    private void SetSquarePosition()
    {
        var square_rect = grid_squares[0].GetComponent<RectTransform>();

        Vector2 offset = new Vector2();
        Vector2 square_gap_number = new Vector2(0.0f, 0.0f);

        bool row_moved = false;

        offset.x = square_rect.rect.width * square_rect.transform.localScale.x + square_offset;
        offset.y = square_rect.rect.height * square_rect.transform.localScale.y + square_offset;

        int column_number = 0;
        int row_number = 0;

        foreach (GameObject square in grid_squares)
        {
            if (column_number + 1 > columns)
            {
                row_number++;
                column_number = 0;
                square_gap_number.x = 0;
                row_moved = false;
            }

            var pos_x_offset = offset.x * column_number + (square_gap_number.x * square_gap);
            var pos_y_offset = offset.y * row_number + (square_gap_number.y * square_gap);

            if (column_number > 0 && column_number % 3 == 0)
            {
                square_gap_number.x++;
                pos_x_offset += square_gap;
            }

            if (row_number > 0 && row_number % 3 == 0 && row_moved == false)
            {
                row_moved = true;
                square_gap_number.y++;
                pos_y_offset += square_gap;
            }

            square.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset);

            column_number++;
        }
    }

    private void SetGridSquareData(SudokuData.SudokuBoardData data)
    {
        for (int i = 0; i < grid_squares.Count; i++)
        {
            grid_squares[i].GetComponent<GridSquare>()
                .SetHasDefaultValue(data.unsolvedData[i] != 0 && data.unsolvedData[i] == data.solvedData[i]);

            grid_squares[i].GetComponent<GridSquare>().SetNumber(data.unsolvedData[i]);
            grid_squares[i].GetComponent<GridSquare>().SetCorrectNumber(data.solvedData[i]);
        }
    }

    private void OnEnable()
    {
        GameEvents.OnSqaureSelected += OnSquareSelected;
        GameEvents.onCheckBoardCompleted += CheckBoardCompleted;
        GameEvents.onGiveAHint += GiveAHint;
    }

    private void OnDisable()
    {
        GameEvents.OnSqaureSelected -= OnSquareSelected;
        GameEvents.onCheckBoardCompleted -= CheckBoardCompleted;
        GameEvents.onGiveAHint -= GiveAHint;

        SaveOnExit(); // ✅ ONLY CHANGE (safe save)
        GameSettings.Instance.SetExitAfterWon(false);
    }

    // ✅ SAFE SAVE METHOD (NO LOGIC CHANGE)
    private void SaveOnExit()
    {
        if (grid_squares == null || grid_squares.Count == 0)
            return;

        var solvedData = SudokuData.Instance
            .sudoku_game[GameSettings.Instance.GetGameMode()][selected_grid_data]
            .solvedData;

        int[] unsolvedData = new int[81];
        Dictionary<string, List<string>> GridNotes = new Dictionary<string, List<string>>();

        for (int i = 0; i < grid_squares.Count; i++)
        {
            var comp = grid_squares[i].GetComponent<GridSquare>();
            unsolvedData[i] = comp.GetNumber();

            string key = "square_note:" + i;
            GridNotes.Add(key, comp.GetSquareNotes());
        }

        SudokuData.SudokuBoardData currentGameData =
            new SudokuData.SudokuBoardData(unsolvedData, solvedData);

        if (!GameSettings.Instance.GetExitAfterWon())
        {
            Config.SaveBoardData(
                currentGameData,
                GameSettings.Instance.GetGameMode(),
                selected_grid_data,
                Lives.instance.GetErrorNumber(),
                GridNotes
            );
        }
        else
        {
            Debug.Log("Game Won");
            Config.DeleteDataFile();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveOnExit(); // ✅ Android safe
        }
    }

    private void OnApplicationQuit()
    {
        SaveOnExit(); // ✅ exit safe
    }

    private void GiveAHint()
    {
        var squareIndexes = new List<int>();

        for (var index = 0; index < grid_squares.Count; index++)
        {
            var comp = grid_squares[index].GetComponent<GridSquare>();

            if (comp.GetHasDefaultValue() == false && comp.GetNumber() == 0)
            {
                squareIndexes.Add(index);
            }
        }

        if (squareIndexes.Count == 0)
            return;

        var random_index = Random.Range(0, squareIndexes.Count);
        var square_index = squareIndexes[random_index];

        grid_squares[square_index].GetComponent<GridSquare>().SetCorrectValueOnHint();
    }

    private void SetSquaresColors(int[] data, Color col)
    {
        foreach (var index in data)
        {
            var comp = grid_squares[index].GetComponent<GridSquare>();

            if (comp.HasWrongValue() == false && comp.isSelected() == false)
            {
                comp.SetSquareColor(col);
            }
        }
    }

    public void OnSquareSelected(int square_index)
    {
        var horizontalLine = LineIndicator.instance.GetHorizontalLine(square_index);
        var verticalLine = LineIndicator.instance.GetVerticalLine(square_index);
        var square = LineIndicator.instance.GetSquare(square_index);

        if (grid_squares[square_index].GetComponent<GridSquare>().GetHasDefaultValue() == false)
        {
            SetSquaresColors(LineIndicator.instance.GetAllSquareIndexes(), Color.white);
            SetSquaresColors(horizontalLine, lineHighlightColor);
            SetSquaresColors(verticalLine, lineHighlightColor);
            SetSquaresColors(square, lineHighlightColor);
        }
        else
        {
            foreach (var gridsquare in grid_squares)
            {
                var comp = gridsquare.GetComponent<GridSquare>();

                if (comp.HasWrongValue() == false && comp.isSelected() == false)
                {
                    comp.SetSquareColor(Color.white);
                }
            }
        }
    }

    private void CheckBoardCompleted()
    {
        foreach (var square in grid_squares)
        {
            var comp = square.GetComponent<GridSquare>();

            if (comp.IsCorrectNumberSet() == false)
                return;
        }

        AdManager.Instance.ShowInterstitialAd();
        GameEvents.onBoardCompletedMethod();
    }
}