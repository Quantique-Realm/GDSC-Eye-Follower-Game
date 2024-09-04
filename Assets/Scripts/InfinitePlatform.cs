using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinitePlatform : MonoBehaviour
{
    public GameObject[] platformPrefabs; // Array of platform prefabs to choose from
    public Transform playerTransform; // Reference to the player
    public float tileLength = 30f; // Length of each platform tile
    public int numberOfTiles = 5; // Number of tiles to keep active at one time
    public float safeZone = 45f; // Distance before deleting/repositioning tiles

    private List<GameObject> activeTiles; // List of currently active tiles
    private float spawnZ = 0f; // Initial Z position for tile spawning
    private int lastPrefabIndex = 0;

    void Start()
    {
        activeTiles = new List<GameObject>();

        // Spawn initial set of tiles
        for (int i = 0; i < numberOfTiles; i++)
        {
            if (i < 2)
            {
                SpawnTile(0); // First few tiles are predefined
            }
            else
            {
                SpawnTile(); // After that, spawn randomly selected tiles
            }
        }
    }

    void Update()
    {
        // Check if we need to spawn new tiles based on the player's position
        if (playerTransform.position.z - safeZone > (spawnZ - numberOfTiles * tileLength))
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

        // Instantiate new tile
        tile = Instantiate(platformPrefabs[prefabIndex]) as GameObject;
        tile.transform.SetParent(transform);
        tile.transform.position = Vector3.forward * spawnZ;
        spawnZ += tileLength;
        activeTiles.Add(tile);
    }

    void DeleteTile()
    {
        // Delete the tile that is farthest behind the player
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    private int RandomPrefabIndex()
    {
        if (platformPrefabs.Length <= 1)
        {
            return 0;
        }

        int randomIndex = lastPrefabIndex;
        while (randomIndex == lastPrefabIndex)
        {
            randomIndex = Random.Range(0, platformPrefabs.Length);
        }

        lastPrefabIndex = randomIndex;
        return randomIndex;
    }
}
