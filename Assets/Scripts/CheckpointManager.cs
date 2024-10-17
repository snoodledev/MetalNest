using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using UnityEngine.SceneManagement;

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
                Debug.Log("Player died, respawning at " + respawnPos);
            }
            else if (c.gameObject.tag == "Checkpoint")
            {
                Checkpoint(c);
                Debug.Log("Spawn set to " + c.gameObject.transform.position);
            }
            else if (c.gameObject.tag == "Ending")
            {
                Debug.Log("Game complete, loading end scene...");
                SceneManager.LoadScene("MENU");
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

