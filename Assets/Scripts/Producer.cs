using System;
using UnityEngine;

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
        if (Type == ProducerType.Passive) {
            ProduceItem();
        } else {
            Debug.Log(TimeToCharge);
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