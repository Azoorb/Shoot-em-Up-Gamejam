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
    int stepForExp;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
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
        sliderExp.value += value;
        if(sliderExp.value >= sliderExp.maxValue)
        {
            sliderExp.maxValue = sliderExp.maxValue + stepForExp;
            LevelManager.instance.GainLevel();
            SetMinSlider(0);
        }
    }

    
}
