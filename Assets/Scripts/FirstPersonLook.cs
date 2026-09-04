using UnityEngine;
using UnityEngine.InputSystem; // Ganti namespace input

public class FirstPersonLook : MonoBehaviour
{
    [Header("Settings")]
    public float mouseSensitivity = 2.0f;
    public Transform playerBody;

    private float xRotation = 0f;
    private Mouse mouse;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Ambil referensi Mouse dari Input System
        mouse = Mouse.current;
    }

    void Update()
    {
        if (mouse == null) return;

        // Baca delta pergerakan mouse (bukan GetAxis)
        Vector2 delta = mouse.delta.ReadValue();
        float mouseX = delta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = delta.y * mouseSensitivity * Time.deltaTime;

        // Rotasi Kiri-Kanan
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }

        // Rotasi Atas-Bawah
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}