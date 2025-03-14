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
    [SerializeField] float rotationSpeed = 2f;
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
            randomIndex = 4;
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
        Vector3 targetPosition1 = new Vector3(-500f, 300f, targetPositionOnZaxis);
        float distance1 = Vector3.Distance(startPosition, targetPosition1);
        float duration1 = (distance1 > 0.01f) ? distance1 / moveSpeed : 0.1f;

        Vector3 targetPosition2 = new Vector3(-400f, targetPosition1.y - 25, -2400f);
        float distance2 = Vector3.Distance(targetPosition1, targetPosition2);
        float duration2 = (distance2 > 0.01f) ? distance2 / moveSpeed : 0.1f;

        Quaternion startRotation = plane.transform.rotation;
        Quaternion targetRotation1 = Quaternion.Euler(0, 45f, 45f);
        float angle1 = Quaternion.Angle(startRotation, targetRotation1);
        float rotationDuration1 = angle1 / rotationSpeed;

        Vector3 targetPosition3 = new Vector3(-400f, targetPosition2.y - 25, -2300f);
        float distance3 = Vector3.Distance(targetPosition2, targetPosition3);
        float duration3 = (distance3 > 0.01f) ? distance3 / moveSpeed : 0.1f;

        Quaternion targetRotation2 = Quaternion.Euler(0, 0, 0);
        float angle2 = Quaternion.Angle(targetRotation1, targetRotation2);
        float rotationDuration2 = angle2 / rotationSpeed;

        float totalDuration = Mathf.Max(duration1 + duration2 + duration3, rotationDuration1 + rotationDuration2);
        float elapsedTime = 0f;

        while (elapsedTime < totalDuration)
        {
            float t1 = elapsedTime / duration1;
            float t2 = Mathf.Clamp01((elapsedTime - duration1) / duration2);
            float t3 = Mathf.Clamp01((elapsedTime - (duration1 + duration2)) / duration3); 

            if (elapsedTime < duration1)
            {
                plane.transform.position = Vector3.Lerp(startPosition, targetPosition1, t1);
            }
            else if (elapsedTime >= duration1 && elapsedTime < duration1 + duration2)
            {
                plane.transform.position = Vector3.Lerp(targetPosition1, targetPosition2, t2);
                plane.transform.rotation = Quaternion.Slerp(startRotation, targetRotation1, t2);
            }
            else if (elapsedTime >= duration1 + duration2)
            {
                plane.transform.position = Vector3.Lerp(targetPosition2, targetPosition3, t3);
                plane.transform.rotation = Quaternion.Slerp(targetRotation1, targetRotation2, t3);
            }

            elapsedTime += Time.deltaTime * increaseSpeed;
            yield return null;
        }

        // ✅ Ensure final values are correctly set
        plane.transform.position = targetPosition3;
        plane.transform.rotation = targetRotation2;
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
        Vector3 targetPosition1 = new Vector3(-400f, 300f, targetPositionOnZaxis); // ✅ Set destination (change as needed)
        float distance1 = Vector3.Distance(startPosition, targetPosition1);
        float duration1 = (distance1 > 0.01f) ? distance1 / moveSpeed : 0.1f;
        Vector3 targetPosition2 = new Vector3(-400f,0f,-850f);
        float distance2 = Vector3.Distance(targetPosition1,targetPosition2);
        float duration2 = (distance2 > 0.01f) ? distance2 / moveSpeed : 0.1f;

        float totalDuration = duration1 + duration2;
        float elapsedTime = 0f;

        while (elapsedTime < totalDuration)
        {
            float t1 = elapsedTime / duration1;
            float t2 = (elapsedTime-duration1) / duration2;
            if (elapsedTime < duration1)
            {
                plane.transform.position = Vector3.Lerp(startPosition, targetPosition1, t1); // ✅ Smooth movement
            }
            else if(elapsedTime > duration1)
            {
                plane.transform.position = Vector3.Lerp(targetPosition1, targetPosition2, t2); // ✅ Smooth movement
            }
            elapsedTime += Time.deltaTime * increaseSpeed;
            yield return null;
        }
    }
}
