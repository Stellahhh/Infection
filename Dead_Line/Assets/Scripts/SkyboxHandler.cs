using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxHandler : MonoBehaviour
{
    public Material skyboxMaterial; // Assign skybox material in inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Human") || other.CompareTag("Zombie"))
        {
            // print("new skybox triggered by human");
            // RenderSettings.skybox = skyboxMaterial;
            // DynamicGI.UpdateEnvironment(); // Ensures lighting updates
            Debug.Log("Skybox zone triggered by: " + other.name);

            // Get the player's camera
            Camera playerCamera = other.GetComponentInChildren<Camera>();
            if (playerCamera != null)
            {
                Skybox camSkybox = playerCamera.GetComponent<Skybox>();
                if (camSkybox == null)
                {
                    camSkybox = playerCamera.gameObject.AddComponent<Skybox>();
                }

                camSkybox.material = skyboxMaterial;
                DynamicGI.UpdateEnvironment(); // Optional: only needed if lighting needs update
            }
        }
    }
}
