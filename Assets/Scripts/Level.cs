using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public string Name { get; set; }
    public int Rows { get; set; }
    public int Cols { get; set; }
    public int Mines { get; set; }
    public bool IsCustomShape { get; set; }
    public string ShapeFile { get; set; } // File path for custom grid shapes
    public bool IsUnlocked { get; set; }

    public Level(string name, int rows, int cols, int mines, bool isCustomShape = false, string shapeFile = "", bool isUnlocked = false)
    {
        Name = name;
        Rows = rows;
        Cols = cols;
        Mines = mines;
        IsCustomShape = isCustomShape;
        ShapeFile = shapeFile;
        IsUnlocked = isUnlocked;
    }
}
