using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowController : MonoBehaviour
{
    [Header("References")]
    public GameObject spearPrefab;
    public Transform throwPoint;

    [Header("Settings")]
    public float throwForce = 25f;
    public float throwCooldown = 0.6f; // Diperpanjang sedikit biar nggak spam

    private float lastThrowTime = -10f; // Inisialisasi negatif biar bisa langsung lempar di awal

    void Update()
    {
        // 1. Cek Cooldown: Jika belum lewat waktu jeda, STOP semua proses di bawah
        if (Time.time - lastThrowTime < throwCooldown) 
        {
            return;
        }

        // 2. Deteksi Klik Kiri Mouse HANYA SATU KALI
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Panggil fungsi lempar HANYA DI SINI
            ThrowSpear();
            
            // Reset waktu cooldown TEPAT SETELAH melempar
            lastThrowTime = Time.time; 
        }
    }

    public void ThrowSpear()
    {
        Debug.Log("[PLAYER] Melempar 1 tongkat!"); // Log buat mastiin cuma dipanggil sekali
        
        GameObject spear = Instantiate(spearPrefab, throwPoint.position, throwPoint.rotation);

        Rigidbody rb = spear.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = throwPoint.forward * throwForce;
        }
    }
}