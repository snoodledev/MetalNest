using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTriggerCheck : MonoBehaviour
{
    void OnTriggerEnter()
    {
        Debug.Log("entered " + gameObject.name);
    }

    void OnTriggerExit()
    {
        Debug.Log("exited " + gameObject.name);

    }
}
