using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileState { NotFlagged, Flagged }
    private TileState currentState = TileState.NotFlagged;

    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material flaggedMaterial;
    [SerializeField] private Transform flag;

    private Renderer tileRenderer;

    private void Start()
    {
        tileRenderer = GetComponent<Renderer>();
        UpdateTileVisuals();
    }

    public void ToggleFlag()
    {
        currentState = currentState == TileState.Flagged ? TileState.NotFlagged : TileState.Flagged;
        UpdateTileVisuals();
    }

    public bool TryDestroyTile()
    {
        if (currentState == TileState.Flagged)
        {
            Debug.Log("Cannot destroy flagged tile.");
            return false;
        }

        Debug.Log("Tile destroyed: " + name);
        Destroy(gameObject);
        return true;
    }

    private void UpdateTileVisuals()
    {
        if (tileRenderer == null) return;

        if (currentState == TileState.Flagged)
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

    public bool IsFlagged()
    {
        return currentState == TileState.Flagged;
    }
}
