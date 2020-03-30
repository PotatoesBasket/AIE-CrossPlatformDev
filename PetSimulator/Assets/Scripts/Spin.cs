using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speed;

    /*! Spins object continuously on its y-axis.*/
    private void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}