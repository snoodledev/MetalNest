using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PylonSFX : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference pulseEvent;
    [SerializeField][Range(0f,0.99f)] private float offset = 0f;
    private LightsourceFlicker lightsourceFlicker;
    private bool hasPulsed = false;

    void Start()
    {
        lightsourceFlicker = GetComponent<LightsourceFlicker>();
    }

    void Update()
    {
        if (lightsourceFlicker._pulsePosition > offset && !hasPulsed)
        {
            PlayPulse();
            hasPulsed = true;
        }
        else if (lightsourceFlicker._pulsePosition <= offset && hasPulsed)
        {
            hasPulsed = false;
        }
        //Debug.Log(hasPulsed);
    }

    void PlayPulse() 
    {
        FMOD.Studio.EventInstance pulse = FMODUnity.RuntimeManager.CreateInstance(pulseEvent);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(pulse, transform);
        pulse.start();
        pulse.release();
    }
}


