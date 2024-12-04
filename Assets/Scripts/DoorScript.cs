using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{
    [Header("Nom de la scène à charger")]
    [SerializeField] private string sceneName;

    [Header("Message d'interaction (facultatif)")]
    [SerializeField] private string interactionMessage = "Entrer";

    private bool isPlayerInRange = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterController>() || other.CompareTag("Player"))
        {
            isPlayerInRange = true;
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
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Aucun nom de scène défini !");
        }

        if (!string.IsNullOrEmpty(sceneName))
        {
            GameManager.Instance.PreviousScene = SceneManager.GetActiveScene().name;

            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Aucun nom de scène défini !");
        }
    }
}
