using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineManager : MonoBehaviour
{
    private int[,] grid; // The grid with numbers and mines (-1 for mine, 0 for empty, 1-8 for numbers)

    public void GenerateMinesAndAssignValues(int rows, int cols, int mineCount, Tuple<int, int> safeTileCoords, Floor[,] floorGrid)
    {
        grid = new int[rows, cols];

        PlaceMines(grid, rows, cols, mineCount, safeTileCoords);
        CalculateNumbers(grid, rows, cols, floorGrid);
    }

    private void PlaceMines(int[,] grid, int rows, int cols, int mineCount, Tuple<int, int> safeZone)
    {
        int placedMines = 0;

        while (placedMines < mineCount)
        {
            int row = UnityEngine.Random.Range(0, rows);
            int col = UnityEngine.Random.Range(0, cols);

            if (IsInSafeZone(row, col, safeZone) || grid[row, col] == -1)
                continue;

            grid[row, col] = -1;
            placedMines++;
        }
    }

    private bool IsInSafeZone(int row, int col, Tuple<int, int> safeZone)
    {
        int safeRow = safeZone.Item1;
        int safeCol = safeZone.Item2;

        return row >= safeRow - 1 && row <= safeRow + 1 &&
               col >= safeCol - 1 && col <= safeCol + 1;
    }

    private void CalculateNumbers(int[,] grid, int rows, int cols, Floor[,] floorGrid)
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (grid[row, col] == -1)
                {
                    floorGrid[row, col].SetCellValue(-1);
                    continue;
                }

                int mineCount = 0;

                // Check all adjacent tiles
                for (int i = row - 1; i <= row + 1; i++)
                {
                    for (int j = col - 1; j <= col + 1; j++)
                    {
                        if (IsInBounds(i, j, rows, cols) && grid[i, j] == -1)
                        {
                            mineCount++;
                        }
                    }
                }

                grid[row, col] = mineCount;
                floorGrid[row, col].SetCellValue(mineCount);
            }
        }
    }

    public bool IsInBounds(int row, int col, int rows, int cols)
    {
        return row >= 0 && row < rows && col >= 0 && col < cols;
    }

    public bool IsMine(int row, int col)
    {
        return grid[row, col] == -1;
    }

    public bool IsEmpty(int row, int col)
    {
        return grid[row, col] == 0;
    }

    public int GetCellValue(int row, int col)
    {
        return grid[row, col];
    }
}
