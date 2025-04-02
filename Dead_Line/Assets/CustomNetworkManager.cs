using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    public GameObject prefabA; // 10% chance
    public GameObject prefabB; // 90% chance
    public Vector3 spawnCenter = new Vector3(67.5f, 10f, 67.5f); // Center of the spawn area
    public float spawnRange = 60; // Half-width of the spawn area

    private int totalPlayers = 0; // Track total players

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // Decide which prefab to spawn
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnCenter.x - spawnRange, spawnCenter.x + spawnRange),
            spawnCenter.y,
            Random.Range(spawnCenter.z - spawnRange, spawnCenter.z + spawnRange)
        );

        print(randomPosition);
        GameObject chosenPrefab = (Random.value < 1f) ? prefabB : prefabA;

        // Instantiate and spawn the player
        GameObject player = Instantiate(chosenPrefab, randomPosition, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player);

        totalPlayers++; // Increment count
    }
    
}
