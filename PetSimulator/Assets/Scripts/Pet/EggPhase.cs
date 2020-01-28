using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggPhase : MonoBehaviour
{
    public GameObject pet;
    public float minHatchTime;
    public float maxHatchTime;

    float timer = 0.0f;
    float hatchTime = 0.0f;

    private void Start()
    {
        hatchTime = Random.Range(minHatchTime, maxHatchTime);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > hatchTime)
        {
            Instantiate(pet, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}