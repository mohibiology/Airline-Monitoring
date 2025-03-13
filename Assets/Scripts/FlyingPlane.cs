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
            randomIndex = 11;
            Vector3 chosenPosition = spawnPositions[randomIndex];
            Quaternion spawnRotation = Quaternion.Euler(0,0,0);
            if(randomIndex<=1)
            {
                spawnRotation = Quaternion.Euler(0, 180f, 0);
                GameObject newPlane = Instantiate(planePrefab, chosenPosition, spawnRotation, planeHandler);
                StartCoroutine(MoveTillSecondPlane(newPlane));
            }
            else if(randomIndex<=3 && randomIndex>1)
            {
                spawnRotation = Quaternion.Euler(0, 180f, 0);
                GameObject newPlane = Instantiate(planePrefab, chosenPosition, spawnRotation, planeHandler);
                StartCoroutine(MoveTillFourthPlane(newPlane));
            }
            else if(randomIndex<=5 && randomIndex>3)
            {
                spawnRotation = Quaternion.Euler(0, 90f, 0);
                GameObject newPlane = Instantiate(planePrefab, chosenPosition, spawnRotation, planeHandler);
                StartCoroutine(MoveTillSixthPlane(newPlane));
            }
            else if(randomIndex<=7 && randomIndex>5)
            {
                spawnRotation = Quaternion.Euler(0, 270f, 0);
                GameObject newPlane = Instantiate(planePrefab, chosenPosition, spawnRotation, planeHandler);
                StartCoroutine(MoveTillEighthPlane(newPlane));
            }
            else if(randomIndex<=9 && randomIndex>7)
            {
                spawnRotation = Quaternion.Euler(0, 135f, 0);
                GameObject newPlane = Instantiate(planePrefab, chosenPosition, spawnRotation, planeHandler);
                StartCoroutine(MoveTillTenthPlane(newPlane));
            }
            else if(randomIndex<=11 && randomIndex>9)
            {
                spawnRotation = Quaternion.Euler(0, 225f, 0);
                GameObject newPlane = Instantiate(planePrefab, chosenPosition, spawnRotation, planeHandler);
                StartCoroutine(MoveTillTwelvthPlane(newPlane));
            }
            else if(randomIndex<=13 && randomIndex>11)
            {
                spawnRotation = Quaternion.Euler(0, 0f, 0);
                GameObject newPlane = Instantiate(planePrefab, chosenPosition, spawnRotation, planeHandler);
                StartCoroutine(MoveTillFourteenthPlane(newPlane));
            }
        }
        else
        {
            Debug.LogError("⚠️ Plane Prefab is missing or no spawn positions assigned!");
        }
    }
    private IEnumerator MoveTillSecondPlane(GameObject plane)
    {
        Vector3 startPosition = plane.transform.position;
        Vector3 targetPosition1 = new Vector3(-2000f, plane.transform.position.y-100, targetPositionOnZaxis); // ✅ Set destination (change as needed)
        float distance1 = Vector3.Distance(startPosition, targetPosition1);
        float duration1 = (distance1 > 0.01f) ? distance1 / moveSpeed : 0.1f;

        float totalDuration = duration1;
        float elapsedTime = 0f;

        while (elapsedTime < totalDuration)
        {
            float t1 = elapsedTime / duration1;
            if (elapsedTime < duration1)
            {
                plane.transform.position = Vector3.Lerp(startPosition, targetPosition1, t1); // ✅ Smooth movement
            }
            elapsedTime += Time.deltaTime * increaseSpeed;
            yield return null;
        }
    }
    private IEnumerator MoveTillFourthPlane(GameObject plane)
    {
        Vector3 startPosition = plane.transform.position;
        Vector3 targetPosition1 = new Vector3(2000f, plane.transform.position.y-100, targetPositionOnZaxis); // ✅ Set destination (change as needed)
        float distance1 = Vector3.Distance(startPosition, targetPosition1);
        float duration1 = (distance1 > 0.01f) ? distance1 / moveSpeed : 0.1f;

        float totalDuration = duration1;
        float elapsedTime = 0f;

        while (elapsedTime < totalDuration)
        {
            float t1 = elapsedTime / duration1;
            if (elapsedTime < duration1)
            {
                plane.transform.position = Vector3.Lerp(startPosition, targetPosition1, t1); // ✅ Smooth movement
            }
            elapsedTime += Time.deltaTime * increaseSpeed;
            yield return null;
        }
    }
    private IEnumerator MoveTillSixthPlane(GameObject plane)
    {
        Vector3 startPosition = plane.transform.position;
        Vector3 targetPosition1 = new Vector3(-2000f, plane.transform.position.y-100, targetPositionOnZaxis); // ✅ Set destination (change as needed)
        float distance1 = Vector3.Distance(startPosition, targetPosition1);
        float duration1 = (distance1 > 0.01f) ? distance1 / moveSpeed : 0.1f;

        float totalDuration = duration1;
        float elapsedTime = 0f;

        while (elapsedTime < totalDuration)
        {
            float t1 = elapsedTime / duration1;
            if (elapsedTime < duration1)
            {
                plane.transform.position = Vector3.Lerp(startPosition, targetPosition1, t1); // ✅ Smooth movement
            }
            elapsedTime += Time.deltaTime * increaseSpeed;
            yield return null;
        }
    }
    private IEnumerator MoveTillEighthPlane(GameObject plane)
    {
        Vector3 startPosition = plane.transform.position;
        Vector3 targetPosition1 = new Vector3(2000f, plane.transform.position.y-100, targetPositionOnZaxis); // ✅ Set destination (change as needed)
        float distance1 = Vector3.Distance(startPosition, targetPosition1);
        float duration1 = (distance1 > 0.01f) ? distance1 / moveSpeed : 0.1f;

        float totalDuration = duration1;
        float elapsedTime = 0f;

        while (elapsedTime < totalDuration)
        {
            float t1 = elapsedTime / duration1;
            if (elapsedTime < duration1)
            {
                plane.transform.position = Vector3.Lerp(startPosition, targetPosition1, t1); // ✅ Smooth movement
            }
            elapsedTime += Time.deltaTime * increaseSpeed;
            yield return null;
        }
    }
    private IEnumerator MoveTillTenthPlane(GameObject plane)
    {
        Vector3 startPosition = plane.transform.position;
        Vector3 targetPosition1 = new Vector3(-2000f, plane.transform.position.y-100, targetPositionOnZaxis); // ✅ Set destination (change as needed)
        float distance1 = Vector3.Distance(startPosition, targetPosition1);
        float duration1 = (distance1 > 0.01f) ? distance1 / moveSpeed : 0.1f;

        float totalDuration = duration1;
        float elapsedTime = 0f;

        while (elapsedTime < totalDuration)
        {
            float t1 = elapsedTime / duration1;
            if (elapsedTime < duration1)
            {
                plane.transform.position = Vector3.Lerp(startPosition, targetPosition1, t1); // ✅ Smooth movement
            }
            elapsedTime += Time.deltaTime * increaseSpeed;
            yield return null;
        }
    }
    private IEnumerator MoveTillTwelvthPlane(GameObject plane)
    {
        Vector3 startPosition = plane.transform.position;
        Vector3 targetPosition1 = new Vector3(2000f, plane.transform.position.y-100, targetPositionOnZaxis); // ✅ Set destination (change as needed)
        float distance1 = Vector3.Distance(startPosition, targetPosition1);
        float duration1 = (distance1 > 0.01f) ? distance1 / moveSpeed : 0.1f;

        float totalDuration = duration1;
        float elapsedTime = 0f;

        while (elapsedTime < totalDuration)
        {
            float t1 = elapsedTime / duration1;
            if (elapsedTime < duration1)
            {
                plane.transform.position = Vector3.Lerp(startPosition, targetPosition1, t1); // ✅ Smooth movement
            }
            elapsedTime += Time.deltaTime * increaseSpeed;
            yield return null;
        }
    }
    private IEnumerator MoveTillFourteenthPlane(GameObject plane)
    {
        Vector3 startPosition = plane.transform.position;
        Vector3 targetPosition1 = new Vector3(-400f, plane.transform.position.y-100, targetPositionOnZaxis); // ✅ Set destination (change as needed)
        float distance1 = Vector3.Distance(startPosition, targetPosition1);
        float duration1 = (distance1 > 0.01f) ? distance1 / moveSpeed : 0.1f;

        float totalDuration = duration1;
        float elapsedTime = 0f;

        while (elapsedTime < totalDuration)
        {
            float t1 = elapsedTime / duration1;
            if (elapsedTime < duration1)
            {
                plane.transform.position = Vector3.Lerp(startPosition, targetPosition1, t1); // ✅ Smooth movement
            }
            elapsedTime += Time.deltaTime * increaseSpeed;
            yield return null;
        }
    }
}
