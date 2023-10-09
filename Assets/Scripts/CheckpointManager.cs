using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;

public class CheckpointManager : MonoBehaviour
{
    private Vector3 respawnPos = Vector3.zero;
    private KinematicCharacterMotor motor;
    public float respawnTime = 0.5f;
    private bool respawning = false;

    private void Start()
    {
        motor = GetComponent<KinematicCharacterMotor>();
    }

    private void OnTriggerEnter(Collider c)
    {
        if (!respawning)
            {
            if (c.gameObject.tag == "Kill")
            {
                StartCoroutine("TimedRespawn");
                //Debug.Log("KILL, respawning at " + respawnPos);
            }
            else if (c.gameObject.tag == "Checkpoint")
            {
                Checkpoint(c);
                //Debug.Log("Spawn set to " + c.gameObject.transform.position);
            }
        }
    }
    private void Checkpoint(Collider c)
    {
        respawnPos = c.gameObject.transform.position;
    }
    IEnumerator TimedRespawn()
    {
        respawning = true;
        yield return new WaitForSeconds(respawnTime);
        gameObject.transform.position = respawnPos;
        motor.SetPosition(respawnPos, true);
        respawning = false;
    }
}

