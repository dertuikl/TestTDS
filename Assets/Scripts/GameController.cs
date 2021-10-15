using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Vector2Int gridSize = new Vector2Int(7, 7);
    
    [SerializeField]
    private GridLayoutGroup layoutGroup;

    [SerializeField]
    private GameObject cellPrefab;

    private RectTransform gridTransform => layoutGroup.transform as RectTransform;
    
    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        layoutGroup.constraintCount = gridSize.x;
        layoutGroup.cellSize = CalculateCellSize();
        
        for (int i = 0; i < gridSize.x * gridSize.y; i++) {
            Instantiate(cellPrefab, gridTransform);
        }
    }

    private Vector2 CalculateCellSize()
    {
        float length = gridTransform.rect.width / gridSize.x;
        return Vector2.one * length;
    }
}
