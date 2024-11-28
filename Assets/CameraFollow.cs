using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Le joueur à suivre
    public float followDistance = 10.0f; // Distance derrière le joueur (plus éloignée)
    public float height = 6.0f; // Hauteur de la caméra (plus haute)
    public float lookDownAngle = 35.0f; // Angle de vue vers le bas
    private Vector3 initialOffset; // Décalage initial entre la caméra et le joueur
    private bool initialized = false; // Indique si la caméra est configurée

    void LateUpdate()
    {
        if (player == null)
        {
            return;
        }

        if (!initialized)
        {
            // Configure la position initiale de la caméra (derrière et plus éloignée du joueur)
            initialOffset = -player.forward * followDistance + Vector3.up * height;
            transform.position = player.position + initialOffset;

            // Oriente la caméra vers le joueur avec un angle vers le bas
            transform.rotation = Quaternion.Euler(lookDownAngle, player.eulerAngles.y, 0);
            initialized = true;
        }

        // Suit le joueur sans changer l'orientation
        Vector3 targetPosition = player.position + initialOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5.0f); // Suivi fluide
    }
}
