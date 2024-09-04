using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;  // Array of tile prefabs
    public GameObject[] blockagePrefabs; // Array of blockage prefabs
    public int numberOfTiles = 5; // Number of tiles to keep active at a time
    public float tileLength = 30f; // Length of each tile
    public Transform player; // Reference to the player
    private float spawnZ = 0f; // Z position for spawning the next tile
    private float safeZone = 45f; // How far behind the player tiles should be destroyed
    private List<GameObject> activeTiles; // List of currently active tiles
    private int lastPrefabIndex = 0;

    void Start()
    {
        activeTiles = new List<GameObject>();

        // Spawn initial tiles
        for (int i = 0; i < numberOfTiles; i++)
        {
            if (i < 2)  // Create the first tiles without randomization
                SpawnTile(0);
            else
                SpawnTile();
        }
    }

    void Update()
    {
        // Check if we need to spawn a new tile based on the player's position
        if (player.position.z - safeZone > (spawnZ - numberOfTiles * tileLength))
        {
            SpawnTile();
            DeleteTile();
        }
    }

    void SpawnTile(int prefabIndex = -1)
    {
        GameObject tile;
        if (prefabIndex == -1)
        {
            prefabIndex = RandomPrefabIndex();
        }

        tile = Instantiate(tilePrefabs[prefabIndex]) as GameObject;
        tile.transform.SetParent(transform);
        tile.transform.position = Vector3.forward * spawnZ;
        spawnZ += tileLength;
        activeTiles.Add(tile);

        // Spawn random blockages on the tile
        SpawnBlockages(tile);
    }

    void SpawnBlockages(GameObject tile)
    {
        // Assuming that each tile has empty GameObjects as spawn points for blockages
        BlockageSpawnPoint[] spawnPoints = tile.GetComponentsInChildren<BlockageSpawnPoint>();

        // Randomly select some spawn points to place blockages
        foreach (var point in spawnPoints)
        {
            if (Random.Range(0, 100) < 50) // 50% chance to spawn a blockage at each point
            {
                int randomIndex = Random.Range(0, blockagePrefabs.Length);
                GameObject blockage = Instantiate(blockagePrefabs[randomIndex], point.transform.position, point.transform.rotation);
                blockage.transform.SetParent(tile.transform);
            }
        }
    }

    void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    private int RandomPrefabIndex()
    {
        if (tilePrefabs.Length <= 1)
        {
            return 0;
        }

        int randomIndex = lastPrefabIndex;
        while (randomIndex == lastPrefabIndex)
        {
            randomIndex = Random.Range(0, tilePrefabs.Length);
        }

        lastPrefabIndex = randomIndex;
        return randomIndex;
    }
}
