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
            targetPosition = new Vector3(positionOnXaxis, 0, targetObject.transform.position.z);
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
                    targetPosition = new Vector3(-260f, 0, currentTriggeredPlane.transform.position.z); // ✅ Move forward

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

        if (buttonType == "Line Up" && tag == "BeforeOnTheLinePlane")
        {
            AlignAndRotatePlanesSmoothly(); // ✅ Special logic for the "Line Up" button
        }
        // else if (buttonType == "TakeOff" && tag == "BeforeTakeOffPlane" && !AnyPlaneIsMoving())
        // {
        //     if (currentTriggeredPlane != null)
        //     {
        //         Vector3 targetPosition = new Vector3(-260f, 0, currentTriggeredPlane.transform.position.z);

        //         if (planeQueue.Contains(currentTriggeredPlane))
        //         {
        //             planeQueue.Remove(currentTriggeredPlane);
        //         }

        //         StartCoroutine(RotateMoveSimultaneously(currentTriggeredPlane, targetPosition, () => AlignPlanesSmoothly()));
        //     }
        // }
    }



    private IEnumerator RotateMoveSimultaneously(GameObject targetObject, Vector3 targetPosition, System.Action onComplete)
    {
        float elapsedTime = 0f;
        Quaternion startRotation = targetObject.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, rotationOnYaxis, 0);
        Vector3 startPosition = targetObject.transform.position;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float duration = (distance > 0.01f) ? distance / moveSpeed : 0.1f;
        float angle = Quaternion.Angle(startRotation, targetRotation);
        float rotationDuration = angle / rotationSpeed;
        
        while (elapsedTime < Mathf.Max(duration, rotationDuration))
        {
            float t = elapsedTime / Mathf.Max(duration, rotationDuration); // Normalize time
            targetObject.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            targetObject.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetObject.transform.rotation = targetRotation;
        targetObject.transform.position = targetPosition;
        targetObject.tag = "BeforeOnTheLinePlane";

        if (planeQueue.Contains(targetObject))
        {
            planeQueue.Remove(targetObject);
        }

        UpdateAlignedPlanes(targetObject);
        onComplete?.Invoke();
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
        elapsedTime = 0f;
        Quaternion rotationAfterMove = targetObject.transform.rotation;
        float angle2 = Quaternion.Angle(rotationAfterMove, finalRotation);
        float rotationDuration2 = angle2 / rotationSpeed;

        while (elapsedTime < rotationDuration2)
        {
            float t = elapsedTime / rotationDuration2;
            targetObject.transform.rotation = Quaternion.Slerp(rotationAfterMove, finalRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetObject.transform.rotation = finalRotation;

        onComplete?.Invoke();
    }

    private IEnumerator takeOff(GameObject targetObject, Vector3 initialTakeoffPosition)
    {
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
        Vector3 climbPosition = new Vector3(initialTakeoffPosition.x, initialTakeoffPosition.y + 10, initialTakeoffPosition.z + 50);
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
        Vector3 finalPosition = new Vector3(climbPosition.x, climbPosition.y + 500, climbPosition.z + 3000);
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
        if (planeQueue.Count == 0) yield break; // ✅ Avoid running when no planes exist

        // ✅ Exclude planes that are already aligned (BeforeOnTheLinePlane)
        List<GameObject> movingPlanes = new List<GameObject>();
        foreach (var plane in planeQueue)
        {
            if (!plane.CompareTag("TakeOffPlane"))
            {
                movingPlanes.Add(plane);
            }
        }

        if (movingPlanes.Count == 0) yield break; // ✅ No valid planes to move

        List<Vector3> targetPositions = new List<Vector3>();

        // ✅ Determine target positions only for valid planes
        for (int i = 0; i < movingPlanes.Count; i++)
        {
            Vector3 newPosition = new Vector3(
                positionOnXaxis,
                firstPlanePosition.y,
                firstPlanePosition.z - (i * planeSpacing) // ✅ Ensures correct spacing
            );
            targetPositions.Add(newPosition);
        }

        float elapsedTime = 0f;
        float duration = 2.5f; // ✅ Increased duration for smoother movement

    while (elapsedTime < duration)
    {
        float t = Mathf.SmoothStep(0, 1, elapsedTime / duration); // ✅ Smooth transition

        for (int i = 0; i < movingPlanes.Count; i++)
        {
            if (i < targetPositions.Count)
            {
                movingPlanes[i].transform.position = Vector3.Lerp(
                    movingPlanes[i].transform.position, 
                    targetPositions[i], 
                    t
                );
            }
        }

        elapsedTime += Time.deltaTime;
        yield return null;
    }

    // ✅ Ensure exact final positions
    for (int i = 0; i < movingPlanes.Count; i++)
    {
        if (i < targetPositions.Count)
        {
            movingPlanes[i].transform.position = targetPositions[i];
        }
    }
}

    private void AlignAndRotatePlanesSmoothly()
    {
        if (triggerObjectBeforeTakeOff == null) return;

        Vector3 firstPlanePosition = triggerObjectBeforeTakeOff.position;
        StartCoroutine(SmoothRotateAndMovePlanes(firstPlanePosition));
    }

    private List<GameObject> alignedPlaneQueue = new List<GameObject>(); // ✅ Queue for aligned planes
    public void UpdateAlignedPlanes(GameObject plane)
    {
        if (plane.CompareTag("BeforeOnTheLinePlane"))
        {
            if (planeQueue.Contains(plane))
            {
                planeQueue.Remove(plane); // ✅ Remove from queue to prevent unwanted movement
            }

            if (!alignedPlaneQueue.Contains(plane))
            {
                alignedPlaneQueue.Add(plane); // ✅ Add to aligned queue if needed
            }
        }
    }

    private IEnumerator SmoothRotateAndMovePlanes(Vector3 firstPlanePosition)
    {
        if (alignedPlaneQueue.Count == 0) yield break;

        List<GameObject> rotatingPlanes = new List<GameObject>(alignedPlaneQueue);
        alignedPlaneQueue.Clear(); 

        Dictionary<GameObject, Quaternion> targetRotations = new Dictionary<GameObject, Quaternion>();
        List<Vector3> targetPositions = new List<Vector3>();

        // ✅ Make sure the first plane is correctly positioned
        Vector3 leaderPlanePosition = firstPlanePosition; 

        for (int i = 0; i < rotatingPlanes.Count; i++)
        {
            GameObject plane = rotatingPlanes[i];

            targetRotations[plane] = Quaternion.Euler(0, 360f, 0);

            Vector3 newPosition;
            if (i == 0)
            {
                // ✅ First plane should stay at its given position
                newPosition = leaderPlanePosition;
            }
            else
            {
                // ✅ Next planes should position **behind** the first plane
                newPosition = new Vector3(
                    leaderPlanePosition.x,
                    leaderPlanePosition.y,
                    targetPositions[i - 1].z - planeSpacing // 🔥 Ensures sequential spacing
                );
            }

            targetPositions.Add(newPosition);
            plane.tag = "TakeOffPlane";

            Debug.Log($"Plane {i} target position: {newPosition}"); // 🔍 Debugging output
        }

        float elapsedTime = 0f;
        float duration = 2.5f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration; // ✅ Use linear interpolation for better control

            for (int i = 0; i < rotatingPlanes.Count; i++)
            {
                GameObject plane = rotatingPlanes[i];

                if (i < targetPositions.Count)
                {
                    Vector3 startPos = plane.transform.position;
                    Vector3 endPos = targetPositions[i];

                    plane.transform.position = Vector3.Lerp(startPos, endPos, t);
                }

                if (targetRotations.ContainsKey(plane))
                {
                    plane.transform.rotation = Quaternion.Slerp(
                        plane.transform.rotation, targetRotations[plane], t * 2
                    );
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ✅ Ensure planes snap to correct positions
        for (int i = 0; i < rotatingPlanes.Count; i++)
        {
            rotatingPlanes[i].transform.position = targetPositions[i];

            if (targetRotations.ContainsKey(rotatingPlanes[i]))
            {
                rotatingPlanes[i].transform.rotation = targetRotations[rotatingPlanes[i]];
            }
        }
    }







    private bool AnyPlaneIsMoving()
    {
        return GameObject.FindGameObjectWithTag(movingPlaneTag) != null;
    }
}
