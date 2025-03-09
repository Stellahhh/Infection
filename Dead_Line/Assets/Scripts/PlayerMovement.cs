// Linda Fan, Stella Huo, Hanbei Zhou
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    public CharacterController controller;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f; // New variable for jump strength
    private int jumpsRemaining = 2; // 🔹 Allow double jump

    private Vector3 velocity;
    private Transform cam;
    public PlayerInput playerInput;
    public InputAction moveAction;
    public InputAction lookAction;
    public InputAction jumpAction; // New InputAction for jump

    private Camera playerCamera;
    private bool isGrounded;

    private void Start()
    {
        if (!isLocalPlayer) return;

        Camera myCam = GetComponentInChildren<Camera>();
        if (myCam != null)
        {
            myCam.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Camera not found for local player.");
        }

        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found!");
            return;
        }

        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        jumpAction = playerInput.actions["Jump"]; // Assign jump action

        if (moveAction == null || lookAction == null || jumpAction == null)
        {
            Debug.LogError("Input actions not properly set up.");
            return;
        }

        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterController not found!");
            return;
        }
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        // Ensure actions are enabled
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();

        // Handle camera
        Camera myCam = GetComponentInChildren<Camera>(true);
        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera != null)
        {
            playerCamera.gameObject.SetActive(true);
            cam = playerCamera.transform;
        }
        foreach (Camera cam in FindObjectsOfType<Camera>())
        {
            if (cam != playerCamera)
            {
                cam.gameObject.SetActive(false);
            }
        }

        if (cam == null) return;

        // Ground Check
        isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            jumpsRemaining = 2; // 🔹 Reset jump count when grounded
            if (velocity.y < 0)
            {
                velocity.y = -2f; // Reset velocity when touching the ground
            }
        }

        // Handle movement input
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 moveDir = cam.forward * moveInput.y + cam.right * moveInput.x;
        moveDir.y = 0f;
        controller.Move(moveDir * moveSpeed * Time.deltaTime);

        // Rotate the player using mouse input
        Vector2 lookInput = lookAction.ReadValue<Vector2>();
        transform.Rotate(Vector3.up * lookInput.x * rotationSpeed * Time.deltaTime);

        // ✅ Double Jump Logic
        if (jumpAction.WasPressedThisFrame() && jumpsRemaining > 0)
        {
            Debug.Log("Jump button pressed! Jumps left: " + jumpsRemaining);
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Apply jump force
            jumpsRemaining--; // 🔹 Reduce jump count
        }

        // ✅ Apply Gravity Properly
        velocity.y += gravity * Time.deltaTime;

        // ✅ Move Player with Updated Velocity
        controller.Move(velocity * Time.deltaTime);

        // Debug check
        if (jumpAction.WasPressedThisFrame())
        {
            Debug.Log("Jump action detected, velocity.y = " + velocity.y);
        }
    }
}
