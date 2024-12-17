using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float followDistance = 10.0f;
    public float height = 6.0f;
    public float lookDownAngle = 35.0f;
    [SerializeField]
    private Vector3 initialOffset;
    private bool initialized = false;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        BattleData battleData = BattleData.Instance;
        if (battleData != null)
        {
            Vector3 previousCameraPosition = battleData.previousCameraPosition;
            Quaternion previousCameraRotation = battleData.previousCameraRotation;

            if (previousCameraPosition != Vector3.zero)
            {
                transform.position = previousCameraPosition;
                transform.rotation = previousCameraRotation;
            }
        }
        SetInitialOffset();
    }

    public void SetInitialOffset()
    {
        initialOffset = -player.forward * followDistance + Vector3.up * height;
        transform.position = player.position + initialOffset;

        transform.rotation = Quaternion.Euler(lookDownAngle, player.eulerAngles.y, 0);
        initialized = true;
    }

    void LateUpdate()
    {
        if (player == null)
        {
            return;
        }

        Vector3 targetPosition = player.position + initialOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5.0f); // Suivi fluide
    }
}
