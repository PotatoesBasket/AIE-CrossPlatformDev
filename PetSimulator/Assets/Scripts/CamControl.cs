using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamControl : MonoBehaviour
{
    public CharacterController focusPoint; //object camera follows to pan, zoom, and orbit horizontally
    public Transform rotatePoint; //object camera follows to orbit vertically

    PetManager petManager;
    Vector3 totalMovement;

    private void Start()
    {
        petManager = GameObject.FindGameObjectWithTag("PetManager").GetComponent<PetManager>();
        InitialiseCameraPosition();
    }

    private void Update()
    {
        totalMovement = Vector3.zero;

        Pan(); //move camera left/right and up/down
        Zoom(); //move camera forward/back
        Orbit(); //set focus point rotation

        LookForFocusTarget();
        MoveToFocusObject();
        FollowFocusObject();

        if (!isFocusing || !arrivedAtTarget)
            focusPoint.Move(totalMovement);

        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
    }

    #region CAMERA USER CONTROLS

    ///////////////////////////
    // CAMERA USER CONTROLS  //
    ///////////////////////////

    public float panSpeed;
    public float zoomSpeed;
    public float orbitSpeed;

    Vector3 horizontalPan;
    Vector3 verticalPan;
    Vector3 zoom;

    float totalOrbitX = 0;
    float totalOrbitY = 0;

    void InitialiseCameraPosition()
    {
        //initialize orbit values to match editor
        totalOrbitX = focusPoint.transform.eulerAngles.y;
        totalOrbitY = rotatePoint.eulerAngles.x + -360;
    }

    void Pan()
    {
        //get all pan input methods
        float inputX = Mathf.Clamp(Input.GetAxis("PanHorizontal"), -1, 1);
        float inputY = Mathf.Clamp(Input.GetAxis("PanVertical"), -1, 1);

        //release from focus object
        if ((inputX != 0 || inputY != 0) && isFocusing == true)
            isFocusing = false;

        //calculate movement
        horizontalPan = -focusPoint.transform.right * inputX * panSpeed * Time.deltaTime;
        verticalPan = focusPoint.transform.up * inputY * panSpeed * Time.deltaTime;

        //add to total movement
        totalMovement += horizontalPan + verticalPan;
    }

    void Zoom()
    {
        //get all zoom input methods
        float input = Mathf.Clamp(
            Input.GetAxis("Zoom") +
            Input.GetAxis("ScrollWheel"), -1, 1);

        //release from focus object
        if (input != 0 && isFocusing == true)
            isFocusing = false;

        //calculate movement
        zoom = focusPoint.transform.forward * input * panSpeed * Time.deltaTime;

        //add to total movement
        totalMovement += zoom;
    }

    void Orbit()
    {
        //get orbit inputs
        totalOrbitX += Input.GetAxis("OrbitHorizontal") * orbitSpeed * Time.deltaTime; // buttons/keys
        totalOrbitY += Input.GetAxis("OrbitVertical") * orbitSpeed * Time.deltaTime;

        //rollover total horizontal orbit
        if (totalOrbitX > 360)
            totalOrbitX = 0;
        if (totalOrbitX < 0)
            totalOrbitX = 360;

        //clamp total vertical orbit
        totalOrbitY = Mathf.Clamp(totalOrbitY, -25, 7);

        //set new focus point rotation
        //it's seperate to not mess with panning and i'm lazy :V
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

    float mouseTimer = 0.0f;
    const float tapTimeLimit = 0.3f;
    bool isTapping = false;
    Vector3 tapPos = Vector3.zero;

    Transform focusObject = null;
    bool isFocusing = false;
    bool arrivedAtTarget = true;

    public StatUI UIPanel;

    //detects if mouse/pressure input was a "tap"
    //if so, locks camera onto target at tap position if available
    void LookForFocusTarget()
    {
        if (Input.GetMouseButton(0))
        {
            isTapping = true;
            mouseTimer += Time.deltaTime;
            tapPos = Input.mousePosition;

            if (mouseTimer > tapTimeLimit)
            {
                isTapping = false;
            }
        }
        else if (isTapping == true)
        {
            if (Input.GetMouseButtonUp(0))
            {
                isTapping = false;
                mouseTimer = 0.0f;

                Physics.Raycast(Camera.main.ScreenPointToRay(tapPos), out RaycastHit hit);

                //tapped on valid focus object, lock on camera and open UI
                if (hit.rigidbody != null && hit.rigidbody.CompareTag("Pet"))
                {
                    focusObject = hit.rigidbody.transform;
                    isFocusing = true;
                    arrivedAtTarget = false;

                    if (focusObject.TryGetComponent(out PetStatus pet))
                    {
                        petManager.currentPet = pet;
                        UIPanel.SetPet(pet);
                        UIPanel.gameObject.SetActive(true);
                    }
                }
                //unfocus and close UI if not currently clicking on UI
                else if (!EventSystem.current.IsPointerOverGameObject())
                {
                    focusObject = null;
                    isFocusing = false;
                    arrivedAtTarget = true;
                    petManager.currentPet = null;
                    UIPanel.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            mouseTimer = 0.0f;
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

    //camera follows focus object if already on it
    void FollowFocusObject()
    {
        if (focusObject != null && isFocusing && arrivedAtTarget)
        {
            focusPoint.transform.position = focusObject.position + focusHeightOffset;
        }
    }

    #endregion
}