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

    private Vector2 cellSize;
    private float leftBound;
    private float rightBound;
    private float topBound;
    private float bottomBound;

    private bool boardIsFull => emptyCellsCoords.Count == 0;
    private RectTransform gridTransform => layoutGroup.transform as RectTransform;

    public void CreateBoard(Vector2Int boardSize, ItemsConfig itemsConfig)
    {
        this.gridSize = boardSize;
        this.itemsConfig = itemsConfig;

        cellSize = CalculateCellSize();
        
        var toDestroy = gridTransform.GetComponentsInChildren<GridCell>();
        foreach (GridCell cell in toDestroy) {
            Destroy(cell.gameObject);
        }

        layoutGroup.constraintCount = boardSize.x;
        layoutGroup.cellSize = cellSize;
        
        gridMatrix = new GridCell[boardSize.y, boardSize.x];
        emptyCellsCoords = new List<Vector2Int>();
        for (int x = 0; x < boardSize.y; x++) {
            for (int y = 0; y < boardSize.y; y++) {
                Vector2Int coords = new Vector2Int(x, y);
                gridMatrix[x, y] = Instantiate(cellPrefab, gridTransform).Initialize(coords);
                emptyCellsCoords.Add(coords);
            }
        }
        
        StartCoroutine( CalculateBoardBounds());
        
        itemsOnBoard = new List<Item>();
    }

    private Vector2 CalculateCellSize()
    {
        float length = gridTransform.rect.width / gridSize.x;
        return Vector2.one * length;
    }

    private IEnumerator CalculateBoardBounds()
    {
        yield return new WaitForEndOfFrame();
        
        leftBound = gridMatrix[0, 0].transform.position.x - cellSize.x / 2;
        rightBound = gridMatrix[0, 0].transform.position.x + (gridSize.x - 1) * cellSize.x + cellSize.x / 2;
        topBound = gridMatrix[0, 0].transform.position.y + cellSize.y / 2;
        bottomBound = gridMatrix[0, 0].transform.position.y - (gridSize.y - 1) * cellSize.y - cellSize.y / 2;
    }

    public void CreateItemInRandomEmptyCell(int itemId)
    {
        if (!boardIsFull) {
            Item item = CreateItemById(itemId);
            Vector2Int coords = GetEmptyCellCoords();
            PlaceItem(coords, item);
        }
    }
    
    private Item CreateItemById(int id)
    {
        ItemsConfig.ItemData data = itemsConfig.GetItemDataById(id);
        Item item = Instantiate(data.Prefab);
        item.Initialize(this, data);
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
    
    private void PlaceItem(Vector2Int coords, Item item)
    {
        gridMatrix[coords.x, coords.y].PlaceItem(item);
        if(emptyCellsCoords.Contains(coords)) {
            emptyCellsCoords.Remove(coords);
        }
    }

    public void MoveItemOnBoard(Item item, GridCell targetCell)
    {
        emptyCellsCoords.Add(item.GridCell.Coords);
        item.GridCell.RemoveItem(item);

        if (!targetCell.IsEmpty) {
            int nextLvlItemId = itemsConfig.GetNextLevelItemId(item.Id);
            Item nextLvlItem = CreateItemById(nextLvlItemId);
            RemoveItem(targetCell.item);
            RemoveItem(item);
            PlaceItem(targetCell.Coords, nextLvlItem);
        } else {
            PlaceItem(targetCell.Coords, item);
        }
    }

    public GridCell GetCellByTouchPosition(Vector2 position)
    {
        int xMatrixCoord = gridSize.y - 1 - (int)((position.y - bottomBound) / cellSize.y);
        int yMatrixCoord = (int)((position.x - leftBound) / cellSize.x);

        if (IsOnGrid(new Vector2Int(xMatrixCoord, yMatrixCoord))) {
            return gridMatrix[xMatrixCoord, yMatrixCoord];
        }
        
        return null;
    }

    private bool IsOnGrid(Vector2Int matrixCoords) => matrixCoords.x >= 0 && matrixCoords.x < gridSize.y && matrixCoords.y >= 0 && matrixCoords.y < gridSize.x;

    private void RemoveItem(Item item)
    {
        itemsOnBoard.Remove(item);
        item.GridCell.Clear();
        Destroy(item.gameObject);
    }
}
