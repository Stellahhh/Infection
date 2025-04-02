using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxHandler : MonoBehaviour
{
    public Material skyboxMaterial; // Assign skybox material in inspector

    private void OnTriggerEnter(Collider other)
    {
        print("triggered by " + other.gameObject.name);
        if (other.CompareTag("Human"))
        {
            print("new skybox triggered by human");
            RenderSettings.skybox = skyboxMaterial;
            DynamicGI.UpdateEnvironment(); // Ensures lighting updates
        }
    }
}
