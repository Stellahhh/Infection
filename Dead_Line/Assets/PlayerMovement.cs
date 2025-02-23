using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;

    private Vector3 velocity;
    private Transform cam;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction lookAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        cam = Camera.main.transform;
    }

    void Update()
    {
        // Get input from the new Input System
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

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
            velocity.y += gravity * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
    }
}
