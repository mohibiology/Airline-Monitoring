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
        if (!planeQueue.Contains(targetObject))
        {
            RegisterPlane(targetObject);
        }

        Vector3 targetPosition = Vector3.zero;

        if (tag == "BeforeLineUpPlane1")
        {
            targetPosition = new Vector3(positionOnXaxis, 0, -300);
            StartCoroutine(RotateMoveRotate(targetObject, 
            Quaternion.Euler(0, rotationOnYaxis, 0), 
            targetPosition,
            rotationBeforeGettingIntoLane, 
            () => AlignPlanesSmoothly())); // ✅ Step 4: Smoothly align planes
        }
        else if (tag == "BeforeLineUpPlane2")
        {
            targetPosition = new Vector3(positionOnXaxis, 0, -525);
            StartCoroutine(RotateMoveRotate(targetObject, 
            Quaternion.Euler(0, rotationOnYaxis, 0), 
            targetPosition,
            rotationBeforeGettingIntoLane, 
            () => AlignPlanesSmoothly())); // ✅ Step 4: Smoothly align planes
        }
        else if (tag == "BeforeLineUpPlane3")
        {
            targetPosition = new Vector3(positionOnXaxis, 0, -750);
            StartCoroutine(RotateMoveRotate(targetObject, 
            Quaternion.Euler(0, rotationOnYaxis, 0), 
            targetPosition,
            rotationBeforeGettingIntoLane, 
            () => AlignPlanesSmoothly())); // ✅ Step 4: Smoothly align planes
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


    private IEnumerator RotateMoveSimultaneously(GameObject targetObject, Vector3 targetPosition, System.Action onComplete)
    {
        float elapsedTime = 0f;
        Quaternion startRotation = targetObject.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, rotationOnYaxis, 0);
        Vector3 startPosition = targetObject.transform.position;

        while (elapsedTime < 1f)
        {
            // ✅ Rotate faster than movement
            targetObject.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime * 10); // Faster rotation
            targetObject.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime); // Normal movement

            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }

        // ✅ Ensure final position and rotation are exact
        targetObject.transform.rotation = targetRotation;
        targetObject.transform.position = targetPosition;
        targetObject.tag = "BeforeOnTheLinePlane";

        onComplete?.Invoke(); // ✅ Call next step
    }


    private IEnumerator RotateMoveRotate(GameObject targetObject, Quaternion firstRotation, Vector3 targetPosition, Quaternion finalRotation, System.Action onComplete)
    {
        targetObject.tag="Processing";
        Quaternion initialRotation = targetObject.transform.rotation;
        float elapsedTime = 0f;

        // Step 1: First Rotation
        while (elapsedTime < 1f)
        {
            targetObject.transform.rotation = Quaternion.Slerp(initialRotation, firstRotation, elapsedTime);
            elapsedTime += Time.deltaTime * rotationSpeed;
            yield return null;
        }
        targetObject.transform.rotation = firstRotation;

        // Step 2: Move
        Vector3 initialPosition = targetObject.transform.position;
        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            targetObject.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }
        targetObject.transform.position = targetPosition;

        // Step 3: Final Rotation
        elapsedTime = 0f;
        Quaternion rotationAfterMove = targetObject.transform.rotation;
        while (elapsedTime < 1f)
        {
            targetObject.transform.rotation = Quaternion.Slerp(rotationAfterMove, finalRotation, elapsedTime);
            elapsedTime += Time.deltaTime * rotationSpeed;
            yield return null;
        }
        targetObject.transform.rotation = finalRotation;

        // ✅ Step 4: Smoothly align planes
        onComplete?.Invoke();
    }

    private void AlignPlanesSmoothly()
    {
        if (triggerObject == null) return;

        Vector3 firstPlanePosition = triggerObject.position; // First plane on trigger position
        StartCoroutine(SmoothMovePlanes(firstPlanePosition));
    }

    private IEnumerator SmoothMovePlanes(Vector3 firstPlanePosition)
    {
        List<Vector3> targetPositions = new List<Vector3>();

        // ✅ Determine new positions for planes
        for (int i = 0; i < planeQueue.Count; i++)
        {
            Vector3 newPosition = new Vector3(
                positionOnXaxis,
                firstPlanePosition.y,
                firstPlanePosition.z - (i * planeSpacing) // ✅ Ensures planes move sequentially
            );
            targetPositions.Add(newPosition);
        }

        float elapsedTime = 0f;
        float duration = 2.5f; // ✅ Increased duration for slower movement (adjust as needed)

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration; // ✅ Normalize time (0 to 1)
            t = Mathf.SmoothStep(0, 1, t); // ✅ Smooth out movement for a more natural effect

            for (int i = 0; i < planeQueue.Count; i++)
            {
                if (i < targetPositions.Count)
                {
                    planeQueue[i].transform.position = Vector3.Lerp(planeQueue[i].transform.position, targetPositions[i], t);
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ✅ Ensure final positions are set exactly
        for (int i = 0; i < planeQueue.Count; i++)
        {
            if (i < targetPositions.Count)
            {
                planeQueue[i].transform.position = targetPositions[i];
            }
        }
    }

    private bool AnyPlaneIsMoving()
    {
        return GameObject.FindGameObjectWithTag(movingPlaneTag) != null;
    }
}
