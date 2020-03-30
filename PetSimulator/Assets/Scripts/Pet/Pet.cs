using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    public GameObject spotlight; //<! Activates when this pet is currently selected pet.
    MeshRenderer skin;

    //! Gets components and generates a random colour for pet.
    private void Start()
    {
        skin = GetComponentInChildren<MeshRenderer>();
        skin.material.color = Color.HSVToRGB(Random.Range(0.0f, 1.0f), 1, 1);
    }

    //! Toggles spotlight shown on pet.
    public void ToggleLight(bool state)
    {
        spotlight.SetActive(state);
    }
}