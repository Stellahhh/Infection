// Linda Fan <yfan43@jhu.edu>, Stella Huo <shuo2@jhu.edu>, Hanbei Zhou <hzhou43@jhu.edu>
// Generate random maps for each game

using UnityEngine.AI; 
using UnityEngine;
using Mirror;
using System.Collections;
using System.Collections.Generic;

// Responsible for procedurally generating and managing a 3x3 tile map on the server
public class DynamicMapGenerator : NetworkBehaviour
{
    // === PUBLIC SETTINGS ===
    public List<GameObject> locationPrefabs;
    public Vector3 startPosition = Vector3.zero;

    // === SYNCED STATE ===
    [SyncVar] private int seed;
    private bool mapGenerated = false;

    // === INTERNAL DATA STRUCTURES ===
    private List<GameObject> spawnedTiles = new List<GameObject>();
    private Dictionary<GameObject, string> tileNames = new Dictionary<GameObject, string>();
    private Dictionary<Vector3, Bounds> tileBoundsByPosition = new Dictionary<Vector3, Bounds>();

    // === GLOBAL ACCESS ===
    public static DynamicMapGenerator Instance;

    // === DANGER TILE TRACKING ===
    private HashSet<Vector3> disabledTilePositions = new HashSet<Vector3>();
    public HashSet<Vector3> DisabledTilePositions => disabledTilePositions;

    // === CONSTANT TILE DIMENSIONS (from BoxCollider) ===
    private const float TILE_HALF_WIDTH = 22.5f;
    private const float TILE_HALF_DEPTH = 22.5f;

    public GameObject cameraPrefab; // Assign a prefab with a Camera component in the Inspector
    public List<Camera> tileCameras = new List<Camera>(); // Store references for switching


