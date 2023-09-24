using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsourceFlicker : MonoBehaviour
{
    [SerializeField] private bool flickerEnabled = true;
    [SerializeField] private Vector2 onRange = new Vector2(1f,5f);
    [SerializeField] private Vector2 offRange = new Vector2(0.1f,0.5f);
    [SerializeField] private Vector2 intensityRange = new Vector2(1f,6f);
    [SerializeField] private float standardIntensityVariance = 0.2f;

    private Light lightComponent;
    private bool flickerOn = false;
    private float flickerDelay;
    private float flickerIntensity;
    private float flickerIntensityDefault;

    //private LightsourceFlicker emitter;

    private void Start()
    {
        //emitter = GetComponent<LightsourceFlicker>();
        flickerIntensityDefault = gameObject.GetComponent<Light>().intensity;
        lightComponent = gameObject.GetComponent<Light>();
    }

    private void Update()
    {
        if (flickerEnabled && !flickerOn)
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
    }
}
