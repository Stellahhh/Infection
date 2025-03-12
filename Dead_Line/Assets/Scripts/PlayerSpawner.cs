using UnityEngine;
using Mirror;

public class PlayerSpawn : NetworkBehaviour
{
    public Vector3 minSpawnPosition = new Vector3(10f, 10f, 10f);
    public Vector3 maxSpawnPosition = new Vector3(135, 10f, 135f);

    public override void OnStartLocalPlayer()
    {
        if (isLocalPlayer)
        {
            Vector3 randomSpawn = new Vector3(
                Random.Range(minSpawnPosition.x, maxSpawnPosition.x),
                Random.Range(minSpawnPosition.y, maxSpawnPosition.y),
                Random.Range(minSpawnPosition.z, maxSpawnPosition.z)
            );

            transform.position = randomSpawn;
        }
    }
}
