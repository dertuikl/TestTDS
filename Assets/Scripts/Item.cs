using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
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
    }

    [SerializeField]
    private List<Producer> producers;
    [SerializeField]
    private int currentLevel = 0;
    [SerializeField]
    private int maxLevel = 6;
    [SerializeField]
    private Text levelText;

    public void Initialize(int startLevel, int maxLevel, List<Producer> producers)
    {
        this.currentLevel = startLevel;
        this.maxLevel = maxLevel;
        this.producers = producers;

        UpdateView();
    }

    private void UpdateView()
    {
        levelText.text = $"{currentLevel}";
    }
}
