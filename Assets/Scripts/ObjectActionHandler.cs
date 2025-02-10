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

        // ✅ Positioning logic: First plane on trigger, others behind it
        for (int i = 0; i < planeQueue.Count; i++)
        {
            Vector3 newPosition = new Vector3(
                positionOnXaxis,
                firstPlanePosition.y,
                firstPlanePosition.z - (i * planeSpacing) // ✅ Always behind the first plane
            );
            targetPositions.Add(newPosition);
        }

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            for (int i = 0; i < planeQueue.Count; i++)
            {
                if (i < targetPositions.Count)
                {
                    planeQueue[i].transform.position = Vector3.Lerp(planeQueue[i].transform.position, targetPositions[i], elapsedTime);
                }
            }
            elapsedTime += Time.deltaTime * finalMoveSpeed;
            yield return null;
        }

        // Ensure final positions are set exactly
        for (int i = 0; i < planeQueue.Count; i++)
        {
            if (i < targetPositions.Count)
            {
                planeQueue[i].transform.position = targetPositions[i];
            }
        }
    }

    public void OnFrontPlaneMove()
    {
        if (planeQueue.Count > 0)
        {
            planeQueue.RemoveAt(0); // ✅ Remove first plane from queue
            AlignPlanesSmoothly(); // ✅ Shift all planes forward
        }
    }
}
