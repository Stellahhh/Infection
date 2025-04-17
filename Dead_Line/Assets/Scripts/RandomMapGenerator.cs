// Linda Fan <yfan43@jhu.edu>, Stella Huo <shuo2@jhu.edu>, Hanbei Zhou <hzhou43@jhu.edu>
// Generate random map at the beginning of each game
using UnityEngine;
using System.Collections.Generic;

public class RandomMapGenerator : MonoBehaviour
{
    public List<GameObject> locationPrefabs; // Assign in Unity Inspector
    public Vector3 startPosition = Vector3.zero; // Starting position of the grid
    public float tileSize = 10f; // Adjust based on prefab size

    private void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        if (locationPrefabs.Count < 9)
        {
            Debug.LogError("Not enough prefabs! Add at least 9 location prefabs.");
            return;
        }

        // ✅ Shuffle the list and pick 9 random prefabs
        List<GameObject> shuffledLocations = new List<GameObject>(locationPrefabs);
        ShuffleList(shuffledLocations);
        Debug.Log("list is shuffled");
        List<GameObject> selectedLocations = shuffledLocations.GetRange(0, 9);

        // ✅ Generate a 3x3 grid
        for (int x = 0; x < 3; x++)
        {
            for (int z = 0; z < 3; z++)
            {
                // Calculate position based on grid layout
                Vector3 spawnPosition = startPosition + new Vector3(x * tileSize, 0, z * tileSize);

                // Pick a random prefab
                GameObject locationPrefab = selectedLocations[x * 3 + z];

                // Spawn location
                Instantiate(locationPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }

    // ✅ Helper Function: Shuffle List (Fisher-Yates Shuffle)
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
