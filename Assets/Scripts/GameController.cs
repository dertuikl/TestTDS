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

    // can be used for speedup booster or smth
    private float tickTime = 1.0f;
    
    private void Start()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        board.CreateBoard(boardSize, itemsConfig);

        foreach (ItemPreset itemPreset in boardStart) {
            for (int i = 0; i < itemPreset.Count; i++) {
                board.CreateItemInRandomEmptyCell(itemPreset.Id);
            }
        }

        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (true) {
            //TODO: if game is active
            board.Tick();
            
            yield return new WaitForSeconds(tickTime);
        }
    }
}
