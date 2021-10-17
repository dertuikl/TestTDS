using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Serializable]
    public class Producer
    {
        public enum ProducerType
        {
            Passive,
            Active
        }

        public ProducerType Type;
        public int ChargesMax;
        public float ChargeRefillTime;
        public int ItemToProduceId;

        public int Charges { get; private set; }
        public float TimeToCharge { get; private set; }

        private Board board;

        public void Initialize(Board board)
        {
            this.board = board;
            Charges = ChargesMax;
            TimeToCharge = ChargeRefillTime;
        }

        public void Tick()
        {
            TimeToCharge--;
            // Debug.Log(TimeToCharge);
            if (Type == ProducerType.Passive) {
                ProduceItem();
            }
        }
        
        public void ProduceItem()
        {
            if (TimeToCharge > 0) {
                return;
            }
            
            if(Type == ProducerType.Passive) {
                board.CreateItemInRandomEmptyCell(ItemToProduceId);
                TimeToCharge = ChargeRefillTime;
            }

            if (Type == ProducerType.Active) {
                if (Charges > 0) {
                    Charges--;
                    TimeToCharge += ChargeRefillTime;
                    board.CreateItemInRandomEmptyCell(ItemToProduceId);
                }
            }
        }
    }

    [SerializeField]
    private List<Producer> producers;
    [SerializeField]
    private int currentLevel = 0;
    [SerializeField]
    private int maxLevel = 6;
    [SerializeField]
    private Text levelText;

    private Board board;
    private bool pointerIsDown;
    
    public GridCell GridCell { get; private set; }
    
    private RectTransform rectTransform => transform as RectTransform;

    public void Initialize(Board board, int startLevel, int maxLevel, List<Producer> producers)
    {
        this.board = board;
        this.currentLevel = startLevel;
        this.maxLevel = maxLevel;
        this.producers = producers;
        
        foreach (Producer producer in this.producers) {
            producer.Initialize(board);
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
        levelText.text = $"{currentLevel}";
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
        
        OnDrag(eventData);
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
        if (!cellUnderItem.IsEmpty && !cellUnderItem.CheckItemCanBePlaced(this)) {
            ReturnToGridCell();
            return;
        }
        
        board.MoveItemOnBoard(this, cellUnderItem);
    }

    private void ReturnToGridCell()
    {
        transform.SetParent(GridCell.transform, false);
    }

    private void OnClick()
    { 
        foreach (Producer producer in producers.Where(p => p.Type == Producer.ProducerType.Active)) {
            producer.ProduceItem();
        }
    }
}
