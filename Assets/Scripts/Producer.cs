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
    private ActiveProducerView activeProducerView;

    public void Initialize(Board board, ActiveProducerView activeProducerView = null)
    {
        this.board = board;
        this.activeProducerView = activeProducerView;
        Charges = ChargesMax;
        TimeToCharge = ChargeRefillTime;

        this.activeProducerView?.Initialize(ChargeRefillTime - TimeToCharge, ChargeRefillTime);
    }

    public void Tick()
    {
        TimeToCharge--;
        if (Type == ProducerType.Passive) {
            ProduceItem();
        } else {
            activeProducerView?.SetValue(ChargeRefillTime - TimeToCharge);
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
                
                if(Charges == 0) {
                    activeProducerView.DestroyView();
                    activeProducerView = null;
                }
            }
        }
    }
}