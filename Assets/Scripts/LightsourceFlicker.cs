using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsourceFlicker : MonoBehaviour
{
    public LightMode mode = LightMode.Flicker;
    
    [Header("Flicker")]
    public Vector2 onRange = new Vector2(1f,5f);
    public Vector2 offRange = new Vector2(0.1f,0.5f);
    public Vector2 intensityRange = new Vector2(1f,6f);
    public float standardIntensityVariance = 0.2f;

    private Light lightComponent;
    private bool flickerOn = false;
    private float flickerDelay;
    private float flickerIntensity;
    private float flickerIntensityDefault;

    [Header("Pulse")]
    [Range(0f,0.99f)] public float pulseOffset = 0f;
    public float pulseMaxIntensity = 40f;
    public float pulseSpeed = 1f;
    public AnimationCurve pulseEasing;

    [HideInInspector] public float _pulsePosition = 0f;
    [HideInInspector] public bool _pulseAscending = true;

    [Header("Billboard")]
    public GameObject bloomObject;
    [Range(0f, 1f)] public float bloomIntensity = 0.5f;
    private Renderer bRend;

    private void Start()
    {
        flickerIntensityDefault = gameObject.GetComponent<Light>().intensity;
        lightComponent = gameObject.GetComponent<Light>();

        if (bloomObject != null && mode == LightMode.Pulse)
        {
            bRend = bloomObject.GetComponent<Renderer>();
        }
    }

    private void Update()
    {
        // FLICKER
        if (mode == LightMode.Flicker && !flickerOn)
        {
            StartCoroutine(LightsourceFlickering());

        }

        IEnumerator LightsourceFlickering()
        {
            flickerOn = true;

            flickerIntensity = Random.Range(intensityRange.x, intensityRange.y);
            lightComponent.intensity = flickerIntensity;
            flickerDelay = Random.Range(offRange.x, offRange.y);
            yield return new WaitForSeconds(flickerDelay);

            lightComponent.intensity = flickerIntensityDefault + Random.Range(-standardIntensityVariance, standardIntensityVariance);
            flickerDelay = Random.Range(onRange.x, onRange.y);
            yield return new WaitForSeconds(flickerDelay);

            flickerOn = false;
        }

        // PULSE
        // pulse value setting
        if (mode == LightMode.Pulse)
        {
            if (_pulseAscending)
            {
                if (_pulsePosition < 1f) {
                    _pulsePosition += 1 / pulseSpeed * Time.deltaTime;
                }
                if (_pulsePosition >= 1f) {
                    _pulsePosition = 1f;
                    _pulseAscending = false;
                }
            } else if (!_pulseAscending)
            {
                if (_pulsePosition > 0f)
                {
                    _pulsePosition -= 1 / pulseSpeed * Time.deltaTime;
                }
                if (_pulsePosition <= 0f)
                {
                    _pulsePosition = 0f;
                    _pulseAscending = true;
                }
            }
            //Debug.Log(gameObject + ", " + _pulsePosition);
            lightComponent.intensity = EvaluateCurve(pulseEasing, _pulsePosition) * pulseMaxIntensity;
            
            if (bloomObject != null)
            {
                bRend.material.SetColor("_MainColor", new Color(lightComponent.color.r, lightComponent.color.g, lightComponent.color.b, _pulsePosition * bloomIntensity));
            }
        }
    }

    private float EvaluateCurve(AnimationCurve curve, float position)
    {
        return curve.Evaluate(position);
    }

    public enum LightMode
    {
        None,
        Flicker, 
        Pulse
    }
}
