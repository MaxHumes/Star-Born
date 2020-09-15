using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.05f;
    public Vector3 lookOffset;
    public Vector3 positionOffset;

    void LateUpdate()
    {
        if(target != null)
        {
            Vector3 desiredPosition = target.position + positionOffset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            transform.LookAt(target.position + lookOffset);
        }
    }
}
