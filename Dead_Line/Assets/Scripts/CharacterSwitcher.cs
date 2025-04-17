// Linda Fan <yfan43@jhu.edu>, Stella Huo <shuo2@jhu.edu>, Hanbei Zhou <hzhou43@jhu.edu>
// Handle the switch from human to zombie when collide

using Mirror;
using UnityEngine;

public class CharacterSwitcher : NetworkBehaviour
{
    public GameObject humanPrefab;
    public GameObject zombiePrefab;

    private GameObject currentPlayer;

    void Start()
    {
        if (isLocalPlayer)
        {
            // Instantiate the initial player prefab (e.g., human)
            currentPlayer = Instantiate(humanPrefab, transform.position, transform.rotation);
            NetworkServer.AddPlayerForConnection(connectionToClient, currentPlayer);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //if collide with zombie
        if (isLocalPlayer && other.CompareTag("Zombie"))
        {
            SwitchCharacter();
        }
    }

    void SwitchCharacter()
    {
        print("switching");
        if (currentPlayer != null)
        {
            // Remove the current player object
            NetworkServer.Destroy(currentPlayer);
        }

        // Instantiate the zombie prefab
        currentPlayer = Instantiate(zombiePrefab, transform.position, transform.rotation);
        NetworkServer.AddPlayerForConnection(connectionToClient, currentPlayer);
    }
}
