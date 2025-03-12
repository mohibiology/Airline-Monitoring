using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyingPlane : MonoBehaviour
{
    [SerializeField] private GameObject planePrefab;  // Assign the plane prefab in the Inspector
    [SerializeField] private Transform planeHandler;
    [SerializeField] private float spawnInterval = 10f; // Time in seconds between spawns
    [SerializeField] private float targetPositionOnZaxis = 3000f;
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
        while (true)
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
            Quaternion spawnRotation = Quaternion.Euler(0,0,0);
            if(randomIndex<=1)
            {
                spawnRotation = Quaternion.Euler(0, 180f, 0);
            }
            else if(randomIndex<=3 && randomIndex>1)
            {
                spawnRotation = Quaternion.Euler(0, 90f, 0);
            }
            else if(randomIndex<=5 && randomIndex>3)
            {
                spawnRotation = Quaternion.Euler(0, 135f, 0);
            }

            GameObject newPlane = Instantiate(planePrefab, chosenPosition, spawnRotation, planeHandler);
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
        Vector3 startPosition = plane.transform.position;
        Vector3 targetPosition1 = new Vector3(-2000f, plane.transform.position.y-100, targetPositionOnZaxis); // ✅ Set destination (change as needed)
        float distance1 = Vector3.Distance(startPosition, targetPosition1);
        float duration1 = (distance1 > 0.01f) ? distance1 / moveSpeed : 0.1f;

        Vector3 targetPosition2 = new Vector3(-400, targetPosition1.y, targetPosition1.z);
        float distance2 = Mathf.Abs(targetPosition2.x - targetPosition1.x);
        float duration2 = (distance2 > 0.01f) ? distance2 / moveSpeed : 2f;
        float curveDepth = 1000f; // Controls how deep the U-curve is
        Vector3 controlPoint1 = new Vector3((targetPosition1.x + targetPosition2.x) / 2, targetPosition1.y, targetPosition1.z - curveDepth); 
        Vector3 controlPoint2 = new Vector3((targetPosition1.x + targetPosition2.x) / 2, targetPosition1.y, targetPosition1.z + curveDepth); // ✅ Pulls back to original Z
        Quaternion initialRotation = plane.transform.rotation;
        Quaternion targetRotation1 = Quaternion.Euler(0, 360f, 0); // ✅ Smooth 180° rotation

        float totalDuration = duration1 + duration2;
        float elapsedTime = 0f;

        while (elapsedTime < totalDuration)
        {
            float t1 = elapsedTime / duration1;
            float t2 = (elapsedTime - duration1) / duration2;
            if (elapsedTime < duration1)
            {
                plane.transform.position = Vector3.Lerp(startPosition, targetPosition1, t1); // ✅ Smooth movement
            }
            else if (elapsedTime > duration1)
            {
                // Vector3 newPos = Mathf.Pow(1 - t2, 3) * targetPosition1
                //         + 3 * Mathf.Pow(1 - t2, 2) * t2 * controlPoint1
                //         + 3 * (1 - t2) * Mathf.Pow(t2, 2) * controlPoint2
                //         + Mathf.Pow(t2, 3) * targetPosition2;

                // // ✅ Apply Position & Rotation
                // plane.transform.position = newPos;
                // plane.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation1, t2);
            }
            elapsedTime += Time.deltaTime * increaseSpeed;
            yield return null;
        }
    }
    private IEnumerator MovePlaneInUCurve(GameObject plane)
    {
        Vector3 startPosition = plane.transform.position;
        Vector3 endPosition = new Vector3(-400, startPosition.y, -2500); // ✅ Correct end position at -2500 Z-axis

        float distance = Vector3.Distance(startPosition, endPosition);
        float duration = (distance > 0.01f) ? distance / moveSpeed : 2f; // ✅ Fixed duration scaling
        float elapsedTime = 0f;

        // 🎯 **Fixed Control Points for a Perfect U-Curve**
        Vector3 controlPoint1 = new Vector3(-1600, startPosition.y, startPosition.z - 1000);  
        Vector3 controlPoint2 = new Vector3(-1000, startPosition.y, -2700); // ✅ Adjusted to guide plane back correctly

        Quaternion initialRotation = plane.transform.rotation;
        Quaternion finalRotation = initialRotation * Quaternion.Euler(0, 180f, 0); // ✅ Smooth 180° rotation

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // 🎯 **Fixed Cubic Bézier Curve**
            Vector3 newPos = Mathf.Pow(1 - t, 3) * startPosition
                        + 3 * Mathf.Pow(1 - t, 2) * t * controlPoint1
                        + 3 * (1 - t) * Mathf.Pow(t, 2) * controlPoint2
                        + Mathf.Pow(t, 3) * endPosition;

            // ✅ Apply Position & Rotation
            plane.transform.position = newPos;
            plane.transform.rotation = Quaternion.Slerp(initialRotation, finalRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ✅ Ensure exact final position & rotation
        plane.transform.position = endPosition;
        plane.transform.rotation = finalRotation;
        Debug.Log($"🛫 Plane completed U-curve movement at {endPosition}");
    }


}
