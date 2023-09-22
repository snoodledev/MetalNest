using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyOrientator : MonoBehaviour
{
    [SerializeField] private GameObject body;
    private Quaternion bodyRot;

    private void Update()
    {
        bodyRot = body.transform.rotation;
        transform.rotation = Quaternion.identity;/*new Quaternion(transform.rotation.x, transform.rotation.y, body.transform.rotation.z, transform.rotation.w);*/
    }

}
