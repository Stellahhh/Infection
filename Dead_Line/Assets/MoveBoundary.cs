using UnityEngine;

public class PlayerBoundary : MonoBehaviour
{
    private Vector3 minBounds = new Vector3(394.06f, 0f, 88.5f); // Minimum bounds (X, Y, Z)
    private Vector3 maxBounds = new Vector3(542.34f, 100f, 159.58f); // Maximum bounds (X, Y, Z)

    void Update()
    {
        // Clamp player position within defined boundaries
        float clampedX = Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y);
        float clampedZ = Mathf.Clamp(transform.position.z, minBounds.z, maxBounds.z);

        // Apply clamped position
        transform.position = new Vector3(clampedX, clampedY, clampedZ);
    }
}
