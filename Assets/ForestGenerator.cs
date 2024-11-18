using UnityEngine;

public class ForestGenerator : MonoBehaviour
{
    public GameObject treePrefab; // Prefab de l'arbre
    public int treeCount = 100; // Nombre total d'arbres

    // Limites de la zone de spawn
    public float xMin = -25f;
    public float xMax = 25f;
    public float zMin = -25f;
    public float zMax = 25f;

    public float yPosition = 0f; // Hauteur des arbres (par d�faut)

    void Start()
    {
        GenerateForest();
    }

    void GenerateForest()
    {
        for (int i = 0; i < treeCount; i++)
        {
            // G�n�re une position al�atoire dans les limites sp�cifi�es
            Vector3 randomPosition = new Vector3(
                Random.Range(xMin, xMax),
                yPosition,
                Random.Range(zMin, zMax)
            );

            // Instancie l'arbre � la position g�n�r�e
            Instantiate(treePrefab, randomPosition, Quaternion.identity);
        }
    }

    // Dessine la zone de spawn dans l'�diteur pour visualisation
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f); // Vert transparent

        // Centre et taille de la zone
        Vector3 center = new Vector3((xMin + xMax) / 2, yPosition, (zMin + zMax) / 2);
        Vector3 size = new Vector3(xMax - xMin, 0.1f, zMax - zMin); // Hauteur = 0.1 pour le visuel

        // Dessine un cube repr�sentant la zone
        Gizmos.DrawCube(center, size);
    }
}
