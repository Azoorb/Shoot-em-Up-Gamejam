using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    public static SliderManager instance;
    [SerializeField]
    Slider sliderExp;
    [SerializeField]
    int stepForExp,startValueExp;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        InitializeSlider();
    }

    private void InitializeSlider()

    {
        SetMinSlider(0);
        SetMaxValue(startValueExp);
    }
    private void SetMinSlider(int value)
    {
        sliderExp.minValue = value;
    }

    private void SetMaxValue(int value)
    {
        sliderExp.maxValue = value;
    }

    public void GainExp(int value)
    {
        Debug.Log("gainExp" + value);
        sliderExp.value += value;
        if(sliderExp.value >= sliderExp.maxValue)
        {
            
            float oldValue = sliderExp.maxValue;
            sliderExp.maxValue = oldValue + stepForExp;
            sliderExp.value = 0;
            LevelManager.instance.GainLevel();
            SetMinSlider(0);
        }
    }

    
}
