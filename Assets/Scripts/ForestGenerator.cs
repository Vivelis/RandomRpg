using UnityEngine;

public class ForestGenerator : MonoBehaviour
{
    public GameObject treePrefab;
    public int treeCount = 100;

    public float xMin = -25f;
    public float xMax = 25f;
    public float zMin = -25f;
    public float zMax = 25f;

    public float yPosition = 0f;

    void Start()
    {
        GenerateForest();
    }

    void GenerateForest()
    {
        for (int i = 0; i < treeCount; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(xMin, xMax),
                yPosition,
                Random.Range(zMin, zMax)
            );

            Instantiate(treePrefab, randomPosition, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);

        Vector3 center = new Vector3((xMin + xMax) / 2, yPosition, (zMin + zMax) / 2);
        Vector3 size = new Vector3(xMax - xMin, 0.1f, zMax - zMin);

        Gizmos.DrawCube(center, size);
    }
}
