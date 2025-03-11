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

        // Start positioning
        float rowStartX = startPosition.x;
        float currentZ = startPosition.z;

        for (int x = 0; x < 3; x++)
        {
            float maxRowHeight = 0f; // Track tallest prefab in row
            float currentX = rowStartX;

            for (int z = 0; z < 3; z++)
            {
                // Pick a prefab
                GameObject locationPrefab = selectedLocations[x * 3 + z];

                // Instantiate at the calculated position
                GameObject spawnedObject = Instantiate(locationPrefab, Vector3.zero, Quaternion.identity);

                // ✅ Correct Position to Align Prefabs Properly
                Bounds bounds = GetMeshBounds(spawnedObject);
                Vector3 bottomLeft = bounds.min;  // Get bottom-left of prefab
                Vector3 correctPosition = new Vector3(currentX - bottomLeft.x, 0, currentZ - bottomLeft.z);
                spawnedObject.transform.position = correctPosition;

                // ✅ Move next object in the row to align to the right edge of the current prefab
                currentX += bounds.size.x;

                // Track the tallest prefab for row height
                if (bounds.size.z > maxRowHeight)
                {
                    maxRowHeight = bounds.size.z;
                }
            }

            // ✅ Move to the next row using the tallest prefab's height
            currentZ += maxRowHeight;
        }
    }

    // ✅ Helper Function: Get Mesh Bounds Instead of Renderer Bounds
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
