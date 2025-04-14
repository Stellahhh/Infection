using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    public GameObject human_prefab; // 10% chance
    public GameObject zombie_prefab; // 90% chance
    public Vector3 spawnCenter = new Vector3(67.5f, 10f, 67.5f); // Center of the spawn area
    public float zombieProportion = 0.1f;
    public float spawnRange = 60; // Half-width of the spawn area

    private int totalPlayers = 0; // Track total players

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        

        Vector3 randomPosition = new Vector3(
            Random.Range(spawnCenter.x - spawnRange, spawnCenter.x + spawnRange),
            spawnCenter.y,
            Random.Range(spawnCenter.z - spawnRange, spawnCenter.z + spawnRange)
        );
        GameObject chosenPrefab = (Random.value < zombieProportion) ? human_prefab : zombie_prefab;

        // Instantiate and spawn the player
        GameObject player = Instantiate(chosenPrefab, randomPosition, Quaternion.identity);
        player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
        NetworkServer.AddPlayerForConnection(conn, player);

        totalPlayers++; // Increment count
    }



    
    
}
