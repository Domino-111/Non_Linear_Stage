using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("How fast the camera should move as the player looks around")]
    [SerializeField] private float sensitivity;
    [Tooltip("The minimum allowed rotation around the x-axis")]
    [SerializeField] private float verticalRotationMin;
    [Tooltip("The maximum allowed rotation around the x-axis")]
    [SerializeField] private float verticalRotationMax;

    [Tooltip("The player object's transform")]
    [SerializeField] private Transform playerTransform;

    [Tooltip("How far up from the player's centre should the camera rest")]
    [SerializeField] private float playerEyeLevel = 0.5f;

    //Track our current rotation values
    private float currentHorizontalRotation, currentVerticalRotation;

    //Hold the camera in use by this script
    new public Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        currentHorizontalRotation = transform.localEulerAngles.y;
        currentVerticalRotation = transform.localEulerAngles.x;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        GetRotationFromInput();
        ClampVerticalRotation();
        ApplyRotation();

        //snap to the player's position, adjusted upwards using the eye level provided
        transform.position = playerTransform.position + Vector3.up * playerEyeLevel;
    }

    // Use the mouse inputs to adjust the current camera rotation
    private void GetRotationFromInput()
    {
        currentHorizontalRotation += Input.GetAxis("Mouse X") * sensitivity;
        currentVerticalRotation -= Input.GetAxis("Mouse Y") * sensitivity;
    }

    // Clamp the x-axis rotation using the minimum and maximum provided
    private void ClampVerticalRotation()
    {
        currentVerticalRotation = Mathf.Clamp(currentVerticalRotation, verticalRotationMin, verticalRotationMax);
    }

    // Set the camera's rotation as determined
    private void ApplyRotation()
    {
        transform.localEulerAngles = new Vector3(currentVerticalRotation, currentHorizontalRotation, 0);
    }

    public Transform GetCameraTransform()
    {
        return camera.transform;
    }
}