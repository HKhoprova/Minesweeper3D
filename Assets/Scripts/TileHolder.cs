using System;
using UnityEngine;

public class TileHolder : MonoBehaviour
{
    private bool isFlagged = false;
    private bool isRevealed = false;

    private int row, col;

    [SerializeField] private Transform tileObject;
    [SerializeField] private Transform flagPrefab;
    private Tile tile;

    private Collider tileCollider;

    private Transform flagInstance;

    private void Start()
    {
        tile = tileObject.GetComponent<Tile>();
        tileCollider = GetComponent<Collider>();
    }

    public void ToggleFlag()
    {
        if (tile == null || isRevealed)
            return;

        isFlagged = !isFlagged;

        if (isFlagged)
        {
            PlaceFlag();
            tile.Flag();
        }
        else
        {
            RemoveFlag();
            tile.UnFlag();
        }
    }

    public bool TryRevealTile(bool isGameLost)
    {
        if (tile == null || isFlagged || isRevealed)
        {
            return false;
        }

        if (tileCollider == null)
        {
            tileCollider = GetComponent<Collider>();
        }

        tile.Reveal(isGameLost, tileCollider);
        isRevealed = true;
        return true;
    }

    public void MarkAsIncorrect()
    {
        if (tile != null) 
            tile.SetIncorrect();
    }

    private void PlaceFlag()
    {
        if (flagPrefab == null || flagInstance != null)
            return;

        flagInstance = Instantiate(flagPrefab, transform.position + Vector3.up * 1f, Quaternion.identity, transform);

        Flag flag = flagInstance.GetComponent<Flag>();
        if (flag != null)
        {
            flag.ShowFlag();
        }
    }

    private void RemoveFlag()
    {
        if (flagInstance == null)
            return;

        Flag flag = flagInstance.GetComponent<Flag>();
        if (flag != null)
        {
            flag.HideFlag();
        }

        Destroy(flagInstance.gameObject, 0.5f);
        flagInstance = null;
    }

    public bool IsFlagged()
    {
        return isFlagged;
    }

    public bool IsRevealed()
    {
        return isRevealed;
    }

    public void SetCoords(int row, int col)
    {
        this.row = row;
        this.col = col;
    }

    public Tuple<int, int> GetCoords()
    {
        return Tuple.Create(row, col);
    }
}
