using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField]
    private Item item;
    
    public Vector2Int Coords { get; private set; }

    public bool IsEmpty => item == null;

    public GridCell Initialize(Vector2Int coords)
    {
        Coords = coords;
        return this;
    }
    
    public void PlaceItem(Item item)
    {
        this.item = item;
        item.PlaceToGridCell(this);
    }

    public bool CheckItemCanBePlaced(Item toPlace)
    {
        Debug.LogError("CheckItemCanBePlaced is nor implemented yet!");
        return true;
    }

    public void RemoveItem(Item item)
    {
        if (this.item == item) {
            this.item = null;
        }
    }
}
