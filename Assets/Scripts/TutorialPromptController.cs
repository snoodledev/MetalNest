using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialPromptController : MonoBehaviour
{
    public GameObject TutorialPopup;
    //public GameObject RestartPopup;
    //private float timeUntilShowReset = 40f;
    //private float timeUntilActualReset = 80f;
    //private float timeSinceLastInput = 0f;
    private bool hasTouchedCloseTutorialPrompt = false;

    void Start()
    {
        TutorialPopup.SetActive(true);
        //RestartPopup.SetActive(false);
    }

    private void Update()
    {
        //// DEMO MODE RESET
        //if (!Input.anyKey)
        //    timeSinceLastInput += Time.deltaTime;
        //else
        //timeSinceLastInput = 0f;
        //if (hasTouchedCloseTutorialPrompt)
        //    if (timeSinceLastInput >= timeUntilShowReset)
        //    {
        //        RestartPopup.SetActive(true);
        //    }
        //    else
        //        RestartPopup.SetActive(false);
        //    if (timeSinceLastInput >= timeUntilActualReset)
        //    {
        //        SceneManager.LoadSceneAsync("MENU");
        //    }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TutorialPopup.SetActive(!TutorialPopup.activeSelf);
        }
    }


    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.tag == "CloseTutorialPrompt" && !hasTouchedCloseTutorialPrompt)
        {
            TutorialPopup.SetActive(false);
            hasTouchedCloseTutorialPrompt = true;
        }
    }
}
