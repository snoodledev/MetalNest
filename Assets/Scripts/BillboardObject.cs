using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardObject : MonoBehaviour
{
    private GameObject player;
    
    private void Start()
    {
        player = GameObject.Find("PlayerCamera");
    }

    void Update()
    {
        if (player == null) { return; }

        transform.LookAt(player.transform);
    }
}
