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
        if (!isLocalPlayer) return; // Ensure this runs only for the local player

        // Find the camera inside this player's prefab
        playerCamera = GetComponentInChildren<Camera>();

        if (playerCamera != null)
        {
            playerCamera.gameObject.SetActive(true); // Enable only this player's camera
        }

        // Disable all other player cameras
        foreach (Camera cam in FindObjectsOfType<Camera>())
        {
            if (cam != playerCamera)
            {
                cam.gameObject.SetActive(false);
            }
        }
    
    }



private void Awake()
    {
        if (!isLocalPlayer) return;

        if (controller == null)
        {
            controller = GetComponent<CharacterController>(); // Assign if missing
        }

        //playerInput = GetComponent<PlayerInput>();
        //moveAction = playerInput.actions["Move"];
        //lookAction = playerInput.actions["Look"];
        //moveAction.Enable();
        //lookAction.Enable();

        //// Try finding the camera within the player object
        //Camera myCam = GetComponentInChildren<Camera>();

        //if (myCam != null)
        //{
        //    cam = myCam.transform; // Assign camera transform
        //}
        //else
        //{
        //    Debug.LogError("No camera found in child objects of the player!");
        //}
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


        Debug.Log("Active camera: " + Camera.main?.name);
        Debug.Log("Assigned player camera: " + (cam != null ? cam.name : "NULL"));

        // Handle movement input
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        print("--------------" + moveInput);
        print("cam is null: " + (cam == null));

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
