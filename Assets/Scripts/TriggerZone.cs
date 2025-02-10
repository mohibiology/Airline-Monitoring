using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Processing")
        {
            Debug.Log($"🚀 Triggered by: {other.gameObject.name}");
            other.gameObject.tag = "BeforeTakeOffPlane";
        }
    }
}
