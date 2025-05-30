using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float currentWalkSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpPower;
    [SerializeField] private float speedThisFrame;
    public LayerMask groundedMask;
    [SerializeField] Vector3 movementThisFrame;
    [SerializeField] Vector2 inputThisFrame;

    public float jumpTimer, timerLength;

    public Rigidbody rb;

    public Transform cameraTransform;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentWalkSpeed = walkSpeed;
    }

    private void Update()
    {
        //get our inputs this frame
        inputThisFrame.x = Input.GetAxis("Horizontal");
        inputThisFrame.y = Input.GetAxis("Vertical");

        //reset our potential movement to 0, 0, 0
        movementThisFrame = Vector3.zero;

        //apply our new input direction right/left and forward/back
        movementThisFrame.x = inputThisFrame.x;
        movementThisFrame.z = inputThisFrame.y;

        //figure out what our speed should be this frame
        speedThisFrame = walkSpeed;

        movementThisFrame *= speedThisFrame;

        movementThisFrame.y = rb.linearVelocity.y - gravity * Time.deltaTime;

        Movement(movementThisFrame);

        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
            {
                if (hit.transform.TryGetComponent<Interactable>(out Interactable interact))
                {
                    interact.Click();
                }
            }
        }

        jumpTimer -= Time.deltaTime;
        Jump();
    }

    private void Movement(Vector3 movement)
    {
        transform.localEulerAngles = new Vector3(0, cameraTransform.localEulerAngles.y, 0);

        movement = transform.TransformDirection(movement);

        rb.linearVelocity = movement;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpTimer <= 0)
        {
            //movementThisFrame.y = jumpPower;
            rb.AddForce(Vector3.up * jumpPower);

            jumpTimer = timerLength;
        }
    }
}
