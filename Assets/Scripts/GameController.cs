using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Vector2Int boardSize = new Vector2Int(7, 7);

    [Serializable]
    public struct ItemPreset
    {
        [SerializeField]
        public int Id;
        [SerializeField]
        public int Count;
    }
    
    [SerializeField]
    private List<ItemPreset> boardStart;

    [SerializeField]
    private Board board;
    [SerializeField]
    private ItemsConfig itemsConfig;

    private void Start()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        board.CreateBoard(boardSize);

        foreach (ItemPreset itemPreset in boardStart) {
            for (int i = 0; i < itemPreset.Count; i++) {
                board.PlaceItemToEmptyCell(CreateItemById(itemPreset.Id));
            }
        }
    }

    private Item CreateItemById(int id)
    {
        ItemsConfig.ItemData data = itemsConfig.GetItemDataById(id);
        Item item = Instantiate(data.Prefab);
        item.Initialize(data.StartLevel, data.MaxLevel, data.Producers);
        return item;
    }
}
