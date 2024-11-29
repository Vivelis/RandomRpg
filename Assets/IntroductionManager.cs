using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroductionManager : MonoBehaviour
{
    public TextMeshProUGUI[] introTexts; // Tableau de textes
    public GameObject startButton; // Bouton "Commencer"
    public float textDisplayTime = 4f; // Temps d'affichage de chaque texte
    public float fadeDuration = 1f; // Durée du fondu

    private void Start()
    {
        // Désactiver tous les textes au début
        foreach (var text in introTexts)
        {
            text.gameObject.SetActive(false);
        }
        startButton.SetActive(false); // Cacher le bouton
        StartCoroutine(PlayIntroduction());
    }

    private IEnumerator PlayIntroduction()
    {
        foreach (var text in introTexts)
        {
            // Activer uniquement le texte en cours
            text.gameObject.SetActive(true);

            // Apparition progressive du texte
            yield return StartCoroutine(FadeTextIn(text));
            yield return new WaitForSeconds(textDisplayTime);

            // Disparition progressive du texte
            yield return StartCoroutine(FadeTextOut(text));

            // Désactiver le texte une fois qu'il est invisible
            text.gameObject.SetActive(false);
        }

        // Afficher le bouton une fois l’introduction terminée
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
        // Charger la scène suivante
        SceneManager.LoadScene("Village");
    }
}