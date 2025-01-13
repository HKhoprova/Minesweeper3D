using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileState { NotFlagged, Flagged }
    private int row, col;
    private bool isFlagged = false;
    private bool isRevealed = false;

    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material flaggedMaterial;
    [SerializeField] private Material incorrectMaterial;
    [SerializeField] private Transform flag; 
    
    private Renderer tileRenderer;
    private Collider tileCollider;

    private void Start()
    {
        tileRenderer = GetComponent<Renderer>();
        tileCollider = GetComponent<Collider>();
        UpdateTileVisuals();
    }

    public void ToggleFlag()
    {
        if (isRevealed)
            return;

        isFlagged = !isFlagged;

        UpdateTileVisuals();
    }

    public bool TryRevealTile()
    {
        if (isFlagged)
        {
            Debug.Log("Cannot reveal flagged tile.");
            return false;
        }

        isRevealed = true;
        tileRenderer.enabled = false;
        tileCollider.enabled = false;
        Debug.Log("Tile revealed: " + name);
        return true;
    }

    private void UpdateTileVisuals()
    {
        if (tileRenderer == null) return;

        if (isFlagged)
        {
            if (flaggedMaterial != null)
            {
                tileRenderer.material = flaggedMaterial;
            }
            if (flag != null)
            {
                //place flag on tile
            }
        }
        else if (defaultMaterial != null)
        {
            tileRenderer.material = defaultMaterial;
        }
    }

    public void MarkAsIncorrect()
    {
        if (tileRenderer == null) return;
        tileRenderer.material = incorrectMaterial;
    }

    public bool IsFlagged()
    {
        return isFlagged;
    }

    public bool IsRevealed()
    {
        return isRevealed;
    }

    public void SetCoords(int row,  int col)
    {
        this.row = row;
        this.col = col;
    }

    public Tuple<int, int> GetCoords()
    {
        return Tuple.Create(row, col);
    }
}
