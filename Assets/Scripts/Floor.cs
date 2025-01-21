using UnityEngine;
using TMPro;

public class Floor : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material borderShapedMaterial;
    [SerializeField] private Material borderSquareMaterial;
    private Renderer floorRenderer;

    [SerializeField] private TMP_Text textMeshPro;
    private int cellValue = 0;
    private bool isBorder = false;
    private bool shapedLevel = false;

    private void Start()
    {
        floorRenderer = GetComponent<Renderer>();
        UpdateVisuals();
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
        if (cellValue == -1 || cellValue == 0)
        {
            textMeshPro.text = ""; // Mine or empty value
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
            case 1: return new Color(0f, 0f, 0.976f); // 1 is blue
            case 2: return new Color(0.02f, 0.486f, 0f); // 2 is green
            case 3: return new Color(0.796f, 0.027f, 0.027f); // 3 is red
            case 4: return new Color(0.067f, 0.035f, 0.478f); // 4 is dark blue
            case 5: return new Color(0.427f, 0.039f, 0.063f); // 5 is brown
            case 6: return Color.cyan; // 6 is cyan
            case 7: return Color.black; // 7 is black
            case 8: return Color.gray; // 8 is gray
            case -1: return new Color(0.1f, 0.1f, 0.1f); // mine is dark gray
            default: return Color.clear; // Default: empty or invalid
        }
    }

    public void UpdateVisuals()
    {
        if (floorRenderer == null) return;

        if (isBorder)
        {
            if (borderShapedMaterial != null && shapedLevel)
            {
                floorRenderer.material = borderShapedMaterial;
            }
            else if (borderSquareMaterial != null)
            {
                floorRenderer.material = borderSquareMaterial;
            }
        }
        else
        {
            if (defaultMaterial != null)
            {
                floorRenderer.material = defaultMaterial;
            }
        }
    }

    public void SetBorder()
    {
        isBorder = true;
    }

    public void SetShapedLevel()
    {
        shapedLevel = true;
    }
}
