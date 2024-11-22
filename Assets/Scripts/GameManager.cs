using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;

public class GameManager : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            //RuntimeManager.GetBus("Ambience").stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
            SceneManager.LoadSceneAsync("MENU");
        }
    }

}

