using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractButtons : MonoBehaviour
{
    PetManager petManager;
    PetStatus pet;

    public float foodValue;
    public float waterValue;
    public float hygieneValue;
    public float funValue;

    private void Start()
    {
        petManager = GameObject.FindGameObjectWithTag("PetManager").GetComponent<PetManager>();
    }

    private void Update()
    {
        pet = petManager.currentPet;
    }

    public void FeedPet()
    {
        if (pet != null)
            pet.AddToStat(ref pet.Hunger, foodValue);
    }

    public void WaterPet()
    {
        if (pet != null)
            pet.AddToStat(ref pet.Thirst, waterValue);
    }

    public void CleanPet()
    {
        if (pet != null)
            pet.AddToStat(ref pet.Hygiene, hygieneValue);
    }

    public void PlayWithPet()
    {
        if (pet != null)
            pet.AddToStat(ref pet.Fun, funValue);
    }
}