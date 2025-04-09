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
            randomIndex = 12;
            Vector3 chosenPosition = spawnPositions[randomIndex];
            Quaternion spawnRotation = Quaternion.Euler(0,0,0);
            GameObject newPlane = null;
            IEnumerator movementCoroutine = null;
            if(randomIndex<=1)
            {
                spawnRotation = Quaternion.Euler(0, 180f, 0);
                newPlane = Instantiate(planePrefab, chosenPosition, spawnRotation, planeHandler);
                movementCoroutine = (MoveTillSecondPlane(newPlane));
            }
            else if(randomIndex<=3 && randomIndex>1)
            {
                spawnRotation = Quaternion.Euler(0, 180f, 0);
                newPlane = Instantiate(planePrefab, chosenPosition, spawnRotation, planeHandler);
                movementCoroutine = (MoveTillFourthPlane(newPlane));
            }
            else if(randomIndex<=5 && randomIndex>3)
            {
                spawnRotation = Quaternion.Euler(0, 90f, 0);
                newPlane = Instantiate(planePrefab, chosenPosition, spawnRotation, planeHandler);
                movementCoroutine = (MoveTillSixthPlane(newPlane));
            }
            else if(randomIndex<=7 && randomIndex>5)
            {
                spawnRotation = Quaternion.Euler(0, -90f, 0);
                newPlane = Instantiate(planePrefab, chosenPosition, spawnRotation, planeHandler);
                movementCoroutine = (MoveTillEighthPlane(newPlane));
            }
            else if(randomIndex<=9 && randomIndex>7)
            {
                spawnRotation = Quaternion.Euler(0, 135f, 0);
                newPlane = Instantiate(planePrefab, chosenPosition, spawnRotation, planeHandler);
                movementCoroutine = (MoveTillTenthPlane(newPlane));
            }
            else if(randomIndex<=11 && randomIndex>9)
            {
                spawnRotation = Quaternion.Euler(0, 225f, 0);
                newPlane = Instantiate(planePrefab, chosenPosition, spawnRotation, planeHandler);
                movementCoroutine = (MoveTillTwelvthPlane(newPlane));
            }
            else if(randomIndex<=13 && randomIndex>11)
            {
                spawnRotation = Quaternion.Euler(0, 0f, 0);
                newPlane = Instantiate(planePrefab, chosenPosition, spawnRotation, planeHandler);
                movementCoroutine = (MoveTillFourteenthPlane(newPlane));
            }

            if (newPlane != null && movementCoroutine != null)
            {
                StartCoroutine(RunMovementAndExit(newPlane, movementCoroutine));
            }
        }
        else
        {
            Debug.LogError("⚠️ Plane Prefab is missing or no spawn positions assigned!");
        }
    }

    private IEnumerator RunMovementAndExit(GameObject plane, IEnumerator movement)
    {
        yield return StartCoroutine(movement);
        yield return StartCoroutine(MoveTillExit(plane));
    }

    private IEnumerator MoveTillSecondPlane(GameObject plane)
    {
        // Vector3 startPosition = plane.transform.position;
        // Vector3 targetPosition1 = new Vector3(-2000f, plane.transform.position.y-100, targetPositionOnZaxis); // ✅ Set destination (change as needed)
        // float distance1 = Vector3.Distance(startPosition, targetPosition1);
        // float duration1 = (distance1 > 0.01f) ? distance1 / moveSpeed : 0.1f;

        // float totalDuration = duration1;
        // float elapsedTime = 0f;

        // while (elapsedTime < totalDuration)
        // {
        //     float t1 = elapsedTime / duration1;
        //     if (elapsedTime < duration1)
        //     {
        //         plane.transform.position = Vector3.Lerp(startPosition, targetPosition1, t1); // ✅ Smooth movement
        //     }
        //     elapsedTime += Time.deltaTime * increaseSpeed;
        //     yield return null;
        // }

        yield return StartCoroutine(MoveAndRotate(
            plane,
            plane.transform.position,
            new Vector3(-2000f, plane.transform.position.y-100, targetPositionOnZaxis),
            plane.transform.rotation,
            plane.transform.rotation // no rotation here
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-2000f, plane.transform.position.y, targetPositionOnZaxis),
            new Vector3(-500f, 300f, targetPositionOnZaxis),
            plane.transform.rotation,
            Quaternion.Euler(0, 90f, 0) // no rotation here
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-500f, 300f, targetPositionOnZaxis),
            new Vector3(-400f, 275f, -2400f),
            plane.transform.rotation,
            Quaternion.Euler(0, 45f, 45f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 275f, -2400f),
            new Vector3(-400f, 250f, -2300f),
            Quaternion.Euler(0, 45f, 45f),
            Quaternion.Euler(0, 0f, 0f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 250f, -2300f),
            new Vector3(-400f, 0f, -850f),
            Quaternion.Euler(0, 0f, 0f),
            Quaternion.Euler(0, 0f, 0f)
        ));
    }
    private IEnumerator MoveTillFourthPlane(GameObject plane)
    {
        // Vector3 startPosition = plane.transform.position;
        // Vector3 targetPosition1 = new Vector3(2000f, plane.transform.position.y-100, targetPositionOnZaxis); // ✅ Set destination (change as needed)
        // float distance1 = Vector3.Distance(startPosition, targetPosition1);
        // float duration1 = (distance1 > 0.01f) ? distance1 / moveSpeed : 0.1f;

        // float totalDuration = duration1;
        // float elapsedTime = 0f;

        // while (elapsedTime < totalDuration)
        // {
        //     float t1 = elapsedTime / duration1;
        //     if (elapsedTime < duration1)
        //     {
        //         plane.transform.position = Vector3.Lerp(startPosition, targetPosition1, t1); // ✅ Smooth movement
        //     }
        //     elapsedTime += Time.deltaTime * increaseSpeed;
        //     yield return null;
        // }

        yield return StartCoroutine(MoveAndRotate(
            plane,
            plane.transform.position,
            new Vector3(2000f, plane.transform.position.y-100, targetPositionOnZaxis),
            plane.transform.rotation,
            plane.transform.rotation // no rotation here
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(2000f, plane.transform.position.y, targetPositionOnZaxis),
            new Vector3(-500f, 300f, targetPositionOnZaxis),
            plane.transform.rotation,
            Quaternion.Euler(0, -90f, 0f) // no rotation here
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-500f, 300f, targetPositionOnZaxis),
            new Vector3(-400f, 275f, -2400f),
            plane.transform.rotation,
            Quaternion.Euler(0, -45f, -45f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 275f, -2400f),
            new Vector3(-400f, 250f, -2300f),
            Quaternion.Euler(0, -45f, -45f),
            Quaternion.Euler(0, 0f, 0f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 250f, -2300f),
            new Vector3(-400f, 0f, -850f),
            Quaternion.Euler(0, 0f, 0f),
            Quaternion.Euler(0, 0f, 0f)
        ));
    }
    private IEnumerator MoveTillSixthPlane(GameObject plane)
    {
        yield return StartCoroutine(MoveAndRotate(
            plane,
            plane.transform.position,
            new Vector3(-500f, 300f, targetPositionOnZaxis),
            plane.transform.rotation,
            plane.transform.rotation // no rotation here
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-500f, 300f, targetPositionOnZaxis),
            new Vector3(-400f, 275f, -2400f),
            plane.transform.rotation,
            Quaternion.Euler(0, 45f, 45f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 275f, -2400f),
            new Vector3(-400f, 250f, -2300f),
            Quaternion.Euler(0, 45f, 45f),
            Quaternion.Euler(0, 0f, 0f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 250f, -2300f),
            new Vector3(-400f, 0f, -850f),
            Quaternion.Euler(0, 0f, 0f),
            Quaternion.Euler(0, 0f, 0f)
        ));
    }

    // private IEnumerator MoveTillSixthPlane(GameObject plane)
    // {
    //     Vector3 startPosition = plane.transform.position;
    //     Vector3 targetPosition1 = new Vector3(-500f, 300f, targetPositionOnZaxis);
    //     float distance1 = Vector3.Distance(startPosition, targetPosition1);
    //     float duration1 = (distance1 > 0.01f) ? distance1 / moveSpeed : 0.1f;

    //     Vector3 targetPosition2 = new Vector3(-400f, targetPosition1.y - 25, -2400f);
    //     float distance2 = Vector3.Distance(targetPosition1, targetPosition2);
    //     float duration2 = (distance2 > 0.01f) ? distance2 / moveSpeed : 0.1f;

    //     Quaternion startRotation = plane.transform.rotation;
    //     Quaternion targetRotation1 = Quaternion.Euler(0, 45f, 45f);
    //     float angle1 = Quaternion.Angle(startRotation, targetRotation1);
    //     float rotationDuration1 = angle1 / rotationSpeed;

    //     Vector3 targetPosition3 = new Vector3(-400f, targetPosition2.y - 25, -2300f);
    //     float distance3 = Vector3.Distance(targetPosition2, targetPosition3);
    //     float duration3 = (distance3 > 0.01f) ? distance3 / moveSpeed : 0.1f;

    //     Quaternion targetRotation2 = Quaternion.Euler(0, 0, 0);
    //     float angle2 = Quaternion.Angle(targetRotation1, targetRotation2);
    //     float rotationDuration2 = angle2 / rotationSpeed;

    //     Vector3 targetPosition4 = new Vector3(-400f,0f,-850f);
    //     float distance4 = Vector3.Distance(targetPosition3,targetPosition4);
    //     float duration4 = (distance4 > 0.01f) ? distance4 / moveSpeed : 0.1f;

    //     float totalDuration = Mathf.Max(duration1 + duration2 + duration3 + duration4, rotationDuration1 + rotationDuration2);
    //     float elapsedTime = 0f;

    //     while (elapsedTime < totalDuration)
    //     {
    //         float t1 = elapsedTime / duration1;
    //         float t2 = Mathf.Clamp01((elapsedTime - duration1) / duration2);
    //         float t3 = Mathf.Clamp01((elapsedTime - (duration1 + duration2)) / duration3); 
    //         float t4 = Mathf.Clamp01((elapsedTime - (duration1 + duration2 + duration3)) /duration4);

    //         if (elapsedTime < duration1)
    //         {
    //             plane.transform.position = Vector3.Lerp(startPosition, targetPosition1, t1);
    //         }
    //         else if (elapsedTime >= duration1 && elapsedTime < duration1 + duration2)
    //         {
    //             plane.transform.position = Vector3.Lerp(targetPosition1, targetPosition2, t2);
    //             plane.transform.rotation = Quaternion.Slerp(startRotation, targetRotation1, t2);
    //         }
    //         else if (elapsedTime >= duration1 + duration2 && elapsedTime < duration1 + duration2 + duration3)
    //         {
    //             plane.transform.position = Vector3.Lerp(targetPosition2, targetPosition3, t3);
    //             plane.transform.rotation = Quaternion.Slerp(targetRotation1, targetRotation2, t3);
    //         }
    //         else
    //         {
    //             plane.transform.position = Vector3.Lerp(targetPosition3, targetPosition4, t4);
    //         }
    //         elapsedTime += Time.deltaTime * increaseSpeed;
    //         yield return null;
    //     }

    //     // ✅ Ensure final values are correctly set
    //     plane.transform.position = targetPosition4;
    //     plane.transform.rotation = targetRotation2;
    // }

    private IEnumerator MoveTillEighthPlane(GameObject plane)
    {
        yield return StartCoroutine(MoveAndRotate(
            plane,
            plane.transform.position,
            new Vector3(-500f, 300f, targetPositionOnZaxis),
            plane.transform.rotation,
            plane.transform.rotation // no rotation here
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-500f, 300f, targetPositionOnZaxis),
            new Vector3(-400f, 275f, -2400f),
            plane.transform.rotation,
            Quaternion.Euler(0, -45f, -45f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 275f, -2400f),
            new Vector3(-400f, 250f, -2300f),
            Quaternion.Euler(0, -45f, -45f),
            Quaternion.Euler(0, 0f, 0f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 250f, -2300f),
            new Vector3(-400f, 0f, -850f),
            Quaternion.Euler(0, 0f, 0f),
            Quaternion.Euler(0, 0f, 0f)
        ));
    }
    // private IEnumerator MoveTillEighthPlane(GameObject plane)
    // {
    //     Vector3 startPosition = plane.transform.position;
    //     Vector3 targetPosition1 = new Vector3(2000f, plane.transform.position.y-100, targetPositionOnZaxis); // ✅ Set destination (change as needed)
    //     float distance1 = Vector3.Distance(startPosition, targetPosition1);
    //     float duration1 = (distance1 > 0.01f) ? distance1 / moveSpeed : 0.1f;

    //     float totalDuration = duration1;
    //     float elapsedTime = 0f;

    //     while (elapsedTime < totalDuration)
    //     {
    //         float t1 = elapsedTime / duration1;
    //         if (elapsedTime < duration1)
    //         {
    //             plane.transform.position = Vector3.Lerp(startPosition, targetPosition1, t1); // ✅ Smooth movement
    //         }
    //         elapsedTime += Time.deltaTime * increaseSpeed;
    //         yield return null;
    //     }
    // }
    private IEnumerator MoveTillTenthPlane(GameObject plane)
    {
        // Vector3 startPosition = plane.transform.position;
        // Vector3 targetPosition1 = new Vector3(-2000f, plane.transform.position.y-100, targetPositionOnZaxis); // ✅ Set destination (change as needed)
        // float distance1 = Vector3.Distance(startPosition, targetPosition1);
        // float duration1 = (distance1 > 0.01f) ? distance1 / moveSpeed : 0.1f;

        // float totalDuration = duration1;
        // float elapsedTime = 0f;

        // while (elapsedTime < totalDuration)
        // {
        //     float t1 = elapsedTime / duration1;
        //     if (elapsedTime < duration1)
        //     {
        //         plane.transform.position = Vector3.Lerp(startPosition, targetPosition1, t1); // ✅ Smooth movement
        //     }
        //     elapsedTime += Time.deltaTime * increaseSpeed;
        //     yield return null;
        yield return StartCoroutine(MoveAndRotate(
            plane,
            plane.transform.position,
            new Vector3(-2000f, plane.transform.position.y-100, targetPositionOnZaxis),
            plane.transform.rotation,
            plane.transform.rotation // no rotation here
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-2000f, plane.transform.position.y, targetPositionOnZaxis),
            new Vector3(-500f, 300f, targetPositionOnZaxis),
            plane.transform.rotation,
            Quaternion.Euler(0, 90f, 0) // no rotation here
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-500f, 300f, targetPositionOnZaxis),
            new Vector3(-400f, 275f, -2400f),
            Quaternion.Euler(0, 90f, 0),
            Quaternion.Euler(0, 45f, 45f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 275f, -2400f),
            new Vector3(-400f, 250f, -2300f),
            Quaternion.Euler(0, 45f, 45f),
            Quaternion.Euler(0, 0f, 0f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 250f, -2300f),
            new Vector3(-400f, 0f, -850f),
            Quaternion.Euler(0, 0f, 0f),
            Quaternion.Euler(0, 0f, 0f)
        ));
    }
    
    private IEnumerator MoveTillTwelvthPlane(GameObject plane)
    {
        // Vector3 startPosition = plane.transform.position;
        // Vector3 targetPosition1 = new Vector3(2000f, plane.transform.position.y-100, targetPositionOnZaxis); // ✅ Set destination (change as needed)
        // float distance1 = Vector3.Distance(startPosition, targetPosition1);
        // float duration1 = (distance1 > 0.01f) ? distance1 / moveSpeed : 0.1f;

        // float totalDuration = duration1;
        // float elapsedTime = 0f;

        // while (elapsedTime < totalDuration)
        // {
        //     float t1 = elapsedTime / duration1;
        //     if (elapsedTime < duration1)
        //     {
        //         plane.transform.position = Vector3.Lerp(startPosition, targetPosition1, t1); // ✅ Smooth movement
        //     }
        //     elapsedTime += Time.deltaTime * increaseSpeed;
        //     yield return null;
        // }

        yield return StartCoroutine(MoveAndRotate(
            plane,
            plane.transform.position,
            new Vector3(2000f, plane.transform.position.y-100, targetPositionOnZaxis),
            plane.transform.rotation,
            plane.transform.rotation // no rotation here
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(2000f, plane.transform.position.y, targetPositionOnZaxis),
            new Vector3(-500f, 300f, targetPositionOnZaxis),
            plane.transform.rotation,
            Quaternion.Euler(0, -90f, 0) // no rotation here
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-500f, 300f, targetPositionOnZaxis),
            new Vector3(-400f, 275f, -2400f),
            plane.transform.rotation,
            Quaternion.Euler(0, -45f, -45f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 275f, -2400f),
            new Vector3(-400f, 250f, -2300f),
            Quaternion.Euler(0, -45f, -45f),
            Quaternion.Euler(0, 0f, 0f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 250f, -2300f),
            new Vector3(-400f, 0f, -850f),
            Quaternion.Euler(0, 0f, 0f),
            Quaternion.Euler(0, 0f, 0f)
        ));
    }
    // private IEnumerator MoveTillFourteenthPlane(GameObject plane)
    // {
    //     Vector3 startPosition = plane.transform.position;
    //     Vector3 targetPosition1 = new Vector3(-400f, 300f, targetPositionOnZaxis); // ✅ Set destination (change as needed)
    //     float distance1 = Vector3.Distance(startPosition, targetPosition1);
    //     float duration1 = (distance1 > 0.01f) ? distance1 / moveSpeed : 0.1f;
    //     Vector3 targetPosition2 = new Vector3(-400f,0f,-850f);
    //     float distance2 = Vector3.Distance(targetPosition1,targetPosition2);
    //     float duration2 = (distance2 > 0.01f) ? distance2 / moveSpeed : 0.1f;

    //     float totalDuration = duration1 + duration2;
    //     float elapsedTime = 0f;

    //     while (elapsedTime < totalDuration)
    //     {
    //         float t1 = elapsedTime / duration1;
    //         float t2 = (elapsedTime-duration1) / duration2;
    //         if (elapsedTime < duration1)
    //         {
    //             plane.transform.position = Vector3.Lerp(startPosition, targetPosition1, t1); // ✅ Smooth movement
    //         }
    //         else if(elapsedTime > duration1)
    //         {
    //             plane.transform.position = Vector3.Lerp(targetPosition1, targetPosition2, t2); // ✅ Smooth movement
    //         }
    //         elapsedTime += Time.deltaTime * increaseSpeed;
    //         yield return null;
    //     }
    // }


    private IEnumerator MoveTillFourteenthPlane(GameObject plane)
    {
        Vector3 rotationEuler = plane.transform.rotation.eulerAngles;
        Quaternion currentRotation = Quaternion.Euler(rotationEuler);

        yield return StartCoroutine(MoveAndRotate(
            plane,
            plane.transform.position,
            new Vector3(-400f, 300f, targetPositionOnZaxis),
            currentRotation,
            currentRotation
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 300f, targetPositionOnZaxis),
            new Vector3(-400f, 0f, -850f),
            currentRotation,
            currentRotation
        ));
    }

    private IEnumerator MoveTillExit(GameObject plane)
    {
        yield return StartCoroutine(MoveAndRotate(
            plane,
            plane.transform.position,
            new Vector3(-400f, 0f, 770f),
            plane.transform.rotation,
            plane.transform.rotation
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 0f, 770f),
            new Vector3(-320f, 0f, 812f),
            plane.transform.rotation,
            Quaternion.Euler(0,90,0)
        ));
    }

    private IEnumerator MoveAndRotate(GameObject plane, Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot)
    {
        float distance = Vector3.Distance(startPos, endPos);
        float duration = (distance > 0.01f) ? distance / moveSpeed : 0.1f;

        float angle = Quaternion.Angle(startRot, endRot);
        float rotationDuration = (angle > 0.01f) ? angle / rotationSpeed : 0.1f;

        float totalDuration = Mathf.Max(rotationDuration, duration);
        float elapsedTime = 0f;

        while (elapsedTime < totalDuration)
        {
            float t = Mathf.Clamp01(elapsedTime / totalDuration);
            plane.transform.position = Vector3.Lerp(startPos, endPos, t);
            plane.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            elapsedTime += Time.deltaTime * increaseSpeed;
            yield return null;
        }

        // Snap to exact final position/rotation
        plane.transform.position = endPos;
        plane.transform.rotation = endRot;
    }

}
