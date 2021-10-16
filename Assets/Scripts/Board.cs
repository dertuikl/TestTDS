using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class Board : MonoBehaviour
{
    [SerializeField]
    private GridLayoutGroup layoutGroup;
    [SerializeField]
    private GridCell cellPrefab;
    
    private Vector2Int gridSize;
    private ItemsConfig itemsConfig;
    private GridCell[,] gridMatrix;
    private List<Vector2Int> emptyCellsCoords;
    private List<Item> itemsOnBoard;

    private bool boardIsFull => emptyCellsCoords.Count == 0;
    private RectTransform gridTransform => layoutGroup.transform as RectTransform;

    public void CreateBoard(Vector2Int boardSize, ItemsConfig itemsConfig)
    {
        this.gridSize = boardSize;
        this.itemsConfig = itemsConfig;

        var toDestroy = gridTransform.GetComponentsInChildren<GridCell>();
        foreach (GridCell cell in toDestroy) {
            Destroy(cell.gameObject);
        }

        layoutGroup.constraintCount = boardSize.x;
        layoutGroup.cellSize = CalculateCellSize();
        
        gridMatrix = new GridCell[boardSize.y, boardSize.x];
        emptyCellsCoords = new List<Vector2Int>();
        for (int x = 0; x < boardSize.y; x++) {
            for (int y = 0; y < boardSize.y; y++) {
                gridMatrix[x, y] = Instantiate(cellPrefab, gridTransform);
                emptyCellsCoords.Add(new Vector2Int(x, y));
            }
        }
        
        itemsOnBoard = new List<Item>();
    }

    private Vector2 CalculateCellSize()
    {
        float length = gridTransform.rect.width / gridSize.x;
        return Vector2.one * length;
    }

    public void CreateItemInRandomEmptyCell(int itemId)
    {
        if (!boardIsFull) {
            Item item = CreateItemById(itemId);
            Vector2Int coords = GetEmptyCellCoords();
            Debug.Log($"Coords: {coords}");
            PlaceItem(coords, item);
        }
    }
    
    private Item CreateItemById(int id)
    {
        ItemsConfig.ItemData data = itemsConfig.GetItemDataById(id);
        Item item = Instantiate(data.Prefab);
        item.Initialize(this, data.StartLevel, data.MaxLevel, data.Producers);
        itemsOnBoard.Add(item);
        return item;
    }
    
    private Vector2Int GetEmptyCellCoords()
    {
        int index = Random.Range(0, emptyCellsCoords.Count - 1);
        return emptyCellsCoords[index];
    }

    public void Tick()
    {
        for (int i = 0; i < itemsOnBoard.Count; i++) {
            itemsOnBoard[i].Tick();
        }
    }
    
    public void PlaceItem(Vector2Int coords, Item item)
    {
        gridMatrix[coords.x, coords.y].PlaceItem(item);
        emptyCellsCoords.Remove(coords);
    }
}
