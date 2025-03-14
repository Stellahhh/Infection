// Linda Fan, Stella Huo, Hanbei Zhou
using UnityEngine;
using UnityEngine.UI;
using TMPro; // For TextMeshPro support
using UnityEngine.InputSystem;
using Mirror;

public class ZombieMovement : NetworkBehaviour
{
    public CharacterController controller;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f; // New variable for jump strength
    private int jumpsRemaining = 2; // 🔹 Allow double jump
    [SerializeField] TextMeshProUGUI hungerText;
    public Hunger hungerScript; 
    private Vector3 velocity;
    private Transform cam;
    public PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction; // New InputAction for jump

    private Camera playerCamera;
    private bool isGrounded;
    // public Animator animator;
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
        GameObject hungerUI = GameObject.Find("HungerText");
        if (hungerUI != null)
        {
            hungerText = hungerUI.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("HungerText UI element not found!");
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

        print("network identity" + (GetComponent<NetworkIdentity>() == null));

    }



    private void Awake()
    {
        
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        AdjustSpeedBasedOnHunger();
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
            if (cam != playerCamera && cam.tag != "MiniMapCam")
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


        //  if (moveInput.magnitude > 0f)
        // {
        //     animator.SetBool("isWalking", true);  // Trigger walk animation
        // }
        // else
        // {
        //     animator.SetBool("isWalking", false); // Stop walking animation
        // }
        //Rotate the player using mouse input
        Vector2 lookInput = lookAction.ReadValue<Vector2>();
        transform.Rotate(Vector3.up * lookInput.x * rotationSpeed * Time.deltaTime);

        // Double Jump Logic
        if (jumpAction.WasPressedThisFrame() && jumpsRemaining > 0)
        {
            Debug.Log("Jump button pressed! Jumps left: " + jumpsRemaining);
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Apply jump force
            jumpsRemaining--; // 🔹 Reduce jump count
            //animator.SetBool("isJumping", true);
        }
        

        // Apply Gravity Properly
        velocity.y += gravity * Time.deltaTime;
        // if (isGrounded && velocity.y <= 0)
        // {
        //     animator.SetBool("isJumping", false); // Set jumping to false if grounded
        // }
        // Move Player with Updated Velocity
        controller.Move(velocity * Time.deltaTime);

    }
    void AdjustSpeedBasedOnHunger()
    {
        // Example: As remainingTime decreases, increase the speed.
        // You can change this multiplier for the exact scaling behavior you want.
        if (hungerScript.remainingTime > 0)
        {
            // You can adjust this formula to your desired speed increase curve
            moveSpeed = Mathf.Lerp(5f, 10f, 1 - (hungerScript.remainingTime / 100f)); // Speed increases as remainingTime decreases
        }
        else
        {
            moveSpeed = 10f; // Maximum speed when hunger time reaches zero
        }
    }
   
}
