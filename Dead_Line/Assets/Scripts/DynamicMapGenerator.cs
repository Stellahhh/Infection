// Linda Fan, Stella Huo, Hanbei Zhou

using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class DynamicMapGenerator : NetworkBehaviour
{
    public List<GameObject> locationPrefabs; // Assign in Unity Inspector
    public Vector3 startPosition = Vector3.zero; // Starting position for the grid

    [SyncVar] private int seed; // Seed to synchronize map layout

    private bool mapGenerated = false;

    private void Start()
    {
        if (isServer) // Only the server (host) generates the map
        {
            seed = Random.Range(0, 100000); // Generate a random seed
            GenerateMap(seed);
        }
        else
        {
            CmdRequestMap(); // Clients request the map seed from the server
        }
    }

    // ✅ Clients Request Map from Server
    [Command(requiresAuthority = false)]
    void CmdRequestMap(NetworkConnectionToClient conn = null)
    {
        TargetReceiveSeed(conn, seed);
    }

    // ✅ Server Sends the Seed to the Client
    [TargetRpc]
    void TargetReceiveSeed(NetworkConnectionToClient conn, int receivedSeed)
    {
        if (!mapGenerated)
        {
            seed = receivedSeed;
            GenerateMap(seed);
        }
    }

    // ✅ Generate the Map Based on the Given Seed
    void GenerateMap(int seed)
    {
        if (mapGenerated) return; // Prevent duplicate generation
        mapGenerated = true;

        Random.InitState(seed); // Set the seed so all clients generate the same map

        if (locationPrefabs.Count < 9)
        {
            Debug.LogError("Not enough prefabs! Add at least 9 location prefabs.");
            return;
        }

        List<GameObject> shuffledLocations = new List<GameObject>(locationPrefabs);
        ShuffleList(shuffledLocations);
        List<GameObject> selectedLocations = shuffledLocations.GetRange(0, 9);

        float rowStartX = startPosition.x;
        float currentZ = startPosition.z;

        for (int x = 0; x < 3; x++)
        {
            float maxRowHeight = 0f;
            float currentX = rowStartX;

            for (int z = 0; z < 3; z++)
            {
                GameObject locationPrefab = selectedLocations[x * 3 + z];

                GameObject spawnedObject = Instantiate(locationPrefab, Vector3.zero, Quaternion.identity);

                Bounds bounds = GetMeshBounds(spawnedObject);
                Vector3 bottomLeft = bounds.min;
                Vector3 correctPosition = new Vector3(currentX - bottomLeft.x, 0, currentZ - bottomLeft.z);
                spawnedObject.transform.position = correctPosition;

                NetworkServer.Spawn(spawnedObject); // ✅ Ensure network sync

                currentX += bounds.size.x;
                if (bounds.size.z > maxRowHeight)
                {
                    maxRowHeight = bounds.size.z;
                }
            }

            currentZ += maxRowHeight;
        }
    }

    // ✅ Get Mesh Bounds Instead of Renderer Bounds
    Bounds GetMeshBounds(GameObject obj)
    {
        MeshFilter[] meshFilters = obj.GetComponentsInChildren<MeshFilter>();
        if (meshFilters.Length == 0) return new Bounds(obj.transform.position, Vector3.zero);

        Bounds bounds = meshFilters[0].sharedMesh.bounds;
        foreach (MeshFilter mf in meshFilters)
        {
            bounds.Encapsulate(mf.sharedMesh.bounds);
        }
        return bounds;
    }

    // ✅ Fisher-Yates Shuffle for Consistent Randomization
    void ShuffleList(List<GameObject> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            GameObject temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
