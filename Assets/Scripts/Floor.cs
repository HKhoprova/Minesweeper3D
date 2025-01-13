using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Floor : MonoBehaviour
{
    [SerializeField] private TMP_Text textMeshPro;
    private int cellValue = 0;

    private void Start()
    {
        UpdateText();
    }

    public void SetCellValue(int value)
    {
        cellValue = value;
        UpdateText();
    }

    private void UpdateText()
    {
        if (textMeshPro == null)
        {
            Debug.LogWarning("textMeshPro is not assigned for " + gameObject.name);
            return;
        }

        // Update the displayed text based on the value
        if (cellValue == -1)
        {
            textMeshPro.text = "*"; // Mine
        }
        else if (cellValue == 0)
        {
            textMeshPro.text = ""; // Empty
        }
        else
        {
            textMeshPro.text = cellValue.ToString(); // Numbers 1-8
        }

        textMeshPro.color = GetTextColor(cellValue);
    }

    private Color GetTextColor(int value)
    {
        switch (value)
        {
            case 1: return Color.blue; // 1 is blue
            case 2: return Color.green; // 2 is green
            case 3: return Color.red; // 3 is red
            case 4: return new Color(0.2f, 0.2f, 1f); // 4 is dark blue
            case 5: return new Color(0.5f, 0f, 0f); // 5 is dark red
            case 6: return Color.cyan; // 6 is cyan
            case 7: return Color.black; // 7 is black
            case 8: return Color.gray; // 8 is gray
            case -1: return new Color(0.1f, 0.1f, 0.1f); // mine is dark gray
            default: return Color.clear; // Default: empty or invalid
        }
    }
}
