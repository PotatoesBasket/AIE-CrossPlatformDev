using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactions : MonoBehaviour
{
    Pet pet;

    public float foodValue;
    public float waterValue;
    public float hygieneValue;
    public float funValue;

    private void Start()
    {
        pet = GameObject.FindGameObjectWithTag("Pet").GetComponent<Pet>();
    }

    public void FeedPet()
    {
        pet.AlterStat(ref pet.Hunger, foodValue);
    }

    public void WaterPet()
    {
        pet.AlterStat(ref pet.Thirst, waterValue);
    }

    public void CleanPet()
    {
        pet.AlterStat(ref pet.Hygiene, hygieneValue);
    }

    public void PlayWithPet()
    {
        pet.AlterStat(ref pet.Fun, funValue);
    }
}