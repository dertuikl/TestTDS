using System;
using System.Runtime.Serialization;
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

    public void LoadSavedData(ProducerSaveData savedData)
    {
        Charges = savedData.Charges;
        TimeToCharge = savedData.TimeToCharge;
        
        if(Charges == 0) {
            activeProducerView?.DestroyView();
            activeProducerView = null;
        }
        
        UpdateView();
    }
    
    public void Tick()
    {
        TimeToCharge--;
        if (Type == ProducerType.Passive) {
            ProduceItem();
        } else {
            UpdateView();
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

    private void UpdateView()
    {
        activeProducerView?.SetValue(ChargeRefillTime - TimeToCharge);
    }

    public ProducerSaveData GetSaveData()
    {
        return new ProducerSaveData {
            Charges = Charges,
            TimeToCharge = TimeToCharge
        };
    }
}

[DataContract]
[Serializable]
public class ProducerSaveData
{
    [DataMember]
    public int Charges;
    [DataMember]
    public float TimeToCharge;
}