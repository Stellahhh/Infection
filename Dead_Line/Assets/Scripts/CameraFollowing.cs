// Linda Fan, Stella Huo, Hanbei Zhou
using Mirror;
using UnityEngine;

public class CameraFollow : NetworkBehaviour
{
    public Camera playerCamera; // Assign in Inspector or find dynamically

    void Start()
    {
        if (!isLocalPlayer) return;

        // Find the Camera (assuming each player prefab has one)
        playerCamera = GetComponentInChildren<Camera>();
        
        // Activate only the local player's camera
        if (playerCamera != null)
        {
            playerCamera.gameObject.SetActive(true);
        }
    }
}
