using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyingPlane : MonoBehaviour
{
    [SerializeField] private GameObject planePrefab;  // Assign the plane prefab in the Inspector
    [SerializeField] private float spawnInterval = 10f; // Time in seconds between spawns
    [SerializeField] private Vector3 targetPosition = new Vector3(0, 0, 0);
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float increaseSpeed = 2f;
    // List of predefined spawn positions
    [SerializeField] private List<Vector3> spawnPositions = new List<Vector3>
    {
        new Vector3(-50, 10, 0),
        new Vector3(30, 12, -20),
        new Vector3(100, 15, 50),
        new Vector3(-75, 10, 30),
        new Vector3(40, 10, -40),
        new Vector3(-30, 10, 70),
        new Vector3(90, 10, -10),
        new Vector3(-100, 10, 90),
        new Vector3(60, 10, 30),
        new Vector3(-20, 10, -60)
    };

    private void Start()
    {
        StartCoroutine(SpawnPlanes());
    }

    private IEnumerator SpawnPlanes()
    {
        while (true) // Infinite loop to keep spawning planes
        {
            yield return new WaitForSeconds(spawnInterval); // Wait for the interval
            SpawnPlane();
        }
    }

    private void SpawnPlane()
    {
        if (planePrefab != null && spawnPositions.Count > 0)
        {
            int randomIndex = Random.Range(0, spawnPositions.Count); // Pick a random spawn position
            Vector3 chosenPosition = spawnPositions[randomIndex];
            Quaternion spawnRotation = Quaternion.Euler(0, 180f, 0);

            GameObject newPlane = Instantiate(planePrefab, chosenPosition, spawnRotation);
            Debug.Log($"🛩 Plane spawned at position: {chosenPosition}");

            StartCoroutine(MovePlane(newPlane));
        }
        else
        {
            Debug.LogError("⚠️ Plane Prefab is missing or no spawn positions assigned!");
        }
    }
    private IEnumerator MovePlane(GameObject plane)
    {
        Vector3 targetPosition = this.targetPosition; // ✅ Set destination (change as needed)
        Vector3 startPosition = plane.transform.position;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float duration = (distance > 0.01f) ? distance / moveSpeed : 0.1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            plane.transform.position = Vector3.Lerp(startPosition, targetPosition, t); // ✅ Smooth movement
            elapsedTime += Time.deltaTime * increaseSpeed;
            yield return null;
        }

        plane.transform.position = targetPosition; // ✅ Ensure exact final position
        Debug.Log($"🛫 Plane reached target position: {targetPosition}");
    }
}
