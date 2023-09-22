using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsourceFlicker : MonoBehaviour
{
    [SerializeField] private bool flickerEnabled = true;
    [SerializeField] private Vector2 onRange = new Vector2(1f,5f);
    [SerializeField] private Vector2 offRange = new Vector2(0.1f,0.5f);

    private bool flickerOn = false;
    private float flickerDelay;

    //private LightsourceFlicker emitter;

    //private void Start()
    //{
    //    emitter = GetComponent<LightsourceFlicker>();
    //}

    private void Update()
    {
        if (flickerEnabled && !flickerOn)
        {
            StartCoroutine(LightsourceFlickering());

        }

        IEnumerator LightsourceFlickering()
        {
            flickerOn = true;

            this.gameObject.GetComponent<Light>().enabled = false;
            flickerDelay = Random.Range(offRange.x, offRange.y);
            yield return new WaitForSeconds(flickerDelay);

            this.gameObject.GetComponent<Light>().enabled = true;
            flickerDelay = Random.Range(onRange.x, onRange.y);
            yield return new WaitForSeconds(flickerDelay);

            flickerOn = false;
        }
    }
}
