using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsourceFlicker : MonoBehaviour
{
    public bool flickeringEnabled = false;
    public Vector2 onRange = new Vector2(1f,5f);
    public Vector2 offRange = new Vector2(0.1f,0.5f);

    private bool isFlickering = false;
    private float flickerDelay;

    void Update()
    {
        if (!isFlickering)
        {
            StartCoroutine(LightsourceFlickering());

        }

        IEnumerator LightsourceFlickering()
        {
            isFlickering = true;

            this.gameObject.GetComponent<Light>().enabled = false;
            flickerDelay = Random.Range(offRange.x, offRange.y);
            yield return new WaitForSeconds(flickerDelay);

            this.gameObject.GetComponent<Light>().enabled = true;
            flickerDelay = Random.Range(onRange.x, onRange.y);
            yield return new WaitForSeconds(flickerDelay);

            isFlickering = false;
        }
    }
}
