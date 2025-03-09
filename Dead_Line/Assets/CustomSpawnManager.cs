using Mirror;
using UnityEngine;

public class CustomSpawnManager : NetworkManager
{
    public Vector3 spawnCenter = Vector3.zero; // Center of the spawn area
    public float spawnRange = 10; // Half-width of the spawn area

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnCenter.x - spawnRange, spawnCenter.x + spawnRange),
            spawnCenter.y,
            Random.Range(spawnCenter.z - spawnRange, spawnCenter.z + spawnRange)
        );

        GameObject player = Instantiate(playerPrefab, randomPosition, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player);
    }
}
