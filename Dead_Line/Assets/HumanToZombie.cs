using UnityEngine;

public class HumanToZombie : MonoBehaviour
{
    public GameObject zombiePrefab; // Assign in the Inspector
    public MonoBehaviour playerMovementScript; // Drag your movement script here

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie")) // Ensure collision is with a zombie
        {
            Transform humanTransform = transform; // Store the position & rotation

            // Find the main camera
            Camera mainCamera = Camera.main;
            Transform cameraTransform = mainCamera.transform;

            // Instantiate the new zombie
            GameObject newZombie = Instantiate(zombiePrefab, humanTransform.position, humanTransform.rotation);
            newZombie.transform.localScale = humanTransform.localScale;

            // Move the camera to the new zombie
            cameraTransform.SetParent(newZombie.transform);
            cameraTransform.localPosition = new Vector3(0, 1.5f, -2.5f); // Adjust camera position
            cameraTransform.localRotation = Quaternion.identity;

            // Transfer movement script
            if (playerMovementScript != null)
            {
                MonoBehaviour newMovement = newZombie.AddComponent(playerMovementScript.GetType()) as MonoBehaviour;
            }

            // Destroy the human AFTER transferring everything
            Destroy(gameObject);
        }
    }
}
