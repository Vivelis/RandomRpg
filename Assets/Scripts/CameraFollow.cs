using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float followDistance = 10.0f;
    public float height = 6.0f;
    public float lookDownAngle = 35.0f;
    private Vector3 initialOffset;
    private bool initialized = false;

    void LateUpdate()
    {
        if (player == null)
        {
            return;
        }

        if (!initialized)
        {
            initialOffset = -player.forward * followDistance + Vector3.up * height;
            transform.position = player.position + initialOffset;

            transform.rotation = Quaternion.Euler(lookDownAngle, player.eulerAngles.y, 0);
            initialized = true;
        }

        Vector3 targetPosition = player.position + initialOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5.0f); // Suivi fluide
    }
}
