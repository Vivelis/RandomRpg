using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Configurations des positions de spawn")]
    [SerializeField] private SpawnConfiguration[] spawnConfigurations;

    [Header("Position et orientation par défaut")]
    [SerializeField] private Vector3 defaultSpawnPosition = Vector3.zero;
    [SerializeField] private float defaultSpawnRotationY = 0f;

    private void Start()
    {
        if (GameManager.Instance.PreviousScene != null)
        {
            string previousScene = GameManager.Instance.PreviousScene;
            foreach (var config in spawnConfigurations)
            {
                if (config.sceneName == previousScene)
                {
                    // Applique la position et l'orientation configurées
                    transform.position = config.spawnPosition;
                    transform.rotation = Quaternion.Euler(0, config.spawnRotationY, 0);
                    return;
                }
            }
        } else {
            transform.position = defaultSpawnPosition;
            transform.rotation = Quaternion.Euler(0, defaultSpawnRotationY, 0);
        }
    }
}

[System.Serializable]
public class SpawnConfiguration
{
    public string sceneName; // Nom de la scène précédente
    public Vector3 spawnPosition; // Position de spawn pour cette scène
    public float spawnRotationY; // Orientation Y (en degrés) du joueur
}
