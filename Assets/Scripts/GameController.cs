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
        BoardSaveData savedData = LoadSavedBoardState();
        if(savedData == null) {
            StartNewGame();
        } else {
            LoadSavedGame(savedData);
        }
    }

    private void StartNewGame()
    {
        Debug.Log("StartNewGame");
        board.CreateBoard(boardSize, itemsConfig);

        foreach (ItemPreset itemPreset in boardStart) {
            for (int i = 0; i < itemPreset.Count; i++) {
                board.CreateItemInRandomEmptyCell(itemPreset.Id);
            }
        }

        StartCoroutine(GameLoop());
    }

    private void LoadSavedGame(BoardSaveData savedData)
    {
        Debug.Log("LoadSavedGame");
        board.CreateBoard(savedData.GridSize, itemsConfig);

        foreach (GridCellSaveData cellSaveData in savedData.CellsSaveData) {
            board.CreateItemBySavedData(cellSaveData.ItemData, cellSaveData.Coords);
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

    private void OnApplicationQuit()
    {
        SaveBoardState();
    }

    private void SaveBoardState()
    {
        DataSaver saver = new DataSaver();
        saver.Save("Board", board.GetSaveData());
    }

    private BoardSaveData LoadSavedBoardState()
    {
        DataSaver saver = new DataSaver();
        return saver.Load("Board");
    }
}
