using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    Pet pet;

    public Slider hungerBar;
    public Slider thirstBar;
    public Slider hygieneBar;
    public Slider funBar;

    private void Start()
    {
        pet = GameObject.FindGameObjectWithTag("Pet").GetComponent<Pet>();

        SetSliderValues(hungerBar, pet.Hunger);
        SetSliderValues(thirstBar, pet.Thirst);
        SetSliderValues(hygieneBar, pet.Hygiene);
        SetSliderValues(funBar, pet.Fun);
    }

    private void Update()
    {
        hungerBar.value = pet.Hunger.CurrentValue;
        thirstBar.value = pet.Thirst.CurrentValue;
        hygieneBar.value = pet.Hygiene.CurrentValue;
        funBar.value = pet.Fun.CurrentValue;
    }

    void SetSliderValues(Slider slider, Stat stat)
    {
        slider.minValue = stat.MinValue;
        slider.maxValue = stat.MaxValue;
    }
}