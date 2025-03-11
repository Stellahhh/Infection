// Linda Fan, Stella Huo, Hanbei Zhou

using UnityEngine;
using System.Collections.Generic;

public class DynamicMapGenerator : MonoBehaviour
{
    public List<GameObject> locationPrefabs; // Assign in Unity Inspector
    public Vector3 startPosition = Vector3.zero; // Starting position for the grid

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
        List<GameObject> selectedLocations = shuffledLocations.GetRange(0, 9);

        // Store size information
        Vector3[,] prefabSizes = new Vector3[3, 3];

        // ✅ Generate a 3x3 grid while aligning prefabs dynamically
        Vector3 lastPos = startPosition;

        for (int x = 0; x < 3; x++)
        {
            for (int z = 0; z < 3; z++)
            {
                // Pick a random prefab
                GameObject locationPrefab = selectedLocations[x * 3 + z];

                // Instantiate to get its real-world size
                GameObject spawnedObject = Instantiate(locationPrefab, lastPos, Quaternion.identity);

                // Get size from Renderer bounds
                Bounds bounds = GetBounds(spawnedObject);
                Vector3 prefabSize = bounds.size;

                prefabSizes[x, z] = prefabSize;

                // Adjust the next position based on current prefab size
                if (z < 2) lastPos.z += prefabSize.z;
            }

            // Move to the next row and reset Z position
            if (x < 2) lastPos = new Vector3(lastPos.x + prefabSizes[x, 0].x, 0, startPosition.z);
        }
    }

    // ✅ Helper Function: Get Object Bounds
    Bounds GetBounds(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return new Bounds(obj.transform.position, Vector3.zero);

        Bounds bounds = renderers[0].bounds;
        foreach (Renderer r in renderers)
        {
            bounds.Encapsulate(r.bounds);
        }
        return bounds;
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
