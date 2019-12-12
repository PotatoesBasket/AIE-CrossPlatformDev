using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    string Name { get; set; }

    //health stats
    public Stat Age = new Stat();
    public Stat Hunger = new Stat();
    public Stat Thirst = new Stat();
    public Stat Hygiene = new Stat();
    public Stat Energy = new Stat();
    public Stat Fun = new Stat();

    //skill stats
    public Stat Love = new Stat();
    public Stat Tameness = new Stat();
    public Stat Athleticism = new Stat();
    public Stat Musical = new Stat();
    public Stat Intelligence = new Stat();
    public Stat Artistry = new Stat();
    public Stat Empathy = new Stat();

    public void AlterStat(ref Stat stat, float amount)
    {
        if (stat.CurrentValue + amount > stat.MaxValue)
            stat.CurrentValue = stat.MaxValue;
        else if (stat.CurrentValue + amount < stat.MinValue)
            stat.CurrentValue = stat.MinValue;
        else
            stat.CurrentValue += amount;
    }

    private void Update()
    {
        NaturalShift(ref Age, 60, 1);
        NaturalShift(ref Hunger, 5, -3);
        NaturalShift(ref Thirst, 3, -3);
        NaturalShift(ref Hygiene, 10, -4);
        NaturalShift(ref Energy, 7, -1);
        NaturalShift(ref Fun, 10, -5);
    }

    /* Ajusts stat by amount when timer hits secInterval value */
    void NaturalShift(ref Stat stat, float secInterval, float amount)
    {
        stat.Timer += Time.deltaTime;

        if (stat.Timer >= secInterval)
        {
            if (stat.CurrentValue + amount > stat.MaxValue)
                stat.CurrentValue = stat.MaxValue;
            else if (stat.CurrentValue + amount < stat.MinValue)
                stat.CurrentValue = stat.MinValue;
            else
                stat.CurrentValue += amount;

            stat.Timer = 0;
        }
    }
}