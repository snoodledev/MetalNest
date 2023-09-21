using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVolumeToggle : MonoBehaviour
{
    [SerializeField] private GameObject HPSXRPVolume;

    void Start()
    {
        HPSXRPVolume.SetActive(true);
    }
}
