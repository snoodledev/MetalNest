using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotat : MonoBehaviour
{
    [SerializeField] private float speedX = 0f;
    [SerializeField] private float speedY = 0.1f;
    [SerializeField] private float speedZ = 0f;
    void Update()
    {
        transform.Rotate(speedX, speedY, speedZ);
    }
}
