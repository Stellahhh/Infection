using UnityEngine;
using UnityEngine.InputSystem;

public class HumanToZombie : MonoBehaviour
{
    public GameObject zombiePrefab; // Assign in the Inspector
    public InputActionAsset inputActions; // Will store our created InputActionAsset

    private void Start()
    {
        
        Transform humanTransform = transform; // Store the position & rotation

        // Find the main camera
        Camera mainCamera = Camera.main;
        Transform cameraTransform = mainCamera.transform;

        // Reference the original player movement script
        PlayerMovement oldMovementScript = GetComponent<PlayerMovement>();

        // Instantiate the new zombie
        GameObject newZombie = Instantiate(zombiePrefab, humanTransform.position, humanTransform.rotation);
        newZombie.transform.localScale = humanTransform.localScale;

        // Add a CharacterController to the new zombie
        CharacterController newController = newZombie.AddComponent<CharacterController>();
        newController.center = new Vector3(0, 1, 0);  // Adjust if needed
        newController.radius = 0.5f;  // Adjust the radius if needed

        // Add PlayerInput component to the new zombie
        PlayerInput playerInput = newZombie.AddComponent<PlayerInput>();

        // Assign the InputActionAsset to the PlayerInput component
        playerInput.actions = inputActions; // Link our programmatically created InputActionAsset
        playerInput.defaultActionMap = "Player"; // Make sure this matches the action map name for the zombie

        // Move the camera to the new zombie
        cameraTransform.SetParent(newZombie.transform);
        cameraTransform.localPosition = new Vector3(0, 5, -10);  // Adjust camera offset as needed

        // Add PlayerMovement script to the new zombie
        PlayerMovement newMovement = newZombie.AddComponent<PlayerMovement>();
        newMovement.controller = newController;
        newMovement.playerInput = playerInput; // Make sure PlayerMovement uses the PlayerInput

        // Destroy the human AFTER transferring everything
        Destroy(gameObject);

    }


}
