using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class Config : MonoBehaviour
{
    static string path = Path.Combine(Application.persistentDataPath, "boarddata.ini");

    // 🔥 DELETE SAVE (call this on game completion)
    public static void DeleteDataFile()
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("Save file deleted");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("DeleteDataFile Error: " + e.Message);
        }
    }

    // 🔥 CHECK IF SAVE EXISTS (used for Continue button)
    public static bool GameDataExist()
    {
        try
        {
            return File.Exists(path);
        }
        catch (System.Exception e)
        {
            Debug.LogError("GameDataExist Error: " + e.Message);
            return false;
        }
    }

    // 🔥 SAVE GAME
    public static void SaveBoardData(
        SudokuData.SudokuBoardData boardData,
        string level,
        int boardIndex,
        int errorNumber,
        Dictionary<string, List<string>> GridNotes)
    {
        Debug.Log("Saving at Path: " + path);

        try
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.WriteLine("#time:" + Clock.GetCurrentTime());
                writer.WriteLine("#level:" + level);
                writer.WriteLine("#errors:" + errorNumber);
                writer.WriteLine("#board_index:" + boardIndex);

                // Unsolved
                string unsolvedString = "#unsolved:";
                if (boardData.unsolvedData != null)
                {
                    foreach (var val in boardData.unsolvedData)
                        unsolvedString += val + ",";
                }
                writer.WriteLine(unsolvedString);

                // Solved
                string solvedString = "#solved:";
                if (boardData.solvedData != null)
                {
                    foreach (var val in boardData.solvedData)
                        solvedString += val + ",";
                }
                writer.WriteLine(solvedString);

                // Notes
                if (GridNotes != null)
                {
                    foreach (var square in GridNotes)
                    {
                        string squareString = "#" + square.Key + ":";
                        bool save = false;

                        if (square.Value != null)
                        {
                            foreach (var note in square.Value)
                            {
                                if (note != " ")
                                {
                                    squareString += note + ",";
                                    save = true;
                                }
                            }
                        }

                        if (save)
                            writer.WriteLine(squareString);
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("SaveBoardData Error: " + e.Message);
        }
    }

    // 🔥 SAFE FILE READER CHECK
    static bool FileReady()
    {
        try
        {
            if (!File.Exists(path))
            {
                Debug.LogWarning("Save file not found");
                return false;
            }
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("FileReady Error: " + e.Message);
            return false;
        }
    }

    // 🔥 READ LEVEL STRING
    public static string ReadBoardLevel()
    {
        if (!FileReady()) return "";

        try
        {
            foreach (var line in File.ReadLines(path))
            {
                var word = line.Split(':');
                if (word.Length > 1 && word[0] == "#level")
                    return word[1];
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("ReadBoardLevel Error: " + e.Message);
        }

        return "";
    }

    // 🔥 READ BOARD INDEX
    public static int ReadGameBoardLevel()
    {
        if (!FileReady()) return -1;

        try
        {
            foreach (var line in File.ReadLines(path))
            {
                var word = line.Split(':');
                if (word.Length > 1 && word[0] == "#board_index")
                {
                    int.TryParse(word[1], out int level);
                    return level;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("ReadGameBoardLevel Error: " + e.Message);
        }

        return -1;
    }

    // 🔥 READ TIME
    public static float ReadGameTime()
    {
        if (!FileReady()) return -1;

        try
        {
            foreach (var line in File.ReadLines(path))
            {
                var word = line.Split(':');
                if (word.Length > 1 && word[0] == "#time")
                {
                    float.TryParse(word[1], out float time);
                    return time;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("ReadGameTime Error: " + e.Message);
        }

        return -1;
    }

    // 🔥 READ ERRORS
    public static int ErrorNumber()
    {
        if (!FileReady()) return 0;

        try
        {
            foreach (var line in File.ReadLines(path))
            {
                var word = line.Split(':');
                if (word.Length > 1 && word[0] == "#errors")
                {
                    int.TryParse(word[1], out int errors);
                    return errors;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("ErrorNumber Error: " + e.Message);
        }

        return 0;
    }

    // 🔥 READ GRID DATA
    public static SudokuData.SudokuBoardData ReadGridData()
    {
        if (!FileReady())
            return new SudokuData.SudokuBoardData(new int[81], new int[81]);

        int[] unsolved = new int[81];
        int[] solved = new int[81];
        int uIndex = 0, sIndex = 0;

        try
        {
            foreach (var line in File.ReadLines(path))
            {
                var word = line.Split(':');

                if (word.Length > 1 && word[0] == "#unsolved")
                {
                    var values = Regex.Split(word[1], ",");
                    foreach (var v in values)
                    {
                        if (int.TryParse(v, out int num) && uIndex < 81)
                            unsolved[uIndex++] = num;
                    }
                }

                if (word.Length > 1 && word[0] == "#solved")
                {
                    var values = Regex.Split(word[1], ",");
                    foreach (var v in values)
                    {
                        if (int.TryParse(v, out int num) && sIndex < 81)
                            solved[sIndex++] = num;
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("ReadGridData Error: " + e.Message);
        }

        return new SudokuData.SudokuBoardData(unsolved, solved);
    }

    // 🔥 READ NOTES
    public static Dictionary<int, List<int>> GetGridNotes()
    {
        Dictionary<int, List<int>> gridNotes = new();

        if (!FileReady()) return gridNotes;

        try
        {
            foreach (var line in File.ReadLines(path))
            {
                var word = line.Split(':');

                if (word.Length > 2 && word[0] == "#square_note")
                {
                    int.TryParse(word[1], out int index);

                    List<int> notes = new();
                    var values = Regex.Split(word[2], ",");

                    foreach (var v in values)
                    {
                        if (int.TryParse(v, out int num) && num > 0)
                            notes.Add(num);
                    }

                    gridNotes[index] = notes;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("GetGridNotes Error: " + e.Message);
        }

        return gridNotes;
    }
}