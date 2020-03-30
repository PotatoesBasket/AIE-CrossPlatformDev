using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditCenterOfMass : MonoBehaviour
{
    Rigidbody body;
    public Vector3 centerPoint;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        body.centerOfMass = centerPoint;
    }
}