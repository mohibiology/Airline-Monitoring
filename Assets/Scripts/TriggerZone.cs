using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ProcessingCompletion")
        {
            Debug.Log($"🚀 Triggered by: {other.gameObject.name}");
            other.gameObject.tag = "BeforeTakeOffPlane";

            // ✅ Set the triggered plane for the button action
            ObjectActionHandler.Instance.SetTriggeredPlane(other.gameObject);
        }
    }
}
