using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    // LateUpdate is called after all Update functions.
    // This is the best place for camera-related logic.
    void LateUpdate()
    {
        if (cameraTransform != null)
        {
            // Rotate the UI to face the same direction as the camera
            transform.rotation = cameraTransform.rotation;
        }
    }
}