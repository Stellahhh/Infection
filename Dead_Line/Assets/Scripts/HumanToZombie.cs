using UnityEngine;
using Mirror;
using UnityEngine.InputSystem; // Import New Input System

public class PlayerSwitch : NetworkBehaviour
{
    public GameObject prefabA;
    public GameObject prefabB;

    private InputAction switchAction;

    public override void OnStartAuthority()
    {
        var playerInput = new InputAction("SwitchPlayer", binding: "<Keyboard>/space");
        playerInput.performed += ctx => CmdSwitchPrefab(); // Trigger switch
        playerInput.Enable();
        switchAction = playerInput;
    }

    public override void OnStopAuthority()
    {
        switchAction.Disable(); // Prevent input when player is destroyed
    }

    [Command]
    void CmdSwitchPrefab()
    {
        print("switching...");
        GameObject newPrefab = (gameObject.name.Contains("A")) ? prefabB : prefabA;
        Transform spawnPosition = transform; // Keep current position

        GameObject newPlayer = Instantiate(newPrefab, spawnPosition.position, spawnPosition.rotation);
        NetworkServer.ReplacePlayerForConnection(connectionToClient, newPlayer, true);
    }
}
