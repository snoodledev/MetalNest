using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScene : MonoBehaviour
{
    public TMP_Text text;
    public Color red = new Color(1f, 0f, 0f);
    public Color white = Color.white;
    public float flashSpeed = 5f;

    private float timer = 0f;


    public void Update()
    {
        timer += Time.deltaTime;
        text.color = LerpColor(red, white);
        if (timer > 6f)
        {
            SceneManager.LoadSceneAsync("MENU");
        }
    }

    public Color LerpColor(Color firstColor, Color secondColor) => Color.Lerp(firstColor, secondColor, Mathf.Sin(Time.time * flashSpeed));
}

