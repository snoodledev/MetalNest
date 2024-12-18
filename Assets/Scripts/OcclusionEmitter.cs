using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class OcclusionEmitter : MonoBehaviour
{
    [Header("FMOD Event")]
    [SerializeField]
    public EventReference Event;
    private EventInstance Audio;
    private EventDescription AudioDes;
    [SerializeField] private StudioListener Listener;
    private PLAYBACK_STATE pb;

    [Header("Occlusion Options")]
    [SerializeField]
    [Range(0f, 10f)]
    private float SoundOcclusionWidening = 1f;
    [SerializeField]
    [Range(0f, 10f)]
    private float PlayerOcclusionWidening = 1f;
    [SerializeField]
    private LayerMask OcclusionLayer;

    private float f_Occlusion = 1f;
    private float occlusionLerp = 1f;
    [SerializeField] private float fadeSpeed = 0.3f;
    private bool AudioIsVirtual;
    private float ListenerDistance;
    private float lineCastHitCount = 0f;
    private Color colour;
    /*[SerializeField] */private float MaxDistance = 20;
    [SerializeField] [Range(0f, 1.2f)] private float volume = 0f;
    [SerializeField][Range(0f, 3f)] private float pitch = 0f;
    //[SerializeField] private float MinDistance = 1;

    [SerializeField] private bool persistentDebug = false;

    //public bool dopplerEnabled = true;
    //private Vector3 objPosLastFrame = Vector3.zero;
    //private Vector3 objVelocity = Vector3.zero; 
    //private Vector3 playerPosLastFrame = Vector3.zero;
    //private Vector3 playerVelocity = Vector3.zero;

    void OnDrawGizmosSelected()
    {
        if (!persistentDebug)
        {
            Gizmos.color = Color.yellow;
            //Gizmos.DrawWireSphere(transform.position, MinDistance);
            Gizmos.DrawWireSphere(transform.position, MaxDistance);
        }
    }
    void OnDrawGizmos()
    {
        if (persistentDebug)
        {
            Gizmos.color = Color.yellow;
            //Gizmos.DrawWireSphere(transform.position, MinDistance);
            Gizmos.DrawWireSphere(transform.position, MaxDistance);
        }
    }

    private void Start()
    {
        Audio = RuntimeManager.CreateInstance(Event);
        RuntimeManager.AttachInstanceToGameObject(Audio, GetComponent<Transform>(), GetComponent<Rigidbody>());
        //Audio.setProperty(EVENT_PROPERTY.MAXIMUM_DISTANCE, MaxDistance);
        if (volume != 0f)
            Audio.setVolume(volume);
        if (pitch != 0f)
            Audio.setPitch(pitch);
        Audio.start();
        Audio.release();

        AudioDes = RuntimeManager.GetEventDescription(Event);
        //AudioDes.getMinMaxDistance(out float _, out float MaxDistanceTemp);
        //MaxDistance = MaxDistanceTemp;
        Listener = FindObjectOfType<StudioListener>();
    }

    private void FixedUpdate()
    {
        Audio.isVirtual(out AudioIsVirtual); 
        Audio.getPlaybackState(out pb); 
        ListenerDistance = Vector3.Distance(transform.position, Listener.transform.position);

        if (!AudioIsVirtual && pb == PLAYBACK_STATE.PLAYING && ListenerDistance <= MaxDistance)
        {
            OccludeBetween(transform.position, Listener.transform.position);
        }

        lineCastHitCount = 0f;
    }

    private void OnDestroy()
    {
        Audio.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        Audio.release();
    }

    private void OccludeBetween(Vector3 sound, Vector3 listener)
    {
        Vector3 SoundLeft = CalculatePoint(sound, listener, SoundOcclusionWidening, true);
        Vector3 SoundRight = CalculatePoint(sound, listener, SoundOcclusionWidening, false);

        Vector3 SoundAbove = new Vector3(sound.x, sound.y + SoundOcclusionWidening, sound.z);
        Vector3 SoundBelow = new Vector3(sound.x, sound.y - SoundOcclusionWidening, sound.z);

        Vector3 ListenerLeft = CalculatePoint(listener, sound, PlayerOcclusionWidening, true);
        Vector3 ListenerRight = CalculatePoint(listener, sound, PlayerOcclusionWidening, false);

        Vector3 ListenerAbove = new Vector3(listener.x, listener.y + PlayerOcclusionWidening * 0.5f, listener.z);
        Vector3 ListenerBelow = new Vector3(listener.x, listener.y - PlayerOcclusionWidening * 0.5f, listener.z);

        CastLine(SoundLeft, ListenerLeft);
        CastLine(SoundLeft, listener);
        CastLine(SoundLeft, ListenerRight);

        CastLine(sound, ListenerLeft);
        CastLine(sound, listener);
        CastLine(sound, ListenerRight);

        CastLine(SoundRight, ListenerLeft);
        CastLine(SoundRight, listener);
        CastLine(SoundRight, ListenerRight);

        CastLine(SoundAbove, ListenerAbove);
        CastLine(SoundBelow, ListenerBelow);

        if (PlayerOcclusionWidening == 0f || SoundOcclusionWidening == 0f)
        {
            colour = Color.blue;
        }
        else
        {
            colour = Color.green;
        }

        SetParameter();
    }

    private Vector3 CalculatePoint(Vector3 a, Vector3 b, float m, bool posOrneg)
    {
        float x;
        float z;
        float n = Vector3.Distance(new Vector3(a.x, 0f, a.z), new Vector3(b.x, 0f, b.z));
        float mn = (m / n);
        if (posOrneg)
        {
            x = a.x + (mn * (a.z - b.z));
            z = a.z - (mn * (a.x - b.x));
        }
        else
        {
            x = a.x - (mn * (a.z - b.z));
            z = a.z + (mn * (a.x - b.x));
        }
        return new Vector3(x, a.y, z);
    }

    private void CastLine(Vector3 Start, Vector3 End)
    {
        RaycastHit hit;
        Physics.Linecast(Start, End, out hit, OcclusionLayer);

        if (hit.collider)
        {
            lineCastHitCount++;
            Debug.DrawLine(Start, End, Color.red);
        }
        else
            Debug.DrawLine(Start, End, colour);
    }


    private void SetParameter()
    {
        // 58:20 video explanation
        Audio.getParameterByName("Occlusion", out f_Occlusion);
        occlusionLerp = Mathf.Lerp(f_Occlusion, lineCastHitCount / 11, fadeSpeed);
        Audio.setParameterByName("Occlusion", occlusionLerp);

        //Debug.Log(occlusionLerp);
    }
}