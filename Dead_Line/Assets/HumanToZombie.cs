using UnityEngine;

public class HumanToZombie : MonoBehaviour
{
    public GameObject zombiePrefab; // Assign the Zombie prefab in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie")) // Check if collided with a zombie
        {
            Transform humanTransform = transform; // Save position & rotation

            // Store the camera reference
            Camera mainCamera = Camera.main;
            Transform cameraTransform = mainCamera.transform;

            // Destroy the human
            Destroy(gameObject);

            // Instantiate the zombie at the same position
            GameObject newZombie = Instantiate(zombiePrefab, humanTransform.position, humanTransform.rotation);
            newZombie.transform.localScale = humanTransform.localScale; // Match scale

            // Move the camera to the new zombie
            cameraTransform.SetParent(newZombie.transform);
            cameraTransform.localPosition = new Vector3(0, 1.5f, -2.5f); // Adjust camera position relative to zombie
            cameraTransform.localRotation = Quaternion.identity; // Reset rotation if needed
        }
    }
}
