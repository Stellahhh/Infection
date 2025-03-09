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

    private Vector3 velocity;
    private Transform cam;
    public PlayerInput playerInput;
    public InputAction moveAction;
    public InputAction lookAction;
    private Camera playerCamera;

    private void Start()
    {
        // Called after this object is fully initialized as the local player
        if (!isLocalPlayer) return;

        Camera myCam = GetComponentInChildren<Camera>();
        if (myCam != null)
        {
            myCam.gameObject.SetActive(true); // Enable only this player's camera
        }
        else
        {
            Debug.LogError("Camera not found for local player.");
        }

        // Ensure player input and actions are set up
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found!");
            return;
        }

        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        if (moveAction == null || lookAction == null)
        {
            Debug.LogError("Input actions not properly set up.");
            return;
        }

        // Initialize character controller if needed
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterController not found!");
            return;
        }

        print("network identity" + (GetComponent<NetworkIdentity>() == null));
        
    }



    private void Awake()
    {
       
    }

    void Update()
    {
        if (!isLocalPlayer) return;


        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        moveAction.Enable();
        lookAction.Enable();

        // Try finding the camera within the player object
        Camera myCam = GetComponentInChildren<Camera>(true);

        playerCamera = GetComponentInChildren<Camera>();

        if (playerCamera != null)
        {
            playerCamera.gameObject.SetActive(true); // Enable only this player's camera
            cam = playerCamera.transform;
        }

        // Disable all other player cameras
        foreach (Camera cam in FindObjectsOfType<Camera>())
        {
            if (cam != playerCamera)
            {
                cam.gameObject.SetActive(false);
            }
        }


        // Handle movement input
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

   

        if (cam == null) return;  // Prevents NullReferenceException

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
