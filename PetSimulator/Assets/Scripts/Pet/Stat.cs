using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    public float MinValue { get; private set; } = 0;
    public float MaxValue { get; private set; } = 100;
    public float CurrentValue { get; set; }
    public float Timer { get; set; }

    public Stat() { CurrentValue = MaxValue; }
    public Stat(float value) { CurrentValue = value; }
}