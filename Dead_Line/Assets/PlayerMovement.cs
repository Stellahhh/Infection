using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    public CharacterController controller;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;

    private Vector3 velocity;
    private Transform cam;
    public PlayerInput playerInput;
    public InputAction moveAction;
    public InputAction lookAction;
    private Camera playerCamera;


    private void Start()
    {
        if (!isLocalPlayer) return;
        // Enable the camera only for the local player
        playerCamera = GetComponentInChildren<Camera>();
        playerCamera.gameObject.SetActive(true);
        cam = Camera.main.transform;
    }

    private void Awake()
    {
        if (!isLocalPlayer) return;

        if (controller == null)
        {
            controller = GetComponent<CharacterController>(); // Assign if it's missing
        }

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];

        moveAction.Enable();
        lookAction.Enable();

        cam = Camera.main.transform;
        if (cam == null)
        {
            Debug.LogError("Camera.main is null! Please assign a camera with the 'MainCamera' tag.");
        }
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];

        moveAction.Enable();
        lookAction.Enable();
        // Handle movement input
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        print("--------------" + moveInput);
        print("cam is null" + cam == null);
        // Convert input to world direction
        Vector3 moveDir = cam.forward * moveInput.y + cam.right * moveInput.x;
        moveDir.y = 0f; // Prevent movement in the Y direction
        controller.Move(moveDir * moveSpeed * Time.deltaTime);

        // Rotate the player using mouse input
        transform.Rotate(Vector3.up * lookInput.x * rotationSpeed * Time.deltaTime);

        // Apply gravity
        if (controller.isGrounded)
        {
            velocity.y = -2f; // Small downward force to keep grounded
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; // Apply gravity when not grounded
        }

        // Move the player with gravity applied
        controller.Move(velocity * Time.deltaTime);
    }
}
