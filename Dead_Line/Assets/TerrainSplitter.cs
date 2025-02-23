using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSplitter : MonoBehaviour
{
    public Terrain originalTerrain;

    void Start()
    {
        SplitTerrainIntoFour();
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

        CreateTerrain("Terrain_BL", heightsBL, newSize, originalTerrain.transform.position);
        CreateTerrain("Terrain_BR", heightsBR, newSize, originalTerrain.transform.position + new Vector3(newSize.x, 0, 0));
        CreateTerrain("Terrain_TL", heightsTL, newSize, originalTerrain.transform.position + new Vector3(0, 0, newSize.z));
        CreateTerrain("Terrain_TR", heightsTR, newSize, originalTerrain.transform.position + new Vector3(newSize.x, 0, newSize.z));

        // Destroy the original terrain safely
        GameObject originalTerrainGO = originalTerrain.gameObject;
        originalTerrain = null; // Clear reference before destroying
        Destroy(originalTerrainGO);
    }

    private void CreateTerrain(string name, float[,] heights, Vector3 size, Vector3 position)
    {
        TerrainData newData = new TerrainData();
        newData.heightmapResolution = heights.GetLength(0);
        newData.size = size;
        newData.SetHeights(0, 0, heights);

        GameObject terrainGO = Terrain.CreateTerrainGameObject(newData);
        terrainGO.name = name;
        terrainGO.transform.position = position;
    }
}
