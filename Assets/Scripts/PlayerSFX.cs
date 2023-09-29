using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;


public class PlayerSFX : MonoBehaviour
{
    [Header("Script References")]
    public ExampleCharacterController characterController;

    [Header("FMOD Settings")]
    [SerializeField] private FMODUnity.EventReference footstepsEventPath;
    [SerializeField] private FMODUnity.EventReference JumpEventPath;
    [SerializeField] private string materialParameterName;
    [SerializeField] private string speedParameterName;
    [SerializeField] private string jumpOrLandParameterName;

    [Header("Playback Settings")]
    [SerializeField] private float stepDistance = 2.0f;
    [SerializeField] private float runStepDistance = 5.0f;
    [SerializeField] private float rayDistance = 1.2f;
    //[SerializeField] private float startRunningTime = 0.3f;
    [SerializeField] private string jumpInputName;
    public string[] materialTypes;
    [HideInInspector] public int defaultMaterialValue = 0;

    [SerializeField] private bool isRunning = false;

    private float stepRandom; // random value for steps volume variation
    private Vector3 prevPos; // position in previous frame
    private float distanceTravelled; // distance travelled between this and last frame

    private RaycastHit raycastHit;
    [SerializeField] private int f_materialValue; // value of material FMOD parameter
    private int f_moveSpeed; // value of movespeed FMOD parameter
    private bool isTouchingGround;
    private bool prevTouchingGround;
    private float timeSinceLastStep;

    private Transform bodyTransform;

    private void Start()
    {
        stepRandom = Random.Range(0f, 0.2f);
        prevPos = transform.position;
        bodyTransform = characterController.gameObject.transform;
    }

    private void Update()
    {
        isRunning = characterController.isRunning;
        if (!isRunning)
            f_moveSpeed = 0;
        else if (isRunning)
        {
            if (f_moveSpeed == 0 && bodyTransform.position != prevPos)
            {
                f_moveSpeed = 1;
                PlayFootstep();
            } else
            {
                f_moveSpeed = 1;
            }
        }

        // FOOTSTEPS
        //timeSinceLastStep += Time.deltaTime; // keeps track of how long the game's been running (accounting for variable framerate)
        distanceTravelled += (bodyTransform.position - prevPos).magnitude; // gets distance player has travelled since last frame (for speed)
        // walk
        if (!isRunning && distanceTravelled >= stepDistance + stepRandom)
        {
            MaterialCheck();
            PlayFootstep();
            stepRandom = Random.Range(0f, 0.5f);
            distanceTravelled = 0f;
        }
        // run
        if (isRunning && distanceTravelled >= runStepDistance + stepRandom)
        {
            MaterialCheck();
            PlayFootstep();
            stepRandom = Random.Range(0f, 0.5f);
            distanceTravelled = 0f;
        }

        if (bodyTransform.position == prevPos)
            distanceTravelled = 0f;

        prevPos = bodyTransform.position;

        // JUMP & LAND
        isTouchingGround = characterController.Motor.GroundingStatus.FoundAnyGround;
        if (isTouchingGround && Input.GetButtonDown(jumpInputName)) // if touching ground & jump button pressed (just jumped)
        {
            MaterialCheck();
            PlayJumpOrLand(true);
        }
        if (!prevTouchingGround && isTouchingGround) // if started touching ground this frame (landed)
        {
            MaterialCheck();
            PlayJumpOrLand(false);
        }

        prevTouchingGround = isTouchingGround; // update prevTouchingGround to be 1 frame behind isTouchingGround

    }



    //void GroundedCheck()
    //{
    //    RaycastHit hit;
    //    Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance);
    //    if (hit.collider)
    //        isTouchingGround = true;
    //    else
    //        isTouchingGround = false;
    //}

    public void MaterialCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(bodyTransform.position + new Vector3(0f,0.5f,0f), Vector3.down, out hit, rayDistance)) // raycast to detect ground
        {
            if (hit.collider.gameObject.GetComponent<FmodMaterialSetter>()) // check for setter
            {
                // material found
                f_materialValue = hit.collider.gameObject.GetComponent<FmodMaterialSetter>().materialValue; // access setter value
            } else
            {
                // no material assigned
                f_materialValue = defaultMaterialValue;
            }
        } else
        {
            // nothing in raycast
            f_materialValue = defaultMaterialValue;
        }
        //Debug.Log(f_materialValue);
    }

    // plays jump or land SFX
    void PlayJumpOrLand(bool f_jumpOrLand)
    {
        FMOD.Studio.EventInstance jumpLand = FMODUnity.RuntimeManager.CreateInstance(JumpEventPath); // load jump event as variable
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(jumpLand, bodyTransform, GetComponent<Rigidbody>()); // attach jump event to body (location)
        jumpLand.setParameterByName(materialParameterName, f_materialValue); // set FMOD parameter to the material value
        jumpLand.setParameterByName(jumpOrLandParameterName, f_jumpOrLand ? 0f : 1f); // same as above. bool -> IF FALSE: 0, IF TRUE: 1
        jumpLand.start(); // plays instance
        jumpLand.release(); // destroys instance after finished playing

    }

    void PlayFootstep()
    {
        FMOD.Studio.EventInstance footstep = FMODUnity.RuntimeManager.CreateInstance(footstepsEventPath);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(footstep, bodyTransform, GetComponent<Rigidbody>());
        footstep.setParameterByName(materialParameterName, f_materialValue);
        footstep.setParameterByName(speedParameterName, f_moveSpeed);
        footstep.start();
        footstep.release();
    }
}
