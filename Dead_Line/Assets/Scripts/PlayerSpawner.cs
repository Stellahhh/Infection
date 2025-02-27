// Linda Fan, Stella Huo, Hanbei Zhou
using Mirror;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    public GameObject playerPrefab;

    public void SpawnPlayer()
    {
        if (isServer)
        {
            GameObject player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(connectionToClient, player);
            NetworkIdentity playerIdentity = player.GetComponent<NetworkIdentity>();
            playerIdentity.AssignClientAuthority(connectionToClient);
        }
    }
}
