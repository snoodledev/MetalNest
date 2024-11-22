using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuScene : MonoBehaviour
{
    public TMP_Text startText;
    public GameObject loadingIndicator;

    public Color red => new Color(1f, 0f, 0f);
    public Color white => Color.white;

    private void Awake()
    {
        loadingIndicator.SetActive(false);
    }


    public void Update()
    {
        startText.color = LerpColor(red, white);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            loadingIndicator.SetActive(true);
            SceneManager.LoadSceneAsync("MAIN");
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public Color LerpColor(Color firstColor, Color secondColor) => Color.Lerp(firstColor, secondColor, Mathf.Sin(Time.time));
}

