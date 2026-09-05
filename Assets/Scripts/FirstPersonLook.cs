using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonLook : MonoBehaviour
{
    [Header("Settings")]
    // Naikkan angka ini jika masih terasa lambat (misal: 0.5f atau 1.0f)
    public float mouseSensitivity = 0.8f; 
    public Transform playerBody;

    private float xRotation = 0f;
    private Mouse mouse;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mouse = Mouse.current;
    }

    void Update()
    {
        if (mouse == null) return;

        Vector2 delta = mouse.delta.ReadValue();
        
        // HAPUS Time.deltaTime di sini agar responsif
        float mouseX = delta.x * mouseSensitivity;
        float mouseY = delta.y * mouseSensitivity;

        // Rotasi Kiri-Kanan (Badan Player)
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }

        // Rotasi Atas-Bawah (Kamera)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Batasi agar leher tidak patah
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}