using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class TerrainSplitter : MonoBehaviour
{
    public Terrain originalTerrain;
    private Terrain[] splitTerrains;
    private Material[] terrainMaterials;

    void Start()
    {
        // Split the terrain into four
        SplitTerrainIntoFour();

        // Start color-changing coroutine
        StartCoroutine(ChangeTerrainColors());
    }

    public void SplitTerrainIntoFour()
    {
        if (originalTerrain == null)
        {
            Debug.LogError("❌ Original terrain reference is null!");
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
        originalTerrain = null;
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

        // Load the base TerrainMaterial
        Material baseMaterial = Resources.Load<Material>("TerrainMaterial");
        if (baseMaterial == null)
        {
            Debug.LogError("❌ TerrainMaterial not found! Make sure it's inside a Resources folder.");
            return terrainGO.GetComponent<Terrain>();
        }

        // Create a unique material instance
        Material terrainMaterial = new Material(baseMaterial);
        Terrain terrainComponent = terrainGO.GetComponent<Terrain>();
        terrainComponent.materialTemplate = terrainMaterial;
        terrainComponent.Flush();

        terrainMaterials[index] = terrainMaterial;

        // Add NavMeshSurface
        AddNavMeshSurface(terrainGO);

        return terrainComponent;
    }

    private void AddNavMeshSurface(GameObject terrainObject)
    {
        NavMeshSurface navMeshSurface = terrainObject.AddComponent<NavMeshSurface>();
        navMeshSurface.agentTypeID = 0;
        navMeshSurface.collectObjects = CollectObjects.All;
        navMeshSurface.useGeometry = NavMeshCollectGeometry.RenderMeshes;
        navMeshSurface.overrideVoxelSize = true;
        navMeshSurface.voxelSize = 0.2f;
        navMeshSurface.overrideTileSize = true;
        navMeshSurface.tileSize = 32;

        navMeshSurface.BuildNavMesh();
    }

    private IEnumerator ChangeTerrainColors()
    {
        Debug.Log("🔄 ChangeTerrainColors started...");
        while (true)
        {
            yield return new WaitForSeconds(5f); // Every 5 seconds

            int randomIndex = Random.Range(0, 4);
            Terrain redTerrain = splitTerrains[randomIndex];

            for (int i = 0; i < splitTerrains.Length; i++)
            {
                if (splitTerrains[i] != null && terrainMaterials[i] != null)
                {
                    Color newColor = (i == randomIndex) ? Color.red : Color.green;
                    terrainMaterials[i].SetColor("_BaseColor", newColor);
                    terrainMaterials[i].SetColor("_Color", newColor);
                }
            }

            Debug.Log($"🟥 {redTerrain.name} turned RED. Checking for characters to destroy...");
            DestroyCharactersOnTerrain(redTerrain);
        }
    }

    private void DestroyCharactersOnTerrain(Terrain terrain)
    {
        if (terrain == null) return;

        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
        GameObject[] humans = GameObject.FindGameObjectsWithTag("Human");

        List<GameObject> toDestroy = new List<GameObject>();

        foreach (GameObject zombie in zombies)
        {
            if (IsObjectOnTerrain(zombie, terrain))
            {
                toDestroy.Add(zombie);
            }
        }

        foreach (GameObject human in humans)
        {
            if (IsObjectOnTerrain(human, terrain))
            {
                toDestroy.Add(human);
            }
        }

        if (toDestroy.Count == 0)
        {
            Debug.Log($"⚠️ No objects found on {terrain.name}, nothing to destroy!");
            return;
        }

        foreach (GameObject obj in toDestroy)
        {
            Debug.Log($"🔥 Destroying {obj.name} on {terrain.name}");
            Destroy(obj);
        }
    }

    // private bool IsObjectOnTerrain(GameObject obj, Terrain terrain)
    // {
    //     if (terrain == null || obj == null) return false;

    //     Vector3 objPosition = obj.transform.position;
    //     Vector3 terrainPosition = terrain.transform.position;
    //     TerrainData terrainData = terrain.terrainData;

    //     float terrainWidth = terrainData.size.x;
    //     float terrainLength = terrainData.size.z;

    //     if (objPosition.x < terrainPosition.x || objPosition.x > terrainPosition.x + terrainWidth ||
    //         objPosition.z < terrainPosition.z || objPosition.z > terrainPosition.z + terrainLength)
    //     {
    //         return false;
    //     }

    //     float terrainHeightAtPoint = terrain.SampleHeight(objPosition) + terrainPosition.y;
    //     return Mathf.Abs(objPosition.y - terrainHeightAtPoint) < 1.5f;
    // }
    private bool IsObjectOnTerrain(GameObject obj, Terrain terrain)
    {
        if (terrain == null || obj == null)
        {
            Debug.LogError("❌ IsObjectOnTerrain called with NULL parameters!");
            return false;
        }

        Vector3 objPosition = obj.transform.position;
        Vector3 terrainPosition = terrain.transform.position;
        TerrainData terrainData = terrain.terrainData;

        float terrainWidth = terrainData.size.x;
        float terrainLength = terrainData.size.z;

        bool insideX = (objPosition.x >= terrainPosition.x && objPosition.x <= terrainPosition.x + terrainWidth);
        bool insideZ = (objPosition.z >= terrainPosition.z && objPosition.z <= terrainPosition.z + terrainLength);

        float terrainHeightAtPoint = terrain.SampleHeight(objPosition) + terrainPosition.y;
        float heightDifference = Mathf.Abs(objPosition.y - terrainHeightAtPoint);

        bool withinHeight = heightDifference < 1.5f; // Allow small margin

        // Print debug info for each object
        Debug.Log($"🔎 Checking {obj.name} at {objPosition} inside {terrain.name} " +
                  $"| Inside X: {insideX} | Inside Z: {insideZ} | Height OK: {withinHeight}");

        // return insideX && insideZ && withinHeight;
        return insideX && insideZ;

    }

}
