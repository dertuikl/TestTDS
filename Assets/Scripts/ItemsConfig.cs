using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemsConfig : MonoBehaviour
{
    [Serializable]
    public class ItemConfig
    {
        public int Id;
        public int StartLevel;
        public int MaxLevel;
        public List<Item.Producer> Producers;
        public Item Prefab;
    }

    [SerializeField]
    private List<ItemConfig> items;

    public Item GetItemById(int id)
    {
        ItemConfig config = items.FirstOrDefault(i => i.Id == id);
        if (config != null) {
            Item item = Instantiate(config.Prefab);
            item.Initialize(config.StartLevel, config.MaxLevel, config.Producers);
            return item;
        } else {
            throw new NotImplementedException($"There is no item with id {id} in config");
        }
    }
}
