using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using HauntedPSX.RenderPipelines.PSX.Runtime;

public class EnvZoneController : MonoBehaviour
{
    public Volume volume;
    private FogVolume fogVolume;

    [Header("Fog")]
    public float minDistance = 1f;
    public float maxDistance = 20f;
    [Range(-1f, 1f)] public float falloffCurve = 0.2f;
    public Color fogColor = new Color(0.29f, 0.29f, 0.29f);
    public bool lightningEnabled = false;
    public Color fogLightningColor = new Color(0.29f, 0.29f, 0.29f);
    public float lerpSpeed = 0.05f;
    private float lightningChancePercent = 0.01f;

    private EnvZoneData envZoneData;

    private int lightningStep = -1;

    void Awake()
    {
        if (volume.profile.TryGet<FogVolume>(out FogVolume fogVolume))
        {
            this.fogVolume = fogVolume;
        }
    }

    private void Update()
    {
        if (fogVolume == null) { Debug.Log("FogVolume is null!"); return; }

        fogVolume.distanceMin.value = Mathf.Lerp(fogVolume.distanceMin.value, minDistance, 1f/lerpSpeed);
        fogVolume.distanceMax.value = Mathf.Lerp(fogVolume.distanceMax.value, maxDistance, 1f/lerpSpeed);
        fogVolume.color.value = Color.Lerp(fogVolume.color.value, fogColor, 1f/lerpSpeed);
        fogVolume.fogFalloffCurve.value = Mathf.Lerp(fogVolume.fogFalloffCurve.value, falloffCurve, 1f / lerpSpeed);

        if (lightningEnabled)
        {
            if (Random.Range(0f, 100f) >= 100 - lightningChancePercent)
            {
                if (lightningStep == -1 || lightningStep >= 5)
                {
                    lightningStep = 0;
                }

                switch (lightningStep)
                {
                    case 0:
                        fogVolume.color.value = Color.Lerp(fogColor, fogLightningColor, Random.Range(0.2f, 0.5f));
                        lightningStep++;
                        break;
                    case 1:
                        fogVolume.color.value = Color.Lerp(fogColor, fogLightningColor, Random.Range(0.6f, 0.9f));
                        lightningStep++;
                        break;
                    case 2:
                        fogVolume.color.value = Color.Lerp(fogColor, fogLightningColor, Random.Range(0.2f, 0.5f));
                        lightningStep++;
                        break;
                    case 3:
                        fogVolume.color.value = Color.Lerp(fogColor, fogLightningColor, Random.Range(0.2f, 0.5f));
                        lightningStep++;
                        break;
                    case 4:
                        fogVolume.color.value = fogLightningColor;
                        lightningStep++;
                        break;
                    default: break;
                }

                fogVolume.color.value = fogLightningColor;

            }
        }
    }



    private void OnTriggerEnter(Collider trigger)
    {
        envZoneData = trigger.gameObject.GetComponent<EnvZoneData>();
        if (envZoneData == null) { /*Debug.Log("No EnvZoneData found in collider");*/ return; }

        if (envZoneData.fogEnabled)
        {
            minDistance = envZoneData.fogDistanceMin;
            maxDistance = envZoneData.fogDistanceMax;
            falloffCurve = envZoneData.falloffCurve;
            fogColor = envZoneData.fogColor;
            lightningEnabled = envZoneData.lightningEnabled;
            fogLightningColor = envZoneData.fogLightningColor;
            lerpSpeed = envZoneData.fogLerpSpeed;
        }
    }
}
