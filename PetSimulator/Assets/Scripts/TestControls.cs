using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestControls : MonoBehaviour
{
    public GameObject eggPrefab;
    public Transform focusPoint;

    public void SpawnEgg()
    {
        Instantiate(eggPrefab, focusPoint.position, focusPoint.rotation);
    }
}