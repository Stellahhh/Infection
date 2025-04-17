// Linda Fan <yfan43@jhu.edu>, Stella Huo <shuo2@jhu.edu>, Hanbei Zhou <hzhou43@jhu.edu>
// Handle the skybox changing at different map blocks

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxHandler : MonoBehaviour
{
    public Material skyboxMaterial; // Assign default skybox for this region in inspector

    private void OnTriggerEnter(Collider other)
    {
        // Only respond to player types
        if (other.CompareTag("Human") || other.CompareTag("Zombie"))
        {
            Debug.Log("Skybox zone triggered by: " + other.name);

            // Get the player's camera component
            Camera playerCamera = other.GetComponentInChildren<Camera>();
            if (playerCamera != null)
            {
                // Get or add Skybox component to the camera
                Skybox camSkybox = playerCamera.GetComponent<Skybox>();
                if (camSkybox == null)
                {
                    camSkybox = playerCamera.gameObject.AddComponent<Skybox>();
                }

                // Check if this tile is currently disabled
                bool isDisabled = DynamicMapGenerator.Instance.DisabledTilePositions.Contains(transform.position);

                // Choose the appropriate material
                Material chosenSkybox;
                if (isDisabled)
                {
                    // Load fallback "Skybox" from Resources folder
                    chosenSkybox = Resources.Load<Material>("Skybox");
                    if (chosenSkybox == null)
                    {
                        Debug.LogWarning("Fallback skybox 'Skybox' not found in Resources.");
                        return;
                    }
                }
                else
                {
                    chosenSkybox = skyboxMaterial;
                }

                // Assign the material to the camera skybox
                camSkybox.material = chosenSkybox;

                // Optionally update lighting if environment needs it
                DynamicGI.UpdateEnvironment();
            }
        }
    }
}
