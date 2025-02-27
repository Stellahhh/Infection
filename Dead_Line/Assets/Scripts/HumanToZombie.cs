// Linda Fan, Stella Huo, Hanbei Zhou
using UnityEngine;
using Mirror;

public class HumanToZombie : NetworkBehaviour
{
    public Material zombieMaterial;
    private MeshRenderer meshRenderer;

    //[SyncVar(hook = nameof(OnColorChanged))] // Syncs color changes
    private Color zombieColor;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    [Command] // Runs on the server
    void CmdTurnIntoZombie()
    {
        if (!isServer) return;

        // Change color (syncs to all clients)
        zombieColor = zombieMaterial.color;

        // Change layer and tag
        gameObject.layer = LayerMask.NameToLayer("Zombie");
        gameObject.tag = "Zombie";

        // Change name
        gameObject.name = "Zombie " + gameObject.name.Substring(6);

        // Add new component and remove old one
        Hunger hunger = gameObject.AddComponent<Hunger>();
        hunger.maxTime = 100;
        Destroy(GetComponent<Life>());
    }

    // Hook function: runs on clients when `zombieColor` changes
    //void OnColorChanged(Color oldColor, Color newColor)
    //{
    //    if (meshRenderer != null)
    //    {
    //        meshRenderer.material.color = newColor;
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie") && isLocalPlayer)
        {
            CmdTurnIntoZombie(); // Tell the server to change the player's appearance
        }
    }
}
