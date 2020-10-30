using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime = 0.2f;

    private Vector3 currentVelocity;

    private void Update()
    {
        var targetPosition = player.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
        transform.LookAt(player.transform);
    }
}
