using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectActionHandler : MonoBehaviour
{
    public static ObjectActionHandler Instance;

    [SerializeField] public float rotationSpeed = 5f;
    [SerializeField] public float moveSpeed = 2f;
    [SerializeField] public float finalMoveSpeed = 2f; // ✅ Smooth movement for Step 4
    [SerializeField] public float rotationOnYaxis = 270f;
    [SerializeField] public float positionOnXaxis = -70f;
    [SerializeField] public float planeSpacing = 200f; // ✅ Updated to 200f spacing

    private Quaternion rotationBeforeGettingIntoLane = Quaternion.Euler(0, 180f, 0);
    private List<GameObject> planeQueue = new List<GameObject>(); // ✅ Store planes dynamically
    [SerializeField] Transform triggerObject; // ✅ Reference to trigger object
    [SerializeField] Transform triggerObjectBeforeTakeOff;
    [SerializeField] Transform triggerTakeOff;
    [SerializeField] Transform triggerHold;

    

    private GameObject currentTriggeredPlane;
    string movingPlaneTag = "BeforeOnTheLinePlane";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void RegisterPlane(GameObject plane)
    {
        if (!planeQueue.Contains(plane))
        {
            planeQueue.Add(plane);
        }
    }
    public void SetTriggeredPlane(GameObject plane)
    {
        currentTriggeredPlane = plane; // ✅ Store the triggered plane when it enters the trigger zone
    }

    public void PerformAction(GameObject targetObject, string tag)
    {
        if (!planeQueue.Contains(targetObject) && tag!="TakeOffPlane")
        {
            RegisterPlane(targetObject);
        }

        Vector3 targetPosition = Vector3.zero;

        if (tag == "BeforeLineUpPlane")
        {
            targetPosition = new Vector3(-175, 0, targetObject.transform.position.z);
            StartCoroutine(RotateMoveRotate(targetObject, 
            Quaternion.Euler(0, rotationOnYaxis, 0), 
            targetPosition,
            rotationBeforeGettingIntoLane, 
            () => AlignPlanesSmoothly())); // ✅ Step 4: Smoothly align planes
        }
        else if(tag=="TakeOffPlane")
        {
            targetPosition = new Vector3(-400f, 0, triggerTakeOff.transform.position.z);
            Debug.Log("Starting takeOff coroutine...");
            StartCoroutine(takeOff(targetObject, targetPosition));
        }
        else if(!AnyPlaneIsMoving())
        {
            if (tag == "BeforeTakeOffPlane")
            {
                if (currentTriggeredPlane != null)
                {
                    targetPosition = triggerHold.transform.position; // ✅ Move forward

                    // ✅ Remove the plane from the queue before moving
                    if (planeQueue.Contains(currentTriggeredPlane))
                    {
                        planeQueue.Remove(currentTriggeredPlane);
                    }

                    StartCoroutine(RotateMoveSimultaneously(currentTriggeredPlane, targetPosition, () => AlignPlanesSmoothly()));
                }
            }
        }

    }

    public void PerformAction(GameObject targetObject, string tag, string buttonType)
    {
        if (!planeQueue.Contains(targetObject) && tag!="TakeOffPlane")
        {
            RegisterPlane(targetObject);
        }
        if(!AnyPlaneForTakeOff())
        {
            if (buttonType == "Line Up" && tag == "BeforeOnTheLinePlane")
            {
                StartCoroutine(RotateMoveSimultaneouslyForTakeOff(targetObject));// ✅ Special logic for the "Line Up" button
            }
            else if(buttonType == "IMD Take Off" && tag == "BeforeOnTheLinePlane")
            {
                StartCoroutine(ImmediateTakeOff(targetObject));
            }
        }
    }



    private IEnumerator RotateMoveSimultaneously(GameObject targetObject, Vector3 targetPosition, System.Action onComplete)
    {
        // float elapsedTime = 0f;

        // // First target position & rotation
        // Quaternion startRotation = targetObject.transform.rotation;
        // Quaternion targetRotation1 = Quaternion.Euler(0, 250f, 0);
        // Vector3 startPosition = targetObject.transform.position;
        // Vector3 beforeTargetPosition = new Vector3(-275, 0, -1086);

        // float distance1 = Vector3.Distance(startPosition, beforeTargetPosition);
        // float duration1 = (distance1 > 0.01f) ? distance1 / moveSpeed : 0.1f;
        // float angle1 = Quaternion.Angle(startRotation, targetRotation1);
        // float rotationDuration1 = angle1 / rotationSpeed;

        // // Second target position & rotation
        Quaternion targetRotation2 = Quaternion.Euler(0, rotationOnYaxis, 0);
        // float distance2 = Vector3.Distance(beforeTargetPosition, targetPosition);
        // float duration2 = (distance2 > 0.01f) ? distance2 / moveSpeed : 0.1f;
        // float angle2 = Quaternion.Angle(targetRotation1, targetRotation2);
        // float rotationDuration2 = angle2 / rotationSpeed;

        // float totalDuration = duration1 + duration2;
        // float totalRotationDuration = rotationDuration1 + rotationDuration2;

        // while (elapsedTime < Mathf.Max(totalDuration, totalRotationDuration))
        // {
        //     float t1 = Mathf.Clamp01(elapsedTime / Mathf.Max(duration1, rotationDuration1)); // Normalize phase 1
        //     float t2 = Mathf.Clamp01((elapsedTime - duration1) / Mathf.Max(duration2, rotationDuration2)); // Normalize phase 2

        //     if (elapsedTime < duration1)
        //     {
        //         // Phase 1 movement and rotation
        //         targetObject.transform.position = Vector3.Lerp(startPosition, beforeTargetPosition, t1);
        //         targetObject.transform.rotation = Quaternion.Slerp(startRotation, targetRotation1, t1);
        //     }
        //     else
        //     {
        //         // Phase 2 movement and rotation
        //         targetObject.transform.position = Vector3.Lerp(beforeTargetPosition, targetPosition, t2);
        //         targetObject.transform.rotation = Quaternion.Slerp(targetRotation1, targetRotation2, t2);
        //     }

        //     elapsedTime += Time.deltaTime;
        //     yield return null;
        // }
        yield return StartCoroutine(MoveAlongBezier(targetObject, targetObject.transform.position,targetPosition,targetObject.transform.rotation,targetRotation2));
        // Ensure final values are set exactly
        targetObject.transform.position = targetPosition;
        targetObject.transform.rotation = targetRotation2;
        targetObject.tag = "BeforeOnTheLinePlane";

        if (planeQueue.Contains(targetObject))
        {
            planeQueue.Remove(targetObject);
        }

        onComplete?.Invoke();
    }
    private IEnumerator ImmediateTakeOff(GameObject targetObject)
    {

        targetObject.tag="PlaneTookOff";
        float elapsedTime = 0f;

        // // First target position & rotation
        Vector3 targetPosition = triggerObjectBeforeTakeOff.transform.position;
        // Quaternion startRotation = targetObject.transform.rotation;
        // Quaternion targetRotation1 = Quaternion.Euler(0, 310f, 0);
        // Vector3 startPosition = targetObject.transform.position;
        // Vector3 beforeTargetPosition = new Vector3(-375, 0, -1070);

        // float distance1 = Vector3.Distance(startPosition, beforeTargetPosition);
        // float duration1 = (distance1 > 0.01f) ? distance1 / moveSpeed : 0.1f;
        // float angle1 = Quaternion.Angle(startRotation, targetRotation1);
        // float rotationDuration1 = angle1 / rotationSpeed;

        // // Second target position & rotation
        Quaternion targetRotation2 = Quaternion.Euler(0, 360f, 0);
        // float distance2 = Vector3.Distance(beforeTargetPosition, targetPosition);
        // float duration2 = (distance2 > 0.01f) ? distance2 / moveSpeed : 0.1f;
        // float angle2 = Quaternion.Angle(targetRotation1, targetRotation2);
        // float rotationDuration2 = angle2 / rotationSpeed;

        // float totalDuration = duration1 + duration2;
        // float totalRotationDuration = rotationDuration1 + rotationDuration2;

        // while (elapsedTime < Mathf.Max(totalDuration, totalRotationDuration))
        // {
        //     float t1 = Mathf.Clamp01(elapsedTime / Mathf.Max(duration1, rotationDuration1)); // Normalize phase 1
        //     float t2 = Mathf.Clamp01((elapsedTime - duration1) / Mathf.Max(duration2, rotationDuration2)); // Normalize phase 2

        //     if (elapsedTime < duration1)
        //     {
        //         // Phase 1 movement and rotation
        //         targetObject.transform.position = Vector3.Lerp(startPosition, beforeTargetPosition, t1);
        //         targetObject.transform.rotation = Quaternion.Slerp(startRotation, targetRotation1, t1);
        //     }
        //     else
        //     {
        //         // Phase 2 movement and rotation
        //         targetObject.transform.position = Vector3.Lerp(beforeTargetPosition, targetPosition, t2);
        //         targetObject.transform.rotation = Quaternion.Slerp(targetRotation1, targetRotation2, t2);
        //     }

        //     elapsedTime += Time.deltaTime;
        //     yield return null;
        // }
        yield return StartCoroutine(MoveAlongBezier(targetObject, targetObject.transform.position, targetPosition, targetObject.transform.rotation, targetRotation2));

        Vector3 initialPosition = targetObject.transform.position;
        elapsedTime = 0f;
        Vector3 initialTakeoffPosition = new Vector3(-400f, 0, triggerTakeOff.transform.position.z);
        float distance = Vector3.Distance(initialPosition, initialTakeoffPosition);
        float duration = (distance > 0.01f) ? distance / moveSpeed : 0.1f;

        // Step 1: Move forward to takeoff position
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            targetObject.transform.position = Vector3.Lerp(initialPosition, initialTakeoffPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetObject.transform.position = initialTakeoffPosition;

        // Step 2: Slight ascent (rotation and upward movement)
        Vector3 climbPosition = new Vector3(initialTakeoffPosition.x, initialTakeoffPosition.y + 10, initialTakeoffPosition.z + 100);
        Quaternion climbRotation = Quaternion.Euler(-15, 0, 0);
        distance = Vector3.Distance(initialTakeoffPosition, climbPosition);
        duration = (distance > 0.01f) ? distance / moveSpeed : 0.1f;
        float angle = Quaternion.Angle(targetObject.transform.rotation, climbRotation);
        float rotationDuration = angle / rotationSpeed;
        elapsedTime = 0f;

        while (elapsedTime < Mathf.Max(duration, rotationDuration))
        {
            float t = elapsedTime / Mathf.Max(duration, rotationDuration);
            targetObject.transform.rotation = Quaternion.Slerp(targetObject.transform.rotation, climbRotation, t);
            targetObject.transform.position = Vector3.Lerp(initialTakeoffPosition, climbPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetObject.transform.position = climbPosition;
        targetObject.transform.rotation = climbRotation;

        // Step 3: Ascend into the sky
        Vector3 finalPosition = new Vector3(climbPosition.x, climbPosition.y + 500, climbPosition.z + 3500);
        distance = Vector3.Distance(climbPosition, finalPosition);
        duration = (distance > 0.01f) ? distance / moveSpeed : 0.1f;
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            targetObject.transform.position = Vector3.Lerp(climbPosition, finalPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetObject.transform.position = finalPosition;

        Destroy(targetObject);
    }
    private IEnumerator RotateMoveSimultaneouslyForTakeOff(GameObject targetObject)
    {
        // float elapsedTime = 0f;

        // // First target position & rotation
        Vector3 targetPosition = triggerObjectBeforeTakeOff.transform.position;
        // Quaternion startRotation = targetObject.transform.rotation;
        // Quaternion targetRotation1 = Quaternion.Euler(0, 310f, 0);
        // Vector3 startPosition = targetObject.transform.position;
        // Vector3 beforeTargetPosition = new Vector3(-375, 0, -1070);

        // float distance1 = Vector3.Distance(startPosition, beforeTargetPosition);
        // float duration1 = (distance1 > 0.01f) ? distance1 / moveSpeed : 0.1f;
        // float angle1 = Quaternion.Angle(startRotation, targetRotation1);
        // float rotationDuration1 = angle1 / rotationSpeed;

        // // Second target position & rotation
        Quaternion targetRotation2 = Quaternion.Euler(0, 360f, 0);
        // float distance2 = Vector3.Distance(beforeTargetPosition, targetPosition);
        // float duration2 = (distance2 > 0.01f) ? distance2 / moveSpeed : 0.1f;
        // float angle2 = Quaternion.Angle(targetRotation1, targetRotation2);
        // float rotationDuration2 = angle2 / rotationSpeed;

        // float totalDuration = duration1 + duration2;
        // float totalRotationDuration = rotationDuration1 + rotationDuration2;

        // while (elapsedTime < Mathf.Max(totalDuration, totalRotationDuration))
        // {
        //     float t1 = Mathf.Clamp01(elapsedTime / Mathf.Max(duration1, rotationDuration1)); // Normalize phase 1
        //     float t2 = Mathf.Clamp01((elapsedTime - duration1) / Mathf.Max(duration2, rotationDuration2)); // Normalize phase 2

        //     if (elapsedTime < duration1)
        //     {
        //         // Phase 1 movement and rotation
        //         targetObject.transform.position = Vector3.Lerp(startPosition, beforeTargetPosition, t1);
        //         targetObject.transform.rotation = Quaternion.Slerp(startRotation, targetRotation1, t1);
        //     }
        //     else
        //     {
        //         // Phase 2 movement and rotation
        //         targetObject.transform.position = Vector3.Lerp(beforeTargetPosition, targetPosition, t2);
        //         targetObject.transform.rotation = Quaternion.Slerp(targetRotation1, targetRotation2, t2);
        //     }

        //     elapsedTime += Time.deltaTime;
        //     yield return null;
        // }


        yield return StartCoroutine(MoveAlongBezier(targetObject, targetObject.transform.position, targetPosition, targetObject.transform.rotation, targetRotation2));
        // Ensure final values are set exactly
        targetObject.transform.position = targetPosition;
        targetObject.transform.rotation = targetRotation2;
        targetObject.tag = "TakeOffPlane";

        if (planeQueue.Contains(targetObject))
        {
            planeQueue.Remove(targetObject);
        }
    }


    private IEnumerator RotateMoveRotate(GameObject targetObject, Quaternion firstRotation, Vector3 targetPosition, Quaternion finalRotation, System.Action onComplete)
    {
        targetObject.tag = "Processing";

        // Step 1: First Rotation
        Quaternion initialRotation = targetObject.transform.rotation;
        float angle = Quaternion.Angle(initialRotation, firstRotation);
        float rotationDuration = angle / rotationSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            float t = elapsedTime / rotationDuration;
            targetObject.transform.rotation = Quaternion.Slerp(initialRotation, firstRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetObject.transform.rotation = firstRotation;

        // Step 2: Move
        Vector3 initialPosition = targetObject.transform.position;
        elapsedTime = 0f;
        float distance = Vector3.Distance(initialPosition, targetPosition);
        float duration = (distance > 0.01f) ? distance / moveSpeed : 0.1f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            targetObject.transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetObject.transform.position = targetPosition;

        // Step 3: Final Rotation
        // elapsedTime = 0f;
        // Quaternion rotationAfterMove = targetObject.transform.rotation;
        // float angle2 = Quaternion.Angle(rotationAfterMove, finalRotation);
        // float rotationDuration2 = angle2 / rotationSpeed;

        // while (elapsedTime < rotationDuration2)
        // {
        //     float t = elapsedTime / rotationDuration2;
        //     targetObject.transform.rotation = Quaternion.Slerp(rotationAfterMove, finalRotation, t);
        //     elapsedTime += Time.deltaTime;
        //     yield return null;
        // }
        yield return StartCoroutine(MoveAlongBezier(targetObject, targetObject.transform.position,new Vector3(-225, 0f, targetObject.transform.position.z-50),targetObject.transform.rotation,finalRotation));
        targetObject.transform.rotation = finalRotation;

        targetObject.tag = "ProcessingCompletion";

        onComplete?.Invoke();
    }
    
    private IEnumerator takeOff(GameObject targetObject, Vector3 initialTakeoffPosition)
    {
        targetObject.tag = "PlaneTookOff";
        Vector3 initialPosition = targetObject.transform.position;
        float elapsedTime = 0f;
        float distance = Vector3.Distance(initialPosition, initialTakeoffPosition);
        float duration = (distance > 0.01f) ? distance / moveSpeed : 0.1f;

        // Step 1: Move forward to takeoff position
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            targetObject.transform.position = Vector3.Lerp(initialPosition, initialTakeoffPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetObject.transform.position = initialTakeoffPosition;

        // Step 2: Slight ascent (rotation and upward movement)
        Vector3 climbPosition = new Vector3(initialTakeoffPosition.x, initialTakeoffPosition.y + 10, initialTakeoffPosition.z + 100);
        Quaternion climbRotation = Quaternion.Euler(-15, 0, 0);
        distance = Vector3.Distance(initialTakeoffPosition, climbPosition);
        duration = (distance > 0.01f) ? distance / moveSpeed : 0.1f;
        float angle = Quaternion.Angle(targetObject.transform.rotation, climbRotation);
        float rotationDuration = angle / rotationSpeed;
        elapsedTime = 0f;

        while (elapsedTime < Mathf.Max(duration, rotationDuration))
        {
            float t = elapsedTime / Mathf.Max(duration, rotationDuration);
            targetObject.transform.rotation = Quaternion.Slerp(targetObject.transform.rotation, climbRotation, t);
            targetObject.transform.position = Vector3.Lerp(initialTakeoffPosition, climbPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetObject.transform.position = climbPosition;
        targetObject.transform.rotation = climbRotation;

        // Step 3: Ascend into the sky
        Vector3 finalPosition = new Vector3(climbPosition.x, climbPosition.y + 500, climbPosition.z + 3500);
        distance = Vector3.Distance(climbPosition, finalPosition);
        duration = (distance > 0.01f) ? distance / moveSpeed : 0.1f;
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            targetObject.transform.position = Vector3.Lerp(climbPosition, finalPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetObject.transform.position = finalPosition;

        Destroy(targetObject);

        Debug.Log($"{targetObject.name} has successfully taken off!");
    }

    private void AlignPlanesSmoothly()
    {
        if (triggerObject == null) return;

        Vector3 firstPlanePosition = triggerObject.position; // First plane on trigger position
        StartCoroutine(SmoothMovePlanes(firstPlanePosition));
    }

    private IEnumerator SmoothMovePlanes(Vector3 firstPlanePosition)
    {
        if (planeQueue.Count == 0) yield break;

        List<GameObject> movingPlanes = new List<GameObject>();
        foreach (var plane in planeQueue)
        {
            if (plane.CompareTag("ProcessingCompletion"))
            {
                movingPlanes.Add(plane);
            }
        }
        
        if (movingPlanes.Count == 0) yield break;

        List<Vector3> targetPositions = new List<Vector3>();

        // ✅ Compute correct target positions for each plane
        for (int i = 0; i < movingPlanes.Count; i++)
        {
            Vector3 newPosition = new Vector3(
                positionOnXaxis,
                firstPlanePosition.y,
                firstPlanePosition.z - (i * planeSpacing) 
            );
            targetPositions.Add(newPosition);
        }

        float elapsedTime = 0f;
        
        // ✅ Find the farthest-moving plane
        float maxDistance = 0f;
        foreach (var plane in movingPlanes)
        {
            float d = Vector3.Distance(plane.transform.position, firstPlanePosition);
            if (d > maxDistance) maxDistance = d;
        }

        // ✅ Use consistent duration based on max distance
        float duration = maxDistance / moveSpeed;

        List<Vector3> initialPositions = new List<Vector3>();
        foreach (var plane in movingPlanes)
        {
            initialPositions.Add(plane.transform.position);
        }

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;  // ✅ Smooth movement

            for (int i = 0; i < movingPlanes.Count; i++)
            {
                if (i < targetPositions.Count)
                {
                    movingPlanes[i].transform.position = Vector3.Lerp(
                        initialPositions[i], 
                        targetPositions[i], 
                        t
                    );
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ✅ Snap to final positions
        for (int i = 0; i < movingPlanes.Count; i++)
        {
            if (i < targetPositions.Count)
            {
                movingPlanes[i].transform.position = targetPositions[i];
            }
        }

        
    }








    private bool AnyPlaneIsMoving()
    {
        return GameObject.FindGameObjectWithTag(movingPlaneTag) != null;
    }
    private bool AnyPlaneForTakeOff()
    {
        return GameObject.FindGameObjectWithTag("TakeOffPlane") != null;
    }

        private IEnumerator MoveAlongBezier(GameObject plane, Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot)
    {
        float moveSpeed = 30f;
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

            elapsed += Time.deltaTime;
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
