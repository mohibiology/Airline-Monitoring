using UnityEngine;

public class MouseControl : MonoBehaviour
{
    public Vector2 turn;
    [SerializeField] float sensitivity = 1f;
    [SerializeField] float minY = -80f; // Min vertical rotation
    [SerializeField] float maxY = 80f;  // Max vertical rotation
    [SerializeField] float minX = -90f; // Min horizontal rotation (optional)
    [SerializeField] float maxX = 90f;  // Max horizontal rotation (optional)

    [SerializeField] Camera playerCamera;
    [SerializeField] float zoomSpeed = 5f;
    [SerializeField] float minZoom = 20f;  // Min FOV for zoom-in
    [SerializeField] float maxZoom = 60f;  // Max FOV for zoom-out

    void Update() 
    {
        // Mouse Look Controls
        turn.x += Input.GetAxis("Mouse X") * sensitivity;
        turn.y += Input.GetAxis("Mouse Y") * sensitivity;

        turn.x = Mathf.Clamp(turn.x, minX, maxX); // Clamp horizontal rotation
        turn.y = Mathf.Clamp(turn.y, minY, maxY); // Clamp vertical rotation

        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);

        // Zooming Feature using Mouse Scroll Wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (playerCamera != null) 
        {
            playerCamera.fieldOfView -= scroll * zoomSpeed;
            playerCamera.fieldOfView = Mathf.Clamp(playerCamera.fieldOfView, minZoom, maxZoom);
        }
    }
}
