using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowController : MonoBehaviour
{
    [Header("References")]
    public GameObject spearPrefab;
    public Transform throwPoint;
    public Camera fpsCamera; // Referensi kamera FPS

    [Header("Settings")]
    public float throwForce = 25f;
    public float throwCooldown = 0.6f;
    public float maxThrowDistance = 50f; // Jarak maksimal raycast mendeteksi target
    
    // Opsional: Buat Layer "Fruit" atau "Target" di Unity, lalu assign di sini 
    // agar raycast mengabaikan dinding/pemain dan hanya mendeteksi buah.
    public LayerMask targetLayer; 

    private float lastThrowTime = -10f;

    void Start()
    {
        // Jika lupa assign kamera di Inspector, pakai kamera utama secara otomatis
        if (fpsCamera == null)
        {
            fpsCamera = Camera.main;
        }
    }

    void Update()
    {
        // 1. Cek Cooldown
        if (Time.time - lastThrowTime < throwCooldown) 
        {
            return;
        }

        // 2. Deteksi Klik Kiri Mouse
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            ThrowSpear();
            lastThrowTime = Time.time; 
        }
    }

    public void ThrowSpear()
    {
        Debug.Log("[PLAYER] Melempar 1 tongkat ke arah crosshair!");
        
        // --- LOGIKA AIMING FPS (RAYCAST DARI TENGAH LAYAR) ---
        // 0.5f, 0.5f adalah koordinat tepat di tengah layar (Viewport)
        Ray ray = fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        
        Vector3 targetPosition;

        // Cek apakah ada objek yang terkena raycast dari tengah layar
        if (Physics.Raycast(ray, out RaycastHit hit, maxThrowDistance, targetLayer))
        {
            // Jika kena objek (misal: buah), targetkan tepat di titik permukaan objek tersebut
            targetPosition = hit.point;
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 1f); // Garis hijau di Scene view
        }
        else
        {
            // Jika tidak kena apa-apa (misal: nembak ke langit), targetkan lurus ke depan sejauh maxThrowDistance
            targetPosition = throwPoint.position + (fpsCamera.transform.forward * maxThrowDistance);
            Debug.DrawRay(ray.origin, ray.direction * maxThrowDistance, Color.red, 1f); // Garis merah di Scene view
        }

        // Hitung arah lemparan dari titik keluar tombak menuju titik target crosshair
        Vector3 throwDirection = (targetPosition - throwPoint.position).normalized;

        // --- INSTANSIASI & FISIKA ---
        GameObject spear = Instantiate(spearPrefab, throwPoint.position, Quaternion.identity);

        // Putar tombak agar menghadap ke arah lemparan (biar ujungnya yang maju, bukan sampingnya)
        spear.transform.rotation = Quaternion.LookRotation(throwDirection);

        Rigidbody rb = spear.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Lempar menggunakan arah yang sudah dihitung dari crosshair
            rb.linearVelocity = throwDirection * throwForce;
        }
    }
}