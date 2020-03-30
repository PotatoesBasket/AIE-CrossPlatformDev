using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    GameManager manager;

    public Pet pet;
    public Material mat;

    public float minHatchTime; //!< Minimum time it can take for egg to hatch
    public float maxHatchTime; //!< Maximum time it can take for egg to hatch

    float timer = 0.0f; //!< Egg hatch timer.
    float hatchTime = 0.0f; //!< Time egg will take to hatch.

    //! Sets a random amount of time for egg to hatch within range.
    private void Start()
    {
        manager = GameManager.Instance;
        hatchTime = Random.Range(minHatchTime, maxHatchTime);
    }

    /*! Runs egg hatch timer. When time is reached, egg object is destroyed and pet is set active.*/
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > hatchTime)
        {
            pet.gameObject.transform.parent = null;
            pet.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}