using UnityEngine;
using System.Collections.Generic;

public class TransparentObjects : MonoBehaviour
{
    public Transform player; // R�f�rence au joueur
    public LayerMask obstacleLayer; // Couches des objets � rendre transparents
    public float transparencyAmount = 0.3f; // Niveau de transparence (entre 0 et 1)

    private List<Renderer> previousRenderers = new List<Renderer>();

    void Update()
    {
        // R�initialiser les objets pr�c�demment transparents
        ResetTransparency();

        // Raycast entre la cam�ra et le joueur
        Vector3 direction = player.position - transform.position;
        Ray ray = new Ray(transform.position, direction);
        float distance = direction.magnitude;

        RaycastHit[] hits = Physics.RaycastAll(ray, distance, obstacleLayer);

        foreach (RaycastHit hit in hits)
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Rendre l'objet transparent
                SetTransparency(renderer);
                previousRenderers.Add(renderer);
            }
        }
    }

    void SetTransparency(Renderer renderer)
    {
        foreach (Material mat in renderer.materials)
        {
            if (mat.HasProperty("_Color"))
            {
                Color color = mat.color;
                color.a = transparencyAmount; // Ajuster la transparence
                mat.color = color;
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;
            }
        }
    }

    void ResetTransparency()
    {
        foreach (Renderer renderer in previousRenderers)
        {
            foreach (Material mat in renderer.materials)
            {
                if (mat.HasProperty("_Color"))
                {
                    Color color = mat.color;
                    color.a = 1f; // Remettre l'opacit�
                    mat.color = color;
                    mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    mat.SetInt("_ZWrite", 1);
                    mat.DisableKeyword("_ALPHATEST_ON");
                    mat.DisableKeyword("_ALPHABLEND_ON");
                    mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    mat.renderQueue = -1;
                }
            }
        }

        previousRenderers.Clear();
    }
}