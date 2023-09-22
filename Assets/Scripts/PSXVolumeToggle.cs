using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSXVolumeToggle : MonoBehaviour
{
    [SerializeField] private GameObject HPSXRPVolume;

    void Start()
    {
        HPSXRPVolume.SetActive(true);
    }
}
