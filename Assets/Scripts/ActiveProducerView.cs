using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveProducerView : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    public void Initialize(float startValue, float maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = startValue;
    }

    public void SetValue(float newValue)
    {
        slider.value = newValue;
    }

    public void DestroyView()
    {
        Destroy(gameObject);
    }
}
