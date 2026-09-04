using UnityEngine;

public class FollowBoat : MonoBehaviour
{
    public Transform boat;
    public Vector3 offset = new Vector3(0, 8, -12); // Atur jarak kamera di sini
    public float smoothSpeed = 0.1f; // Semakin kecil semakin smooth (0.01 - 0.2)

    void LateUpdate()
    {
        if (boat != null)
        {
            Vector3 desiredPosition = boat.position + offset;
            // Lerp membuat pergerakan kamera halus, tidak langsung teleport
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.LookAt(boat.position);
        }
    }
}