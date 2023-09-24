using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using HauntedPSX.RenderPipelines.PSX.Runtime;

public class PSXVolumeController : MonoBehaviour
{
    public Volume v;
    private FogVolume f;

    [Header("Fog")]
    public float minDistance = 1f;
    public float maxDistance = 20f;
    [Range(-1f, 1f)] public float falloffCurve = 0.2f;
    public Color fogColor = new Color(0.29f, 0.29f, 0.29f);
    public bool lightningEnabled = false;

    void Awake()
    {
        if (v.profile.TryGet<FogVolume>(out FogVolume fogVolume))
        {
            f = fogVolume;
        }
    }

    void Update()
    {
        if (f == null) { Debug.Log("FogVolume is null"); return; }
        f.distanceMin.value = minDistance;
        f.distanceMax.value = maxDistance;
        f.color.value = fogColor;
        f.fogFalloffCurve.value = falloffCurve;
    }
}
