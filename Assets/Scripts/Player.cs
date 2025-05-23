using UnityEngine;

//The RequireComponent attribute checks a GameObject for the defined components, and adds them if they are missing.
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class CustomController : MonoBehaviour
{
    // This enumerator will define all our possible player states
    public enum State
    {
        Walk,
        Rise,
        Fall,
    }

    [Tooltip("To track the current behaviour of the player")]
    [SerializeField] private State currentState;

    [Tooltip("How many units per sec the player should move by default")]
    [SerializeField] private float speedWalk;

    [Tooltip("How much upward momentum to start with when jumping")]
    [SerializeField] private float jumpPower;

    [Tooltip("Reduce the player's vertical momentum by this many units per second")]
    [SerializeField] private float gravity;

    [Tooltip("What physics layer should the player object recognise as the ground")]
    [SerializeField] private LayerMask groundLayer;

    [Tooltip("The maximum number of jumps before the player touches the ground again")]
    [SerializeField] private int jumpsAllowed = 1;

    // The number of jumps the player currently has available
    private int jumpsRemaining;

    // To hold the rigidbody on the player object
    private Rigidbody rb;

    // Get a reference to the cameraController to move with it's direction
    public CameraController cameraController;

    // To hold the player's collider
    private CapsuleCollider capsuleCollider;

    void Start()
    {
        // Get the rigidbody component from the player object
        rb = GetComponent<Rigidbody>();

        // Prevent the rigidbody from rotating
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // This script manages gravity its own way
        rb.useGravity = false;

        // Get the capsule collider component from the player object
        capsuleCollider = GetComponent<CapsuleCollider>();

        // Refresh the number of jumps the player has available
        jumpsRemaining = jumpsAllowed;
    }

    void Update()
    {
        // Depending on our current state, choose a different set of behaviour to follow
        switch (currentState)
        {
            case State.Walk:
                WalkState();
                break;
            case State.Rise:
                RiseState();
                break;
            case State.Fall:
                FallState();
                break;
        }
    }

    private void WalkState()
    {
        // Make sure we have all our jumps available
        jumpsRemaining = jumpsAllowed;

        // Get a movement direction based on inputs and translated using the camera direction
        Vector3 inputMovement = GetMovementFromInput();

        // Increase that using our base walk speed 
        inputMovement *= speedWalk;

        // Adjust up/down speed based on gravity but since we're walking we shouldn't fall
        inputMovement.y = Mathf.Clamp(rb.linearVelocity.y - gravity * Time.deltaTime, 0f, float.PositiveInfinity);

        // If we are no longer on the ground...
        if (!IsGrounded())
        {
            // ...we should be falling
            currentState = State.Fall;

            // Reduce our jumps by 1
            jumpsRemaining -= 1;

            return;
        }

        // If we make it here, we must be on the ground.
        TryToJump();
    }

    private void RiseState()
    {
        // Set movement based on input direction, camera direction, and walking speed
        Vector3 inputMovement = GetMovementFromInput();
        inputMovement *= speedWalk;

        // Apply gravity
        inputMovement.y = rb.linearVelocity.y - gravity * Time.deltaTime;

        // Apply the determined movement to our rigidbody
        rb.linearVelocity = inputMovement;

        // If velocity.y is less than 0, we are moving down, so we should enter Fall state
        if (rb.linearVelocity.y < 0f)
        {
            currentState = State.Fall;
        }

        TryToJump();
    }

    private void FallState()
    {
        // Set movement based on input, camera, walk speed, and apply gravity
        Vector3 inputMovement = GetMovementFromInput();
        inputMovement *= speedWalk;
        inputMovement.y = rb.linearVelocity.y - gravity * Time.deltaTime;

        // Apply movement to the rigidbody
        rb.linearVelocity = inputMovement;

        // If we are on the ground..
        if (IsGrounded())
        {
            // ...Change to the Walk state
            currentState = State.Walk;
        }

        TryToJump();
    }

    private void TryToJump()
    {
        // If the player presses jump...
        if (Input.GetButtonDown("Jump") && jumpsRemaining > 0)
        {
            // ... add upwards momentum to the player and change to the Rise state 
            RiseAtSpeed(jumpPower);

            //Reduce our remaining jumps
            jumpsRemaining -= 1;
        }
    }

    private void RiseAtSpeed(float speed)
    {
        // Set our vertical momentum upward using the provided speed 
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, speed, rb.linearVelocity.z);

        // Change to the Rise state
        currentState = State.Rise;
    }

    // Get current inputs and translate to a movement direction
    private Vector3 GetMovementFromInput()
    {
        Vector2 inputThisFrame = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 moveDirection = new Vector3(inputThisFrame.x, 0, inputThisFrame.y);

        // Get the transform of the currently active camera
        Transform cameraTransform = cameraController.GetCameraTransform();

        // Translate the movement direction based on the camera's transform
        moveDirection = cameraTransform.TransformDirection(moveDirection);

        return moveDirection;
    }

    private bool IsGrounded()
    {
        //Raycast downwards from our centre, using half of our collider's height
        return Physics.Raycast(transform.position, Vector3.down, capsuleCollider.height / 2f + 0.01f, groundLayer);
    }
}