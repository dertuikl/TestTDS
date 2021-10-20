using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public Item item { get; private set; }
    public Vector2Int Coords { get; private set; }

    public bool IsEmpty => item == null;

    public GridCell Initialize(Vector2Int coords)
    {
        Coords = coords;
        return this;
    }
    
    public void PlaceItem(Item newItem)
    {
        if (item == null) {
            item = newItem;
            item.PlaceToGridCell(this);
        } else {
            throw new NotImplementedException($"Attempt to place item in not empty cell.");
        }
    }

    public bool CheckItemCanBePlaced(Item toPlace) => IsEmpty || item.CanBeMergedTo(toPlace);

    public void RemoveItem(Item item)
    {
        if (this.item == item) {
            this.item = null;
        }
    }

    public void Clear()
    {
        if(item != null) {
            Destroy(item.gameObject);
            RemoveItem(item);
        }
    }

    public GridCellSaveData GetSaveData()
    {
        return new GridCellSaveData {
            Coords = Coords,
            ItemData = item?.GetSaveData()
        };
    }
}

[DataContract]
[Serializable]
public class GridCellSaveData
{
    [DataMember]
    public Vector2Int Coords;
    [DataMember]
    public ItemSaveData ItemData;
}