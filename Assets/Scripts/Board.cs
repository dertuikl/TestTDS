using System.Collections;
using System.Collections.Generic;
using System.Numerics;
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
    private GridCell[,] gridMatrix;
    private List<Vector2Int> emptyCellsCoords;
    
    private RectTransform gridTransform => layoutGroup.transform as RectTransform;

    public void CreateBoard(Vector2Int boardSize)
    {
        this.gridSize = boardSize;

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
    }

    private Vector2 CalculateCellSize()
    {
        float length = gridTransform.rect.width / gridSize.x;
        return Vector2.one * length;
    }

    public void PlaceItemToEmptyCell(Item item)
    {
        Vector2Int coords = GetEmptyCellCoords();
        Debug.Log($"Coords: {coords}");
        PlaceItem(coords, item);
    }
    
    private Vector2Int GetEmptyCellCoords()
    {
        int index = Random.Range(0, emptyCellsCoords.Count - 1);
        return emptyCellsCoords[index];
    }
    
    public void PlaceItem(Vector2Int coords, Item item)
    {
        gridMatrix[coords.x, coords.y].PlaceItem(item);
        emptyCellsCoords.Remove(coords);
    }
}