    // === UNITY LIFECYCLE ===
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        if (isServer)
        {
            seed = Random.Range(0, 100000);
            GenerateMap(seed);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!isServer)
        {
            CmdRequestMap();
        }
    }

    // === NETWORK COMMANDS & RPCs ===

    [Command(requiresAuthority = false)]
    void CmdRequestMap(NetworkConnectionToClient conn = null)
    {
        if (conn != null)
        {
            TargetReceiveSeed(conn, seed);
        }
    }

    [TargetRpc]
    void TargetReceiveSeed(NetworkConnectionToClient conn, int receivedSeed)
    {
        if (!mapGenerated)
        {
            seed = receivedSeed;
            GenerateMap(seed);
        }
    }

    // === MAP GENERATION ===

    void GenerateMap(int seed)
    {
        if (mapGenerated) return;
        mapGenerated = true;

        Random.InitState(seed);

        if (locationPrefabs.Count < 9)
        {
            Debug.LogError("Not enough prefabs! Add at least 9 location prefabs.");
            return;
        }

        // Shuffle and pick 9 unique tiles
        List<GameObject> shuffledLocations = new List<GameObject>(locationPrefabs);
        ShuffleList(shuffledLocations);
        List<GameObject> selectedLocations = shuffledLocations.GetRange(0, 9);

        float rowStartX = startPosition.x;
        float currentZ = startPosition.z;

        // Place tiles in a 3x3 grid
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

                // Spawn a camera for this tile
                if (cameraPrefab != null)
                {
                    Vector3 camPos = correctPosition + new Vector3(0, 30, 0); // 30 units above the tile
                    GameObject camObj = Instantiate(cameraPrefab, camPos, Quaternion.Euler(90, 0, 0)); // Looking straight down
                    camObj.name = $"TileCamera_{x}_{z}";
                    camObj.SetActive(false); // All cameras off by default
                    Camera cam = camObj.GetComponent<Camera>();
                    if (cam != null)
                        tileCameras.Add(cam);
                }

                bounds.center += correctPosition;
                tileBoundsByPosition[correctPosition] = bounds;

                string tileName = $"Tile_{x}_{z}";
                spawnedObject.name = tileName;
                tileNames[spawnedObject] = tileName;

                spawnedTiles.Add(spawnedObject);
                NetworkServer.Spawn(spawnedObject);

                currentX += bounds.size.x;
                if (bounds.size.z > maxRowHeight)
                {
                    maxRowHeight = bounds.size.z;
                }
            }

            currentZ += maxRowHeight;
        }
        foreach (GameObject tile in spawnedTiles)
        {
            NavMeshSurface[] surfaces = tile.GetComponentsInChildren<NavMeshSurface>();
            foreach (NavMeshSurface surface in surfaces)
            {
                surface.BuildNavMesh();
            }
        }
        // Start danger zone scheduling
        if (isServer)
        {
            DisableRandomTile(); // Disable one immediately
            StartCoroutine(DisableTileRoutine()); // Continue disabling every 60s
        }
        
    }

    // === TILE DISABLING ===

    IEnumerator DisableTileRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f);
            DisableRandomTile();
        }
    }

    /*
    void DisableRandomTile()
    {
        if (spawnedTiles.Count == 0) return;

        GameObject tileToDisable = spawnedTiles[Random.Range(0, spawnedTiles.Count)];
        Vector3 pos = tileToDisable.transform.position;

        // Skip if already disabled
        if (disabledTilePositions.Contains(pos))
        {
            DisableRandomTile(); // Try again
            return;
        }

        // Clear previously disabled tile(s)
        disabledTilePositions.Clear();
        // Add the new one
        disabledTilePositions.Add(pos);

        if (tileNames.ContainsKey(tileToDisable))
            Debug.Log($"[MAP WARNING] Disabled tile: {tileNames[tileToDisable]} at position {pos}");
        else
            Debug.Log($"[MAP WARNING] Disabled unknown tile at position {pos}");

        RpcDisableTile(pos);
    }
    */

    void DisableRandomTile()
{
    if (spawnedTiles.Count == 0) return;

    GameObject tileToDisable = spawnedTiles[Random.Range(0, spawnedTiles.Count)];
    Vector3 pos = tileToDisable.transform.position;

    // Skip if already disabled
    if (disabledTilePositions.Contains(pos))
    {
        DisableRandomTile(); // Try again
        return;
    }

    // Clear previous disabled tiles
    disabledTilePositions.Clear();

    // Add all positions in the tile region to the disabled set
    float tileHalfSize = 19f;  // Adjust based on your tile size
    float step = 1f;          // Resolution: how granular the positions are

    for (float x = pos.x - tileHalfSize; x <= pos.x + tileHalfSize; x += step)
    {
        for (float z = pos.z - tileHalfSize; z <= pos.z + tileHalfSize; z += step)
        {
            // disabledTilePositions.Add(new Vector3(Mathf.Round(x), pos.y, Mathf.Round(z)));
            disabledTilePositions.Add(new Vector3(Mathf.Round(x), Mathf.Round(0), Mathf.Round(z)));
        
        }
    }

    if (tileNames.ContainsKey(tileToDisable))
        Debug.Log($"[MAP WARNING] Disabled tile: {tileNames[tileToDisable]} at position {pos}");
    else
        Debug.Log($"[MAP WARNING] Disabled unknown tile at position {pos}");

    RpcDisableTile(pos);
}


    [ClientRpc]
    void RpcDisableTile(Vector3 tilePosition)
    {
        // disabledTilePositions.Add(tilePosition);
        disabledTilePositions.Add(new Vector3(Mathf.Round(tilePosition.x), Mathf.Round(0), Mathf.Round(tilePosition.z)));
        Debug.Log($"[CLIENT] Tile disabled at: {tilePosition}");
    }

    // === BOUNDS UTILS ===

    Bounds GetMeshBounds(GameObject obj)
    {
        MeshFilter[] meshFilters = obj.GetComponentsInChildren<MeshFilter>();
        if (meshFilters.Length == 0) return new Bounds(obj.transform.position, Vector3.zero);

        Bounds bounds = meshFilters[0].mesh.bounds;
        Matrix4x4 objMatrix = obj.transform.localToWorldMatrix;

        foreach (MeshFilter mf in meshFilters)
        {
            Bounds meshBounds = mf.sharedMesh.bounds;
            Vector3 worldCenter = objMatrix.MultiplyPoint3x4(meshBounds.center);
            Vector3 worldSize = Vector3.Scale(meshBounds.size, mf.transform.lossyScale);
            bounds.Encapsulate(new Bounds(worldCenter, worldSize));
        }

        return bounds;
    }

    // === REGION CHECKING ===

    public bool IsInsideDisabledTileRegionXZ(Vector3 playerPos)
    {
        foreach (Vector3 tileCenter in disabledTilePositions)
        {
            if (IsWithinXZ(tileCenter, playerPos, TILE_HALF_WIDTH, TILE_HALF_DEPTH))
                return true;
        }
        return false;
    }

    bool IsWithinXZ(Vector3 tileCenter, Vector3 point, float halfWidth, float halfDepth)
    {
        return Mathf.Abs(point.x - tileCenter.x) <= halfWidth &&
               Mathf.Abs(point.z - tileCenter.z) <= halfDepth;
    }

    // === SHUFFLE UTILITY ===

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

    // === GIZMO VISUALIZATION (EDITOR ONLY) ===

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (tileBoundsByPosition != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            foreach (var kvp in tileBoundsByPosition)
            {
                if (disabledTilePositions.Contains(kvp.Key))
                    Gizmos.DrawCube(kvp.Value.center, kvp.Value.size);
            }
        }
    }
#endif
}
