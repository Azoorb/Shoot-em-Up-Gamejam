using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    public static SliderManager instance;
    [SerializeField]
    Slider sliderExp, sliderHealthPlayer;
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
        sliderExp.minValue = 0;
        sliderExp.maxValue = startValueExp;
    }

    public void InitializeSliderPlayer()
    {
        sliderHealthPlayer.minValue = 0;
        sliderHealthPlayer.maxValue = PlayerScript.instance.hp;
        sliderHealthPlayer.value = PlayerScript.instance.hp;
    }

    public void TakeDamage(int damage)
    {
        sliderHealthPlayer.value -= damage;
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
            sliderExp.minValue = 0;
        }
    }

    
}
