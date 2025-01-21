using System;

[Serializable]
public class Level
{
    public string Name;
    public int Rows;
    public int Cols;
    public int Mines;
    public bool IsCustomShape;
    public string ShapeFile;
    public bool IsUnlocked;

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
