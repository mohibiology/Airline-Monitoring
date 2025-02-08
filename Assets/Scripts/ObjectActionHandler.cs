using UnityEngine;
using System.Collections;

public class ObjectActionHandler : MonoBehaviour
{
    public static ObjectActionHandler Instance;

    [SerializeField] public float rotationSpeed = 5f; // Rotation speed
    [SerializeField] public float moveSpeed = 2f; // Movement speed
    [SerializeField] public float rotationOnYaxis = 270f;
    [SerializeField] public float positionOnXaxis = -70f;

    private Coroutine currentCoroutine = null; // ✅ Track coroutine

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PerformAction(GameObject targetObject, string tag)
    {
        if (tag == "BeforeLineUpPlane1")
        {
            StartCoroutine(RotateThenMove(targetObject, Quaternion.Euler(0, rotationOnYaxis, 0), new Vector3(positionOnXaxis, 0, -300)));
        }
        else if (tag == "BeforeLineUpPlane2")
        {
            StartCoroutine(RotateThenMove(targetObject, Quaternion.Euler(0, rotationOnYaxis, 0), new Vector3(positionOnXaxis, 0, -525)));
        }
        else if (tag == "BeforeLineUpPlane3")
        {
            StartCoroutine(RotateThenMove(targetObject, Quaternion.Euler(0, rotationOnYaxis, 0), new Vector3(positionOnXaxis, 0, -750)));
        }
    }

    private IEnumerator RotateThenMove(GameObject targetObject, Quaternion targetRotation, Vector3 targetPosition)
    {
        Quaternion initialRotation = targetObject.transform.rotation;
        float elapsedTime = 0f;

        // Step 1: Rotate first
        while (elapsedTime < 1f)
        {
            targetObject.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime);
            elapsedTime += Time.deltaTime * rotationSpeed;
            yield return null;
        }
        targetObject.transform.rotation = targetRotation;

        // Step 2: Move after rotation completes
        Vector3 initialPosition = targetObject.transform.position;
        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            targetObject.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }
        targetObject.transform.position = targetPosition;
    }
}
