using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class spawner : MonoBehaviour
{
    public GameObject prefabToSpawn; // The prefab object to spawn
    public Transform spawnPosition; // The position where the prefab will be spawned
    public float spawnInterval = 2f; // Time between spawns
    public float spawnDuration = 5f; // Duration of spawning

    private float timer;

    void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // Check if the timer exceeds the spawn interval
        if (timer >= spawnInterval)
        {
            // Check if the prefab object is not already present
            if (GameObject.Find(prefabToSpawn.name) == null)
            {
                // Spawn the prefab object at the specified position
                SpawnPrefab();

                // Reset the timer
                timer = 0f;
            }
        }
    }

    void SpawnPrefab()
    {
        // Instantiate the prefab at the specified position
        Instantiate(prefabToSpawn, spawnPosition.position, Quaternion.identity);

        // Start the coroutine to despawn the prefab after a certain duration
        StartCoroutine(DespawnPrefab());
    }

    IEnumerator DespawnPrefab()
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(spawnDuration);

        // Destroy the spawned prefab
        Destroy(GameObject.Find(prefabToSpawn.name));
    }
}
