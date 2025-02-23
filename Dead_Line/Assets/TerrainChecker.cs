using UnityEngine;

public class TerrainChecker : MonoBehaviour
{
    private Terrain[] terrains;
    private bool terrainsInitialized = false;

    void Start()
    {
        Invoke("UpdateTerrainsList", 1f);  // Delay ensures terrains are properly created
    }

    void UpdateTerrainsList()
    {
        terrains = Object.FindObjectsByType<Terrain>(FindObjectsSortMode.None);

        if (terrains.Length == 0)
        {
            Debug.LogError("No terrains found in the scene! Retrying...");
            Invoke("UpdateTerrainsList", 1f); // Retry after 1 second if no terrains found
        }
        else
        {
            terrainsInitialized = true;
            Debug.Log("Terrains successfully found!");
        }
    }

    void Update()
    {
        if (!terrainsInitialized) return; // Don't run logic if terrains are not ready

        Vector3 playerPos = transform.position;
        foreach (Terrain t in terrains)
        {
            if (t == null) continue; // Skip destroyed terrains

            Vector3 terrainPos = t.transform.position;
            Vector3 terrainSize = t.terrainData.size;

            float minX = terrainPos.x;
            float maxX = terrainPos.x + terrainSize.x;
            float minZ = terrainPos.z;
            float maxZ = terrainPos.z + terrainSize.z;

            if (playerPos.x >= minX && playerPos.x <= maxX &&
                playerPos.z >= minZ && playerPos.z <= maxZ)
            {
                Debug.Log("Player is on " + t.name);
            }
        }
    }
}
