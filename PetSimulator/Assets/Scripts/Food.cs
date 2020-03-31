using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public bool IsBeingConsumed { get; set; } //!< Whether or not food is currently being eaten, set by other entities

    public float consumeSpeed; //!< How fast food gets consumed.

    float currentScale; //!< Current overall scale of object.

    /*! If food is currently being consumed by something, shrink it until it reaches a minimum size,
     then destroy it. ...Well, that was supposed to be what happened, but it doesn't :V*/
    private void Update()
    {
        if (IsBeingConsumed)
        {
            currentScale -= consumeSpeed * Time.deltaTime;
            transform.localScale = Vector3.one * currentScale;
        }

        if (transform.localScale.x < 0.2f)
            Destroy(gameObject);
    }
}