using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetStatus : MonoBehaviour
{
    GameManager manager;

    //health stats
    public Stat Hunger = new Stat();
    public Stat Thirst = new Stat();
    public Stat Love = new Stat(0);

    private void Start()
    {
        manager = GameManager.Instance;
    }

    private void Update()
    {
        NaturalShift(ref Hunger, 5, -3);
        NaturalShift(ref Thirst, 3, -3);
        NaturalShift(ref Love, 10, 5);
        Die();
    }

    //! Adds amount to stat without going outside of min/max range.
    public void AddToStat(ref Stat stat, float amount)
    {
        if (stat.CurrentValue + amount >= stat.MaxValue)
            stat.CurrentValue = stat.MaxValue;
        else if (stat.CurrentValue + amount <= stat.MinValue)
            stat.CurrentValue = stat.MinValue;
        else
            stat.CurrentValue += amount;
    }

    /*! Adjusts stat by amount when timer hits secInterval value. */
    void NaturalShift(ref Stat stat, float secInterval, float amount)
    {
        stat.Timer += Time.deltaTime;

        if (stat.Timer >= secInterval)
        {
            AddToStat(ref stat, amount);
            stat.Timer = 0;
        }
    }

    //! Checks if pet should be dead, destroys object and decrements total number of pets if yes.
    void Die()
    {
        if (Hunger.CurrentValue <= Hunger.MinValue ||
            Thirst.CurrentValue <= Thirst.MinValue)
        {
            --manager.noOfPets;
            Destroy(gameObject);
        }
    }
}