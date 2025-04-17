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
            randomIndex = 13;
            Vector3 chosenPosition = spawnPositions[randomIndex];
            Quaternion spawnRotation = Quaternion.Euler(0,0,0);
            GameObject newPlane = null;
            IEnumerator movementCoroutine = null;
            if(randomIndex<=1)
            {
                spawnRotation = Quaternion.Euler(0, -180f, 0);
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
        yield return StartCoroutine(MoveAndRotate(
            plane,
            plane.transform.position,
            new Vector3(-2000f, 300, targetPositionOnZaxis),
            plane.transform.rotation,
            plane.transform.rotation
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-2000f, plane.transform.position.y, targetPositionOnZaxis),
            new Vector3(-1500f, 275f, targetPositionOnZaxis),
            plane.transform.rotation,
            Quaternion.Euler(0, 90f, 45f) 
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-1500f, 275f, targetPositionOnZaxis),
            new Vector3(-1200f, 250f, targetPositionOnZaxis),
            plane.transform.rotation,
            Quaternion.Euler(0, 90f, 0f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-1200f, 250f, targetPositionOnZaxis),
            new Vector3(-800f, 220f, targetPositionOnZaxis),
            plane.transform.rotation,
            plane.transform.rotation
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-800f, 220f, targetPositionOnZaxis),
            new Vector3(-400f, 200f, -2200f),
            plane.transform.rotation,
            Quaternion.Euler(0, 0f, 45f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 200f, -2200f),
            new Vector3(-400f, 170f, -1900),
            plane.transform.rotation,
            Quaternion.Euler(0, 0f, 0f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 170f, -1900f),
            new Vector3(-400f, 0f, -850f),
            Quaternion.Euler(0, 0f, 0f),
            Quaternion.Euler(0, 0f, 0f)
        ));

    }
    private IEnumerator MoveTillFourthPlane(GameObject plane)
    {

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
            new Vector3(1000f, 300f, targetPositionOnZaxis),
            plane.transform.rotation,
            Quaternion.Euler(0, -90f, -45f) 
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(1000f, 300f, targetPositionOnZaxis),
            new Vector3(800f, 275f, targetPositionOnZaxis),
            plane.transform.rotation,
            Quaternion.Euler(0, -90f, 0f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(800f, 275f, targetPositionOnZaxis),
            new Vector3(200f, 250f, targetPositionOnZaxis),
            plane.transform.rotation,
            plane.transform.rotation
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(200f, 250f, targetPositionOnZaxis),
            new Vector3(-400f, 180f, -2200f),
            plane.transform.rotation,
            Quaternion.Euler(0, 0f, -45f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 180f, -2200f),
            new Vector3(-400f, 150f, -2000),
            plane.transform.rotation,
            Quaternion.Euler(0, 0f, 0f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 150f, -2000f),
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
            new Vector3(-800f, 220f, targetPositionOnZaxis),
            plane.transform.rotation,
            plane.transform.rotation // no rotation here
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-800f, 220f, targetPositionOnZaxis),
            new Vector3(-400f, 200f, -2200f),
            plane.transform.rotation,
            Quaternion.Euler(0, 0f, 45f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 200f, -2200f),
            new Vector3(-400f, 170f, -1900),
            plane.transform.rotation,
            Quaternion.Euler(0, 0f, 0f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 170f, -1900f),
            new Vector3(-400f, 0f, -850f),
            Quaternion.Euler(0, 0f, 0f),
            Quaternion.Euler(0, 0f, 0f)
        ));
    }


    private IEnumerator MoveTillEighthPlane(GameObject plane)
    {
        yield return StartCoroutine(MoveAndRotate(
            plane,
            plane.transform.position,
            new Vector3(200f, 250f, targetPositionOnZaxis),
            plane.transform.rotation,
            plane.transform.rotation // no rotation here
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(200f, 250f, targetPositionOnZaxis),
            new Vector3(-400f, 180f, -2200f),
            plane.transform.rotation,
            Quaternion.Euler(0, 0f, -45f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 180f, -2200f),
            new Vector3(-400f, 150f, -2000),
            plane.transform.rotation,
            Quaternion.Euler(0, 0f, 0f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 150f, -2000f),
            new Vector3(-400f, 0f, -850f),
            Quaternion.Euler(0, 0f, 0f),
            Quaternion.Euler(0, 0f, 0f)
        ));
    }
    private IEnumerator MoveTillTenthPlane(GameObject plane)
    {

        yield return StartCoroutine(MoveAndRotate(
            plane,
            plane.transform.position,
            new Vector3(-2000f, 300f, targetPositionOnZaxis),
            plane.transform.rotation,
            plane.transform.rotation // no rotation here
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-2000f, plane.transform.position.y, targetPositionOnZaxis),
            new Vector3(-1500f, 275f, targetPositionOnZaxis),
            plane.transform.rotation,
            Quaternion.Euler(0, 90f, 45f) 
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-1500f, 275f, targetPositionOnZaxis),
            new Vector3(-1200f, 250f, targetPositionOnZaxis),
            plane.transform.rotation,
            Quaternion.Euler(0, 90f, 0f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-1200f, 250f, targetPositionOnZaxis),
            new Vector3(-800f, 220f, targetPositionOnZaxis),
            plane.transform.rotation,
            plane.transform.rotation
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-800f, 220f, targetPositionOnZaxis),
            new Vector3(-400f, 200f, -2200f),
            plane.transform.rotation,
            Quaternion.Euler(0, 0f, 45f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 200f, -2200f),
            new Vector3(-400f, 170f, -1900),
            plane.transform.rotation,
            Quaternion.Euler(0, 0f, 0f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 170f, -1900f),
            new Vector3(-400f, 0f, -850f),
            Quaternion.Euler(0, 0f, 0f),
            Quaternion.Euler(0, 0f, 0f)
        ));
    }
    
    private IEnumerator MoveTillTwelvthPlane(GameObject plane)
    {

        yield return StartCoroutine(MoveAndRotate(
            plane,
            plane.transform.position,
            new Vector3(2000f, 360f, targetPositionOnZaxis),
            plane.transform.rotation,
            plane.transform.rotation // no rotation here
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(2000f, plane.transform.position.y, targetPositionOnZaxis),
            new Vector3(1500f, 300f, targetPositionOnZaxis),
            plane.transform.rotation,
            Quaternion.Euler(0, -90f, -45f) 
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(1500f, 300f, targetPositionOnZaxis),
            new Vector3(1300f, 275f, targetPositionOnZaxis),
            plane.transform.rotation,
            Quaternion.Euler(0, -90f, 0f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(1300f, 275f, targetPositionOnZaxis),
            new Vector3(200f, 250f, targetPositionOnZaxis),
            plane.transform.rotation,
            plane.transform.rotation
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(200f, 250f, targetPositionOnZaxis),
            new Vector3(-400f, 180f, -2200f),
            plane.transform.rotation,
            Quaternion.Euler(0, 0f, -45f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 180f, -2200f),
            new Vector3(-400f, 150f, -2000),
            plane.transform.rotation,
            Quaternion.Euler(0, 0f, 0f)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 150f, -2000f),
            new Vector3(-400f, 0f, -850f),
            Quaternion.Euler(0, 0f, 0f),
            Quaternion.Euler(0, 0f, 0f)
        ));
    }

    private IEnumerator MoveTillFourteenthPlane(GameObject plane)
    {
        Vector3 rotationEuler = plane.transform.rotation.eulerAngles;
        Quaternion currentRotation = Quaternion.Euler(rotationEuler);

        // yield return StartCoroutine(MoveAndRotate(
        //     plane,
        //     plane.transform.position,
        //     new Vector3(-400f, 300f, targetPositionOnZaxis),
        //     currentRotation,
        //     currentRotation
        // ));

        // yield return StartCoroutine(MoveAndRotate(
        //     plane,
        //     new Vector3(-400f, 300f, targetPositionOnZaxis),
        //     new Vector3(-400f, 0f, -850f),
        //     currentRotation,
        //     currentRotation
        // ));
        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-400f, 0f, -900),
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
            new Vector3(-400f, 0f, 750f),
            plane.transform.rotation,
            plane.transform.rotation
        ));

        yield return StartCoroutine(MoveAlongBezier(
            plane,
            new Vector3(-400f, 0f, 750f),
            new Vector3(-330f, 0f, 812f),
            plane.transform.rotation,
            Quaternion.Euler(0, 90, 0)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-330f, 0f, 812f),
            new Vector3(-250f, 0f, 812f),
            plane.transform.rotation,
            plane.transform.rotation
        ));

        yield return StartCoroutine(MoveAlongBezier(
            plane,
            new Vector3(-250f, 0f, 812f),
            new Vector3(-199f, 0f, 730f),
            plane.transform.rotation,
            Quaternion.Euler(0,180,0)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-199f, 0f, 730f),
            new Vector3(-199f, 0f, 650f),
            plane.transform.rotation,
            plane.transform.rotation
        ));

        yield return StartCoroutine(MoveAlongBezier(
            plane,
            new Vector3(-199f, 0f, 650f),
            new Vector3(-236f, 0f, 550f),
            plane.transform.rotation,
            Quaternion.Euler(0,225,0)
        ));

        yield return StartCoroutine(MoveAndRotate(
            plane,
            new Vector3(-236f, 0f, 550f),
            new Vector3(-315f, 0f, 472f),
            plane.transform.rotation,
            Quaternion.Euler(0,225,0)
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

    private IEnumerator MoveAlongBezier(GameObject plane, Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot)
    {
        moveSpeed = 75f;
        float distance = Vector3.Distance(startPos, endPos);
        float duration = (distance > 0.01f) ? distance / moveSpeed : 0.1f;

        // Define control points for cubic Bezier
        Vector3 direction = (endPos - startPos).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, direction); // perpendicular to path

        // Control point offset (adjust this to get wider or tighter turns)
        float controlOffset = 40f;

        Vector3 p0 = startPos;
        Vector3 p3 = endPos;
        Vector3 p1 = p0 + (startRot * Vector3.forward) * controlOffset;
        Vector3 p2 = p3 - (endRot * Vector3.forward) * controlOffset;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);

            // Bezier formula
            Vector3 pos =
                Mathf.Pow(1 - t, 3) * p0 +
                3 * Mathf.Pow(1 - t, 2) * t * p1 +
                3 * (1 - t) * Mathf.Pow(t, 2) * p2 +
                Mathf.Pow(t, 3) * p3;

            // Get direction for rotation
            Vector3 forwardDir = BezierTangent(p0, p1, p2, p3, t).normalized;
            if (forwardDir != Vector3.zero)
                plane.transform.rotation = Quaternion.LookRotation(forwardDir);

            plane.transform.position = pos;

            elapsed += Time.deltaTime * increaseSpeed;
            yield return null;
        }

        // Final snap
        plane.transform.position = p3;
        plane.transform.rotation = endRot;
    }
    private Vector3 BezierTangent(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return
            3 * Mathf.Pow(1 - t, 2) * (p1 - p0) +
            6 * (1 - t) * t * (p2 - p1) +
            3 * Mathf.Pow(t, 2) * (p3 - p2);
    }
}
