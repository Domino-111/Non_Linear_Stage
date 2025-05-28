using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 1f;

    float currentRotationHorizontal;
    float currentRotationVertical;

    public Transform playerTransform;
    public float playerEyeLevel = 0.5f;

    public float verticalRotationMin;
    public float verticalRotationMax;

    private void Start()
    {
        currentRotationHorizontal = transform.localEulerAngles.y;
        currentRotationVertical = transform.localEulerAngles.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        currentRotationHorizontal += Input.GetAxis("Mouse X") * sensitivity;
        currentRotationVertical -= Input.GetAxis("Mouse Y") * sensitivity;

        currentRotationVertical = Mathf.Clamp(currentRotationVertical, verticalRotationMin, verticalRotationMax);

        transform.localEulerAngles = new Vector3(currentRotationVertical, currentRotationHorizontal, 0);

        transform.position = playerTransform.position + Vector3.up * playerEyeLevel;
    }
}