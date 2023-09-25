using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvZoneData : MonoBehaviour
{
    [Header("Main")]
    public bool fogEnabled = true;
    public float fogDistanceMin = 1f;
    public float fogDistanceMax = 20f;
    [Range(-1f, 1f)] public float falloffCurve = 0.2f;
    public Color fogColor = new Color(0.29f, 0.29f, 0.29f);
    public float fogLerpSpeed = 100f;

    [Header("Lightning")]
    public bool lightningEnabled = false;
    public Color fogLightningColor = new Color(0.29f, 0.29f, 0.29f);
}
