using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class HumanToZombie : NetworkBehaviour
{
    public GameObject zombiePrefab; // Assign in the Inspector
    public InputActionAsset inputActions; // Will store our created InputActionAsset

    [Command] // Runs this on the server
    public void CmdTurnIntoZombie()
    {
        if (!isServer) return;

        Transform humanTransform = transform; // Store the position & rotation

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
        playerInput.defaultActionMap = "Player"; // Ensure this matches the action map name

        // Add PlayerMovement script to the new zombie
        PlayerMovement newMovement = newZombie.AddComponent<PlayerMovement>();
        newMovement.controller = newController;

        // Ensure all necessary values from old script are passed to new zombie
        newMovement.moveSpeed = oldMovementScript.moveSpeed;  // Assuming speed is a field in the movement script
        newMovement.rotationSpeed = oldMovementScript.rotationSpeed; // Assuming rotationSpeed is a field
        newMovement.playerInput = playerInput;

        // Spawn the new zombie object across the network
        NetworkServer.Spawn(newZombie);

        // Move the camera to the new zombie (after spawning)
        Camera mainCamera = Camera.main;
        Transform cameraTransform = mainCamera.transform;
        cameraTransform.SetParent(newZombie.transform);
        cameraTransform.localPosition = new Vector3(0, 1.5f, 0); // Adjust camera position

        // Destroy the human object on all clients
        NetworkServer.Destroy(gameObject);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Zombie") && isLocalPlayer)
    //    {
    //        CmdTurnIntoZombie(); // Trigger the transformation if the player collides with a zombie
    //    }
    //}
}
