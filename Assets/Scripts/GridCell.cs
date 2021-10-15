using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField]
    private Item item;

    public bool IsEmpty => item == null;

    public void PlaceItem(Item item)
    {
        this.item = item;
        item.transform.SetParent(transform, false);
    }
}
