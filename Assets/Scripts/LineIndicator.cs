using UnityEngine;

public class LineIndicator : MonoBehaviour
{
    public static LineIndicator instance;

    private int[,] line_data = new int[9, 9]
    {
        {0,1,2, 3,4,5, 6,7,8},
        {9,10,11, 12,13,14, 15,16,17},
        {18,19,20, 21,22,23, 24,25,26},

        {27,28,29, 30,31,32, 33,34,35},
        {36,37,38, 39,40,41, 42,43,44},
        {45,46,47, 48,49,50, 51,52,53},

        {54,55,56, 57,58,59, 60,61,62},
        {63,64,65, 66,67,68, 69,70,71},
        {72,73,74, 75,76,77, 78,79,80},
    };

    private int[] line_data_flat = new int[81]
    {
        0,1,2,   3,4,5,    6,7,8,
        9,10,11, 12,13,14, 15,16,17,
        18,19,20, 21,22,23, 24,25,26,

        27,28,29, 30,31,32, 33,34,35,
        36,37,38, 39,40,41, 42,43,44,
        45,46,47, 48,49,50, 51,52,53,

        54,55,56, 57,58,59, 60,61,62,
        63,64,65, 66,67,68, 69,70,71,
        72,73,74, 75,76,77, 78,79,80,
    };

    private int[,] square_data = new int[9, 9]
    {
        {0,1,2,    9,10,11,  18,19,20 },
        {3,4,5,    12,13,14, 21,22,23 },
        {6,7,8,    15,16,17, 24,25,26 },
        {27,28,29, 36,37,38, 45,46,47 },
        {30,31,32, 39,40,41, 48,49,50 },
        {33,34,35, 42,43,44, 51,52,53 },
        {54,55,56, 63,64,65, 72,73,74 },
        {57,58,59, 66,67,68, 75,76,77 },
        {60,61,62, 69,70,71, 78,79,80 }
    };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private (int, int) GetSquarePosition(int squareIndex)
    {
        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                if (line_data[row, column] == squareIndex)
                {
                    return (row, column);
                }
            }
        }

        Debug.LogWarning("Invalid squareIndex: " + squareIndex);
        return (0, 0);
    }

    public int[] GetHorizontalLine(int squareIndex)
    {
        int[] line = new int[9];

        var pos = GetSquarePosition(squareIndex);
        int row = pos.Item1;

        if (row < 0 || row >= 9)
        {
            Debug.LogWarning("Invalid row for index: " + squareIndex);
            return line;
        }

        for (int i = 0; i < 9; i++)
        {
            line[i] = line_data[row, i];
        }

        return line;
    }

    public int[] GetVerticalLine(int squareIndex)
    {
        int[] line = new int[9];

        var pos = GetSquarePosition(squareIndex);
        int col = pos.Item2;

        if (col < 0 || col >= 9)
        {
            Debug.LogWarning("Invalid column for index: " + squareIndex);
            return line;
        }

        for (int i = 0; i < 9; i++)
        {
            line[i] = line_data[i, col];
        }

        return line;
    }

    public int[] GetSquare(int squareIndex)
    {
        int[] line = new int[9];
        int pos_row = -1;

        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (square_data[row, col] == squareIndex)
                {
                    pos_row = row;
                    break;
                }
            }

            if (pos_row != -1) break;
        }

        if (pos_row < 0 || pos_row >= 9)
        {
            Debug.LogWarning("Invalid squareIndex: " + squareIndex);
            return line;
        }

        for (int i = 0; i < 9; i++)
        {
            line[i] = square_data[pos_row, i];
        }

        return line;
    }

    public int[] GetAllSquareIndexes()
    {
        return line_data_flat;
    }
}