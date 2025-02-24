using UnityEngine;
using UnityEngine.InputSystem;

public class HumanToZombie : MonoBehaviour
{
    public GameObject zombiePrefab; // Assign in the Inspector
    public InputActionAsset inputActions; // Will store our created InputActionAsset

    private void OnTriggerEnter(Collider other)
    {
        print("triggered");
        if (other.CompareTag("Zombie")) // Check if collided with a zombie
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


            // Add PlayerInput component to the new zombie
            PlayerInput playerInput = newZombie.AddComponent<PlayerInput>();

            // Assign the InputActionAsset to the PlayerInput component
            playerInput.actions = inputActions; // Link our programmatically created InputActionAsset
            playerInput.defaultActionMap = "Player"; // Make sure this matches the action map name

            // Move the camera to the new zombie
            cameraTransform.SetParent(newZombie.transform);
            cameraTransform.localPosition = new Vector3(0, 1.5f, 0); // Adjust camera position



            // Add PlayerMovement script to the new zombie
            PlayerMovement newMovement = newZombie.AddComponent<PlayerMovement>();
            newMovement.controller = newController;

            // Ensure all necessary values from old script are passed to new zombie
            newMovement.moveSpeed = oldMovementScript.moveSpeed;  // Assuming speed is a field in the movement script
            newMovement.rotationSpeed = oldMovementScript.rotationSpeed; // Assuming rotationSpeed is a field
            newMovement.playerInput = playerInput;
            // Destroy the human AFTER transferring everything
            Destroy(gameObject);
        }
    }

    
}
