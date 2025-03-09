using UnityEngine;
using Mirror;
using UnityEngine.InputSystem; // Import New Input System

public class PlayerSwitch : NetworkBehaviour
{
    public GameObject prefabA;
    public GameObject prefabB;

    //private InputAction switchAction;

    //public override void OnStartAuthority()
    //{
    //    var playerInput = new InputAction("SwitchPlayer", binding: "<Keyboard>/space");
    //    playerInput.performed += ctx => CmdSwitchPrefab(); // Trigger switch
    //    playerInput.Enable();
    //    switchAction = playerInput;
    //}

    //public override void OnStopAuthority()
    //{
    //    switchAction.Disable(); // Prevent input when player is destroyed
    //}


    private void OnTriggerEnter(Collider other)
    {
        if (!isOwned) return; // Ensure only the owner can trigger the switch

        if (other.gameObject.layer == LayerMask.NameToLayer("Zombie"))
        {
            CmdSwitchPrefab();
        }
    }

    [Command]
    void CmdSwitchPrefab()
    {
        print("switching...");
        GameObject newPrefab = (gameObject.name.Contains("puppet_kid")) ? prefabB : prefabA;
        Transform spawnPosition = transform; // Keep current position

        GameObject newPlayer = Instantiate(newPrefab, spawnPosition.position, spawnPosition.rotation);
        NetworkServer.ReplacePlayerForConnection(connectionToClient, newPlayer, true);
        NetworkServer.Destroy(gameObject);
    }
}
