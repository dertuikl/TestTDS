using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsConfig", menuName = "ScriptableObjects/ItemsConfig")]
public class ItemsConfig : ScriptableObject
{
    [Serializable]
    public class ItemData
    {
        public int Id;
        public int StartLevel;
        public int MaxLevel;
        public List<Item.Producer> Producers;
        public Item Prefab;
    }

    [SerializeField]
    private List<ItemData> items;

    public ItemData GetItemDataById(int id)
    {
        ItemData itemData = items.FirstOrDefault(i => i.Id == id);
        if (itemData != null) {
            return itemData;
        } else {
            throw new NotImplementedException($"There is no item with id {id} in config");
        }
    }
}
