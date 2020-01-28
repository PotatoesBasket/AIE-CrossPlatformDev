using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]

public class PetAI : MonoBehaviour
{
    public float minSwitchTime;
    public float maxSwitchTime;

    public float walkSpeed;
    public float runSpeed;

    enum States
    {
        IdleA,
        IdleB,
        Walking,
        Running
    }

    Animator petAnimator;
    Rigidbody petRigid;

    float timer = 0.0f;
    float currentSpeed = 0.0f;

    float nextSwitchTime = 0.0f;
    States nextState = States.IdleA;

    private void Start()
    {
        petAnimator = GetComponent<Animator>();
        petRigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > nextSwitchTime)
        {
            switch (nextState)
            {
                case States.IdleA:
                    currentSpeed = 0.0f;
                    petAnimator.SetBool("isWalking", false);
                    petAnimator.SetBool("isRunning", false);
                    break;

                case States.IdleB:
                    currentSpeed = 0.0f;
                    petAnimator.SetTrigger("idleB");
                    petAnimator.SetBool("isWalking", false);
                    petAnimator.SetBool("isRunning", false);
                    break;

                case States.Walking:
                    currentSpeed = walkSpeed;
                    petAnimator.SetBool("isWalking", true);
                    petAnimator.SetBool("isRunning", false);
                    break;

                case States.Running:
                    currentSpeed = runSpeed;
                    petAnimator.SetBool("isWalking", false);
                    petAnimator.SetBool("isRunning", true);
                    break;
            }

            timer = 0.0f;
            RollNewRandomValues();
        }

        petRigid.position += transform.forward * currentSpeed * Time.deltaTime;
    }

    //if AI is not currently idle, change direction,
    //determine next state and how long until it triggers
    void RollNewRandomValues()
    {
        if (nextState == States.Walking || nextState == States.Running)
        {
            transform.Rotate(0, Random.Range(0.0f, 360.0f), 0);
        }

        nextSwitchTime = Random.Range(minSwitchTime, maxSwitchTime);
        nextState = (States)Random.Range(0, 4);
    }
}