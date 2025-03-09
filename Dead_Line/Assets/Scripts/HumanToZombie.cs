using UnityEngine;
using Mirror;
using UnityEngine.InputSystem; // Import New Input System
using System.Collections;

public class PlayerSwitch : NetworkBehaviour
{
    public GameObject prefabA;
    public GameObject prefabB;
    public AudioSource audioSource;
    public AudioClip soundEffect;

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


    IEnumerator DelayedSwitch()
    {
        audioSource.volume = 1.0f;
        audioSource.PlayOneShot(soundEffect);
        yield return new WaitForSeconds(soundEffect.length); // Wait for sound to finish
        CmdSwitchPrefab();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isOwned) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Zombie"))
        {
            StartCoroutine(DelayedSwitch());
        }
    }
    [Command]
    void CmdSwitchPrefab()
    {
        GameObject newPrefab = (gameObject.name.Contains("puppet_kid")) ? prefabB : prefabA;
        Transform spawnPosition = transform; // Keep current position

        GameObject newPlayer = Instantiate(newPrefab, spawnPosition.position, spawnPosition.rotation);
        NetworkServer.ReplacePlayerForConnection(connectionToClient, newPlayer, true);
        NetworkServer.Destroy(gameObject);
    }
}
