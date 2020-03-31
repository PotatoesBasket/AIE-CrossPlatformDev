using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamControl : MonoBehaviour
{
    GameManager manager;

    public CharacterController focusPoint; //!< Object camera follows to pan, zoom, and orbit horizontally.
    public Transform rotatePoint; //!< Object camera follows to orbit vertically.
    public LayerMask mask;

    public float minVerticalOrbit; //!< Minimum up/down orbit amount.
    public float maxVerticalOrbit; //!< Maximum up/down orbit amount.

    Vector3 totalMovement; //!< Overall accumulated movement per frame.

    private void Start()
    {
        InitComponents();
        InitCameraPosition();
    }

    private void Update()
    {
        totalMovement = Vector3.zero;

        DetectTouch();

        Pan(); //move camera left/right and up/down
        Zoom(); //move camera forward/back
        Orbit(); //set focus point rotation

        LookForFocusTarget();
        MoveToFocusObject();
        FollowFocusObject();

        if (!isFocusing || !arrivedAtTarget)
            focusPoint.Move(totalMovement);
    }

    //! Gets components needed for script
    void InitComponents()
    {
        manager = GameManager.Instance;
    }

    //! Initializes orbit values to match editor
    void InitCameraPosition()
    {
        totalOrbitX = focusPoint.transform.eulerAngles.y;
        totalOrbitY = rotatePoint.eulerAngles.x + -360;
    }

    Touch touch1;
    Touch touch2;

    bool isTapping = false;
    bool isDragging = false;
    bool isPinching = false;

    float tapTimer = 0.0f;
    const float timeForDragDetect = 0.08f;
    const float sqrDistForPinchDetect = 1500.0f;

    Vector3 tapPosLastFrame;
    Vector3 tapPos;
    RaycastHit tapRayHit;

    void DetectTouch()
    {
        if (Input.touchCount >= 1)
            touch1 = Input.GetTouch(0);

        if (Input.touchCount >= 2)
            touch2 = Input.GetTouch(1);

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(tapPos), out tapRayHit, 300, mask))
            {
                if (tapRayHit.transform.CompareTag("Money"))
                {
                    Destroy(tapRayHit.rigidbody.gameObject);
                    manager.money += 100;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                isTapping = true;
            }

            if (Input.GetMouseButton(0))
            {
                tapTimer += Time.deltaTime;
                tapPosLastFrame = tapPos;
                tapPos = Input.mousePosition;

                if (tapTimer > timeForDragDetect)
                {
                    isTapping = false;
                    isDragging = true;
                }
            }

            if (Input.GetMouseButtonUp(0) || touch1.phase == TouchPhase.Ended)
            {
                isTapping = false;
                isDragging = false;
                isPinching = false;
                tapTimer = 0.0f;
            }

            // detect pinch gesture
            if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
                Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

                float prevMagnitudeSqr = (touch1PrevPos - touch2PrevPos).sqrMagnitude;
                float currentMagnitudeSqr = (touch1.position - touch2.position).sqrMagnitude;

                if (Mathf.Abs(currentMagnitudeSqr - prevMagnitudeSqr) > sqrDistForPinchDetect)
                    isPinching = true;
                else
                    isPinching = false;
            }
        }
    }

    #region CAMERA USER CONTROLS

    ///////////////////////////
    // CAMERA USER CONTROLS  //
    ///////////////////////////

    public float panSpeed;
    Vector3 horizontalPan;
    Vector3 verticalPan;

    void Pan()
    {
        // get all pan input methods
        float inputX = Mathf.Clamp(Input.GetAxis("PanHorizontal"), -1, 1);
        float inputY = Mathf.Clamp(Input.GetAxis("PanVertical"), -1, 1);

        // release from focus object
        if ((inputX != 0 || inputY != 0) && isFocusing == true)
            isFocusing = false;

        // calculate movement
        horizontalPan = -focusPoint.transform.right * inputX * panSpeed * Time.deltaTime;
        verticalPan = focusPoint.transform.up * inputY * panSpeed * Time.deltaTime;

        // touch pan
        if (isDragging == true && isPinching == false && touch2.phase == TouchPhase.Moved)
        {
            Vector3 direction = (tapPosLastFrame - tapPos).normalized;

            //totalMovement += direction;
            horizontalPan = -focusPoint.transform.right * direction.x * panSpeed * Time.deltaTime;
            verticalPan = focusPoint.transform.up * direction.y * panSpeed * Time.deltaTime;
        }

        // add to total movement
        totalMovement += horizontalPan + verticalPan;
    }

    public float zoomSpeed;
    Vector3 zoom;

    void Zoom()
    {
        float input;

        // get keyboard/gamepad zoom input methods
        input = Input.GetAxis("Zoom");

        // pinch gesture zoom input
        if (isPinching && touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
        {
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

            float prevMagnitude = (touch1PrevPos - touch2PrevPos).magnitude;
            float currentMagnitude = (touch1.position - touch2.position).magnitude;

            input = -currentMagnitude - -prevMagnitude;
        }

        // release from focus object
        if ((input != 0 || Input.touchCount == 2) && isFocusing == true)
            isFocusing = false;

        // calculate movement
        zoom = focusPoint.transform.forward * input * panSpeed * Time.deltaTime;

        // add to total movement
        totalMovement += zoom;
    }

    public float orbitSpeed;
    float totalOrbitX = 0.0f;
    float totalOrbitY = 0.0f;

    void Orbit()
    {
        // get orbit inputs
        totalOrbitX += Input.GetAxis("OrbitHorizontal") * orbitSpeed * Time.deltaTime; // buttons/keys
        totalOrbitY += Input.GetAxis("OrbitVertical") * orbitSpeed * Time.deltaTime;

        if (isDragging && Input.touchCount != 2) // touch
        {
            Vector3 direction = (tapPosLastFrame - tapPos).normalized;

            totalOrbitX += direction.x * orbitSpeed * Time.deltaTime;
            totalOrbitY += direction.y * orbitSpeed * Time.deltaTime;
        }

        // rollover total horizontal orbit
        if (totalOrbitX > 360)
            totalOrbitX = 0;
        if (totalOrbitX < 0)
            totalOrbitX = 360;

        // clamp total vertical orbit
        totalOrbitY = Mathf.Clamp(totalOrbitY, minVerticalOrbit, maxVerticalOrbit);

        // set new focus point rotation
        // it's seperate to not mess with panning and i'm lazy :V
        focusPoint.transform.rotation = Quaternion.Euler(0, totalOrbitX, 0);
        rotatePoint.localRotation = Quaternion.Euler(totalOrbitY, 0, 0);
    }

    #endregion

    #region CAMERA FOCUS

    ////////////////////
    //  CAMERA FOCUS  //
    ////////////////////

    public float focusSpeed;
    public Vector3 focusHeightOffset;

    Transform focusObject = null;
    bool isFocusing = false;
    bool arrivedAtTarget = true;

    Pet pet = null;

    // locks camera onto target at tap position if available
    void LookForFocusTarget()
    {
        if (isTapping)
        {
            //tapped on valid focus object, lock on camera and open UI
            if (tapRayHit.rigidbody != null && tapRayHit.rigidbody.CompareTag("Pet"))
            {
                focusObject = tapRayHit.rigidbody.transform;
                isFocusing = true;
                arrivedAtTarget = false;

                if (focusObject.TryGetComponent(out pet))
                {
                    manager.SetCurrentPet(pet);
                }
            }
            //unfocus and close UI if not currently clicking on UI
            else if (!EventSystem.current.IsPointerOverGameObject())
            {
                focusObject = null;
                isFocusing = false;
                arrivedAtTarget = true;
                manager.UnselectCurrentPet();
                pet = null;
            }
        }
    }

    //moves camera to focus object if not already on it
    void MoveToFocusObject()
    {
        if (focusObject != null && arrivedAtTarget == false)
        {
            if (Vector3.Distance(focusObject.position + focusHeightOffset, focusPoint.transform.position) > 1.0f)
            {
                Vector3 direction = ((focusObject.position + focusHeightOffset) - focusPoint.transform.position).normalized;
                totalMovement += direction * focusSpeed * Time.deltaTime;
            }
            else
                arrivedAtTarget = true;
        }
    }

    // camera follows focus object if already on it
    void FollowFocusObject()
    {
        if (focusObject != null && isFocusing && arrivedAtTarget)
        {
            focusPoint.transform.position = focusObject.position + focusHeightOffset;
        }
    }

    #endregion
}