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
        InitializeSlider();
    }

    private void InitializeSlider()

    {
        SetMinSlider(0);
        SetMaxValue(20);
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
            Debug.Log("Coucou");
            sliderExp.maxValue = sliderExp.maxValue + stepForExp;
            sliderExp.value = 0;
            LevelManager.instance.GainLevel();
            SetMinSlider(0);
        }
    }

    
}
