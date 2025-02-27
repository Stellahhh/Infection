// Linda Fan, Stella Huo, Hanbei Zhou
using System.Collections;
using UnityEngine;

public class TerrainSplitter : MonoBehaviour
{
    public Terrain originalTerrain;
    private Terrain[] splitTerrains;
    private Material[] terrainMaterials; // Store individual materials for each terrain

    void Start()
    {
        SplitTerrainIntoFour();
        StartCoroutine(ChangeTerrainColors()); // Start the color-changing coroutine
    }

    public void SplitTerrainIntoFour()
    {
        if (originalTerrain == null)
        {
            Debug.LogError("Original terrain reference is null!");
            return;
        }

        TerrainData originalData = originalTerrain.terrainData;
        int originalRes = originalData.heightmapResolution;
        int newRes = (originalRes - 1) / 2 + 1;

        float[,] originalHeights = originalData.GetHeights(0, 0, originalRes, originalRes);

        float[,] heightsBL = new float[newRes, newRes];
        float[,] heightsBR = new float[newRes, newRes];
        float[,] heightsTL = new float[newRes, newRes];
        float[,] heightsTR = new float[newRes, newRes];

        for (int y = 0; y < newRes; y++)
        {
            for (int x = 0; x < newRes; x++)
            {
                heightsBL[y, x] = originalHeights[y, x];
                heightsBR[y, x] = originalHeights[y, x + newRes - 1];
                heightsTL[y, x] = originalHeights[y + newRes - 1, x];
                heightsTR[y, x] = originalHeights[y + newRes - 1, x + newRes - 1];
            }
        }

        Vector3 originalSize = originalData.size;
        Vector3 newSize = new Vector3(originalSize.x / 2f, originalSize.y, originalSize.z / 2f);

        splitTerrains = new Terrain[4];
        terrainMaterials = new Material[4];

        splitTerrains[0] = CreateTerrain("Terrain_BL", heightsBL, newSize, originalTerrain.transform.position, 0);
        splitTerrains[1] = CreateTerrain("Terrain_BR", heightsBR, newSize, originalTerrain.transform.position + new Vector3(newSize.x, 0, 0), 1);
        splitTerrains[2] = CreateTerrain("Terrain_TL", heightsTL, newSize, originalTerrain.transform.position + new Vector3(0, 0, newSize.z), 2);
        splitTerrains[3] = CreateTerrain("Terrain_TR", heightsTR, newSize, originalTerrain.transform.position + new Vector3(newSize.x, 0, newSize.z), 3);

        // Destroy the original terrain safely
        GameObject originalTerrainGO = originalTerrain.gameObject;
        originalTerrain = null; // Clear reference before destroying
        Destroy(originalTerrainGO);
    }

    private Terrain CreateTerrain(string name, float[,] heights, Vector3 size, Vector3 position, int index)
    {
        TerrainData newData = new TerrainData();
        newData.heightmapResolution = heights.GetLength(0);
        newData.size = size;
        newData.SetHeights(0, 0, heights);

        GameObject terrainGO = Terrain.CreateTerrainGameObject(newData);
        terrainGO.name = name;
        terrainGO.transform.position = position;

        // Load the base TerrainMaterial from Resources
        Material baseMaterial = Resources.Load<Material>("TerrainMaterial");

        if (baseMaterial == null)
        {
            Debug.LogError("TerrainMaterial not found! Make sure you placed it in a Resources folder.");
            return terrainGO.GetComponent<Terrain>();
        }

        // 🔥 Create a unique instance of the material for this terrain
        Material terrainMaterial = new Material(baseMaterial);

        terrainGO.GetComponent<Terrain>().materialTemplate = terrainMaterial;
        terrainGO.GetComponent<Terrain>().Flush(); // Force update rendering

        // Store material instance for color changes
        terrainMaterials[index] = terrainMaterial;

        return terrainGO.GetComponent<Terrain>();
    }




    private IEnumerator ChangeTerrainColors()
    {
        Debug.Log("ChangeTerrainColors...");

        while (true)
        {
            yield return new WaitForSeconds(5f); // Wait 5 seconds

            if (splitTerrains == null || splitTerrains.Length < 4)
            {
                Debug.LogError("No split terrains found! Exiting color change coroutine.");
                yield break;
            }

            int randomIndex = Random.Range(0, 4); // Select a random terrain
            Debug.Log($"Select a random terrain: {randomIndex}");

            for (int i = 0; i < splitTerrains.Length; i++)
            {
                if (splitTerrains[i] != null && terrainMaterials[i] != null)
                {
                    // ✅ Modify the unique material instance
                    Color newColor = (i == randomIndex) ? Color.red : Color.green;
                    terrainMaterials[i].SetColor("_BaseColor", newColor); // URP Shader
                    terrainMaterials[i].SetColor("_Color", newColor); // Standard Shader

                    Debug.Log($"{splitTerrains[i].name} is now {(i == randomIndex ? "RED" : "GREEN")}");
                }
            }
        }
    }


}
