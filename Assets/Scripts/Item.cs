using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ItemType
{
    A,
    B
}

public class Item : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private ActiveProducerView activeProducerViewPrefab;

    private List<Producer> producers;
    private Board board;
    private ItemType itemType;
    private int level;
    private int maxLevel;
    private bool pointerIsDown;
    private ActiveProducerView activeProducerView;
    
    public GridCell GridCell { get; private set; }
    public int Id { get; private set; }
    
    public bool LvlIsMax => level == maxLevel;
    private RectTransform rectTransform => transform as RectTransform;
    private ItemType type;

    public void Initialize(Board board, ItemsConfig.ItemData itemData)
    {
        this.board = board;
        Id = itemData.Id;
        itemType = itemData.Type;
        level = itemData.Level;
        maxLevel = itemData.MaxLevel;
        producers = itemData.Producers;
        
        foreach (Producer producer in producers) {
            ActiveProducerView view = null;
            if (producer.Type == Producer.ProducerType.Active) {
                view = Instantiate(activeProducerViewPrefab, transform, false);
            }
            producer.Initialize(board, view);
        }

        UpdateView();
    }

    public void PlaceToGridCell(GridCell gridCell)
    {
        GridCell = gridCell;
        transform.SetParent(gridCell.transform, false);
        
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }

    private void UpdateView()
    {
        levelText.text = $"{itemType}{level}";
    }

    public void Tick()
    {
        foreach (Producer producer in producers) {
            producer.Tick();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerIsDown = true;
        transform.SetParent(board.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        pointerIsDown = false;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(board.transform as RectTransform, eventData.position, eventData.enterEventCamera, out Vector2 newPosition);
        rectTransform.anchoredPosition = newPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (pointerIsDown) {
            OnClick();
        }

        pointerIsDown = false;

        GridCell cellUnderItem = board.GetCellByTouchPosition(eventData.position);
        if (cellUnderItem == null || !cellUnderItem.CheckItemCanBePlaced(this)) {
            ReturnToGridCell();
            return;
        }
        
        board.MoveItemOnBoard(this, cellUnderItem);
    }

    private void ReturnToGridCell()
    {
        PlaceToGridCell(GridCell);
    }

    private void OnClick()
    { 
        foreach (Producer producer in producers.Where(p => p.Type == Producer.ProducerType.Active)) {
            producer.ProduceItem();
        }
    }
}
