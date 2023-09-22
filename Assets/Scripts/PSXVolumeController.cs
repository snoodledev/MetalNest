using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PSXVolumeController : MonoBehaviour
{
    [HideInInspector] public VolumeProfile volumeProfile;

    [Header("Fog")]
    public float maxDistance;

    void Awake()
    {
        //UnityEngine.Rendering.VolumeProfile volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        //if (!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        volumeProfile = GetComponent<Volume>().profile;

        Debug.Log(volumeProfile);
    }

    void Update()
    {
        
    }
}
