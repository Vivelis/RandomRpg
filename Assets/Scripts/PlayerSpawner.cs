using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Configurations des positions de spawn")]
    [SerializeField] private SpawnConfiguration[] spawnConfigurations;

    [Header("Position et orientation par d�faut")]
    [SerializeField] private Vector3 defaultSpawnPosition = Vector3.zero;
    [SerializeField] private float defaultSpawnRotationY = 0f;

    private void Awake()
    {
        if (GameManager.Instance != null) {
            if (GameManager.Instance.PreviousScene != null)
            {
                string previousScene = GameManager.Instance.PreviousScene;

                foreach (var config in spawnConfigurations)
                {
                    if (config.sceneName == previousScene)
                    {
                        Debug.Log("Position1 de spawn configur�e pour la sc�ne " + previousScene);
                        // Applique la position et l'orientation configur�es
                        transform.position = config.spawnPosition;
                        transform.rotation = Quaternion.Euler(0, config.spawnRotationY, 0);
                        break;
                    }
                }
            }
            if (!(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "BattleSystem")) {
                if (BattleData.Instance != null && BattleData.Instance.previousPosition != Vector3.zero) {
                    transform.position = BattleData.Instance.previousPosition;
                    transform.rotation = BattleData.Instance.previousRotation;
                    BattleData.Instance.previousPosition = Vector3.zero;
                    BattleData.Instance.previousRotation = Quaternion.identity;
                    BattleData.Instance.previousCameraPosition = Vector3.zero;
                    BattleData.Instance.previousCameraRotation = Quaternion.identity;
                    Debug.Log("Set player position to : " + transform.position);
                    return;
                }
                // transform.position = defaultSpawnPosition;
                // transform.rotation = Quaternion.Euler(0, defaultSpawnRotationY, 0);
                // Debug.Log("Position2 de spawn par d�faut, valeur : " + defaultSpawnPosition);
            } else {
                GetComponent<BasicController>().canMove = false;
            }
        }
    }
}

[System.Serializable]
public class SpawnConfiguration
{
    public string sceneName; // Nom de la sc�ne pr�c�dente
    public Vector3 spawnPosition; // Position de spawn pour cette sc�ne
    public float spawnRotationY; // Orientation Y (en degr�s) du joueur
}
