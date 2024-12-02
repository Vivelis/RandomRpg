using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroductionManager : MonoBehaviour
{
    public TextMeshProUGUI[] introTexts;
    public GameObject startButton;
    public float textDisplayTime = 4f;
    public float fadeDuration = 1f;

    private void Start()
    {
        foreach (var text in introTexts)
        {
            text.gameObject.SetActive(false);
        }
        startButton.SetActive(false);
        StartCoroutine(PlayIntroduction());
    }

    private IEnumerator PlayIntroduction()
    {
        foreach (var text in introTexts)
        {
            text.gameObject.SetActive(true);

            yield return StartCoroutine(FadeTextIn(text));
            yield return new WaitForSeconds(textDisplayTime);
            yield return StartCoroutine(FadeTextOut(text));

            text.gameObject.SetActive(false);
        }

        startButton.SetActive(true);
    }

    private IEnumerator FadeTextIn(TextMeshProUGUI text)
    {
        float timer = 0;
        Color color = text.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, timer / fadeDuration);
            text.color = color;
            yield return null;
        }
        color.a = 1;
        text.color = color;
    }

    private IEnumerator FadeTextOut(TextMeshProUGUI text)
    {
        float timer = 0;
        Color color = text.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, timer / fadeDuration);
            text.color = color;
            yield return null;
        }
        color.a = 0;
        text.color = color;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Village");
    }
}