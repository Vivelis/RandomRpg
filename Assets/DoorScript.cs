using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{
    [Header("Nom de la sc�ne � charger")]
    [SerializeField] private string sceneName;

    [Header("Message d'interaction (facultatif)")]
    [SerializeField] private string interactionMessage = "Appuyez sur E pour entrer";

    private bool isPlayerInRange = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterController>() || other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log(interactionMessage);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterController>() || other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    void Update()
    {
        if (isPlayerInRange)
        {
            LoadScene();
        }
    }

    void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            Debug.Log($"Chargement de la sc�ne : {sceneName} depuis {SceneManager.GetActiveScene().name}");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Aucun nom de sc�ne d�fini !");
        }

        if (!string.IsNullOrEmpty(sceneName))
        {
            GameManager.Instance.PreviousScene = SceneManager.GetActiveScene().name;

            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Aucun nom de sc�ne d�fini !");
        }
    }
}
