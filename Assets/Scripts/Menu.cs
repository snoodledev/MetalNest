using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    public TMP_Text startText;

    public Color red => new Color(1f, 0f, 0f);
    public Color white => Color.white;


    public void Update()
    {
        startText.color = LerpColor(red, white);
        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene("MAIN");
    }

    public Color LerpColor(Color firstColor, Color secondColor) => Color.Lerp(firstColor, secondColor, Mathf.Sin(Time.time));
}

