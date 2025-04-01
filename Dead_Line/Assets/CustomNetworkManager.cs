using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    public GameObject prefabA; // 10% chance
    public GameObject prefabB; // 90% chance

    private int totalPlayers = 0; // Track total players

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // Decide which prefab to spawn
        
        GameObject chosenPrefab = (Random.value < 0.001f) ? prefabB : prefabA;

        // Instantiate and spawn the player
        GameObject player = Instantiate(chosenPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);

        totalPlayers++; // Increment count
    }
}
