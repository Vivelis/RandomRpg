using UnityEngine;
using UnityEngine.UI;

public class ScrollUV : MonoBehaviour
{
    public RawImage rawImage; // Référence à la RawImage
    public Vector2 scrollSpeed = new Vector2(0, -0.1f); // Vitesse de défilement

    private Vector2 uvOffset = Vector2.zero;

    void Update()
    {
        uvOffset += scrollSpeed * Time.deltaTime; // Calcul du décalage UV
        rawImage.uvRect = new Rect(uvOffset.x, uvOffset.y, rawImage.uvRect.width, rawImage.uvRect.height); // Appliquer le décalage
    }
}