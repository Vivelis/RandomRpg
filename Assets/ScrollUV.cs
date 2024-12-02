using UnityEngine;
using UnityEngine.UI;

public class ScrollUV : MonoBehaviour
{
    public RawImage rawImage;
    public Vector2 scrollSpeed = new Vector2(0, -0.1f);

    private Vector2 uvOffset = Vector2.zero;

    void Update()
    {
        uvOffset += scrollSpeed * Time.deltaTime;
        rawImage.uvRect = new Rect(uvOffset.x, uvOffset.y, rawImage.uvRect.width, rawImage.uvRect.height);
    }
}