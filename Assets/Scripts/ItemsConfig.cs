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
        public ItemType Type;
        public int Level;
        public int MaxLevel;
        public List<Producer> Producers;
        public Item Prefab;
    }

    [SerializeField]
    private List<ItemData> items;

    public ItemData GetItemDataById(int id)
    {
        ItemData itemData = items.FirstOrDefault(i => i.Id == id);
        if (itemData != null) {
            return itemData;
        }

        throw new NotImplementedException($"There is no item with id {id} in config");
    }

    public int GetNextLevelItemId(int id)
    {
        ItemData itemData = items.FirstOrDefault(i => i.Id == id);
        if (itemData != null) {
            ItemData nextItemData = items.FirstOrDefault(i => i.Type == itemData.Type && i.Level == itemData.Level + 1);
            if (nextItemData == null) {
                Debug.LogError($"Trying to get next level item for already maxed item");
                return itemData.Id;
            }
            return nextItemData.Id;
        }
        
        throw new NotImplementedException($"There is no item with id {id} in config");
    }
}
