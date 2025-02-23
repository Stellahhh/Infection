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
        TerrainData originalData = originalTerrain.terrainData;
        int originalRes = originalData.heightmapResolution;
        // Calculate new resolution for each quadrant. 
        // Because heightmap resolution is (2^n)+1, splitting in half can be tricky.
        // For example, if originalRes is 513, then newRes will be 257.
        int newRes = (originalRes - 1) / 2 + 1;

        // Retrieve the full heightmap
        float[,] originalHeights = originalData.GetHeights(0, 0, originalRes, originalRes);

        // Prepare height arrays for each quadrant
        float[,] heightsBL = new float[newRes, newRes]; // Bottom Left
        float[,] heightsBR = new float[newRes, newRes]; // Bottom Right
        float[,] heightsTL = new float[newRes, newRes]; // Top Left
        float[,] heightsTR = new float[newRes, newRes]; // Top Right

        // Fill the quadrant height arrays.
        for (int y = 0; y < newRes; y++)
        {
            for (int x = 0; x < newRes; x++)
            {
                // Bottom Left quadrant: starting at (0,0)
                heightsBL[y, x] = originalHeights[y, x];

                // Bottom Right quadrant: offset x by (newRes - 1)
                heightsBR[y, x] = originalHeights[y, x + newRes - 1];

                // Top Left quadrant: offset y by (newRes - 1)
                heightsTL[y, x] = originalHeights[y + newRes - 1, x];

                // Top Right quadrant: offset both x and y by (newRes - 1)
                heightsTR[y, x] = originalHeights[y + newRes - 1, x + newRes - 1];
            }
        }

        // Define the size for the new terrains (half the original width and length)
        Vector3 originalSize = originalData.size;
        Vector3 newSize = new Vector3(originalSize.x / 2f, originalSize.y, originalSize.z / 2f);

        // Create and configure new TerrainData objects for each quadrant.
        CreateTerrain("Terrain_BL", heightsBL, newSize, originalTerrain.transform.position);
        CreateTerrain("Terrain_BR", heightsBR, newSize, originalTerrain.transform.position + new Vector3(newSize.x, 0, 0));
        CreateTerrain("Terrain_TL", heightsTL, newSize, originalTerrain.transform.position + new Vector3(0, 0, newSize.z));
        CreateTerrain("Terrain_TR", heightsTR, newSize, originalTerrain.transform.position + new Vector3(newSize.x, 0, newSize.z));
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
