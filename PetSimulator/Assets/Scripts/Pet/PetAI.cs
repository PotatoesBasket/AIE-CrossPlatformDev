using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetAI : MonoBehaviour
{
    public float minSwitchTime; //!< Minimum possible time before next random action is chosen.
    public float maxSwitchTime; //!< Maximum possible time before next random action is chosen.

    public float walkSpeed;
    public float runSpeed;

    public LayerMask mask; //!< Allow for item hits only.

    enum States
    {
        Idle,
        Walking,
        Running
    }

    PetStatus stats;
    Animator petAnimator;
    Rigidbody petRigid;

    float timer = 0.0f;
    float currentSpeed = 0.0f;

    float nextSwitchTime = 0.0f; //!< Interval of time until next randomly picked action occurs.
    States nextState = States.Idle;

    Transform currentTarget; //!< Current targeted consumable.
    bool goingToConsumable = false; //!< If pet is currently travelling to a consumable or not.
    bool consuming = false; //!< If pet is currently consuming a consumable or not.

    public Transform spawnPoint; //!< Where gem-poop gets spawned.
    public GameObject gem; //!< Prefab for gem-poop.
    float poopTimer = 0.0f; //!< Timer for keeping track of when pet can poop.
    const float minPoopTime = 8; //!< Minimum time possible before next poop.
    const float maxPoopTime = 20; //!< Maximum time possible before next poop.
    float nextPoopTime = 10f; //!< Current interval between last and next poop.

    private void Start()
    {
        stats = GetComponent<PetStatus>();
        petAnimator = GetComponentInChildren<Animator>();
        petRigid = GetComponent<Rigidbody>();
    }

    /*! Updates pet's current action. */
    private void Update()
    {
        if (consuming)
        {
            Debug.Log("consume");
            Consume();
        }
        else if (goingToConsumable)
        {
            Debug.Log("go to item");
            GoToConsumable();
        }
        else
            NoActionAvailable();

        poopTimer += Time.deltaTime;
        if (poopTimer > nextPoopTime)
        {
            Instantiate(gem, spawnPoint.position, spawnPoint.rotation, null);
            nextPoopTime = Random.Range(minPoopTime, maxPoopTime);
            poopTimer = 0;
        }
    }

    //! Travel to consumable action.
    void GoToConsumable()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 2, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2, mask))
        {
            if (hit.transform.gameObject.CompareTag("Food") || hit.transform.gameObject.CompareTag("Water"))
            {
                goingToConsumable = false;
                consuming = true;
            }
        }
        else
        {
            petRigid.transform.LookAt(currentTarget);
            petRigid.position += petRigid.transform.forward * currentSpeed * Time.deltaTime;
        }
    }

    //! Eating action - fills stats, tells food it's being eaten.
    void Consume()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 2, Color.yellow);

        Food food = null;

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2, mask))
        {
            if (hit.transform.TryGetComponent(out food))
            {
                food.IsBeingConsumed = true;
                if (food.CompareTag("Food") && stats.Hunger.CurrentValue <= 99)
                {
                    stats.AddToStat(ref stats.Hunger, 100);
                }
                else if (food.CompareTag("Water") && stats.Thirst.CurrentValue <= 99)
                {
                    stats.AddToStat(ref stats.Thirst, 100);
                }
                else
                {
                    consuming = false;
                    food.IsBeingConsumed = false;
                }
            }
        }
        else
        {
            if (food != null)
                food.IsBeingConsumed = false;

            consuming = false;
        }
    }

    //! Default actions for when pet has nothing better to do, randomly chosen.
    void NoActionAvailable()
    {
        timer += Time.deltaTime;

        if (timer > nextSwitchTime)
        {
            switch (nextState)
            {
                case States.Idle:
                    currentSpeed = 0.0f;
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

        // move forward
        petRigid.position += petRigid.transform.forward * currentSpeed * Time.deltaTime;
    }

    //! Roll new values for random action state.
    void RollNewRandomValues()
    {
        if (nextState == States.Walking || nextState == States.Running)
        {
            petRigid.transform.Rotate(0, Random.Range(0.0f, 360.0f), 0);
        }

        nextSwitchTime = Random.Range(minSwitchTime, maxSwitchTime);
        nextState = (States)Random.Range(0, 3);
    }

    //! Check nearby items.
    private void OnTriggerStay(Collider other)
    {
        if (stats.Thirst.CurrentValue < 75 && other.transform.parent.TryGetComponent(out Food water))
        {
            Debug.Log("thirsty");
            if (water.CompareTag("Water"))
            {
                goingToConsumable = true;
                currentTarget = other.transform;
            }
        }
        else if (stats.Hunger.CurrentValue < 55 && other.transform.parent.TryGetComponent(out Food food))
        {
            Debug.Log("hungry");
            if (food.CompareTag("Food"))
            {
                goingToConsumable = true;
                currentTarget = other.transform;
            }
        }
        else
        {
            goingToConsumable = false;
            currentTarget = null;
        }
    }
}