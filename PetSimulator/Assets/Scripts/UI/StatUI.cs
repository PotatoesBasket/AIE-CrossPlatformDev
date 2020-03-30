using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    public PetStatus currentPet;

    public Slider hungerBar;
    public Slider thirstBar;
    public Slider loveBar;

    public void SetPet(PetStatus pet) { currentPet = pet; }

    //! Inits stat bars in HUD to be beginning values.
    private void Start()
    {
        currentPet = GameManager.Instance.currentPet.GetComponent<PetStatus>();

        SetSliderValues(hungerBar, currentPet.Hunger);
        SetSliderValues(thirstBar, currentPet.Thirst);
        SetSliderValues(loveBar, currentPet.Love);
    }

    //! Updates HUD stat bars.
    private void Update()
    {
        hungerBar.value = currentPet.Hunger.CurrentValue;
        thirstBar.value = currentPet.Thirst.CurrentValue;
        loveBar.value = currentPet.Love.CurrentValue;
    }

    //! Sets single stat bar in HUD to beginning values.
    void SetSliderValues(Slider slider, Stat stat)
    {
        slider.minValue = stat.MinValue;
        slider.maxValue = stat.MaxValue;
    }
}