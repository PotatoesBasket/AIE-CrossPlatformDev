using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    PetStatus currentPet;

    public Slider hungerBar;
    public Slider thirstBar;
    public Slider hygieneBar;
    public Slider funBar;

    public void SetPet(PetStatus pet) { currentPet = pet; }

    private void Start()
    {
        currentPet = GameObject.FindGameObjectWithTag("Pet").GetComponent<PetStatus>();

        SetSliderValues(hungerBar, currentPet.Hunger);
        SetSliderValues(thirstBar, currentPet.Thirst);
        SetSliderValues(hygieneBar, currentPet.Hygiene);
        SetSliderValues(funBar, currentPet.Fun);
    }

    private void Update()
    {
        hungerBar.value = currentPet.Hunger.CurrentValue;
        thirstBar.value = currentPet.Thirst.CurrentValue;
        hygieneBar.value = currentPet.Hygiene.CurrentValue;
        funBar.value = currentPet.Fun.CurrentValue;
    }

    void SetSliderValues(Slider slider, Stat stat)
    {
        slider.minValue = stat.MinValue;
        slider.maxValue = stat.MaxValue;
    }
}