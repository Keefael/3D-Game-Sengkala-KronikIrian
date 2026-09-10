using UnityEngine;

public class CrosshairManager : MonoBehaviour
{
    [Header("Crosshair Settings")]
    public Camera fpsCamera;
    public LayerMask targetLayer; // Layer buah/target (opsional, biar lebih presisi)
    public float maxThrowDistance = 50f; // Jarak maksimal lempar tombak

    [Header("Debug (Opsional)")]
    public bool showDebugRay = true;

    private void Start()
    {
        // Kalau fpsCamera belum di-set di Inspector, pakai Camera.main
        if (fpsCamera == null)
        {
            fpsCamera = Camera.main;
        }
    }

    /// <summary>
    /// Dapatkan titik tengah layar (crosshair position) dalam bentuk Vector3 world position.
    /// Ini yang dipakai untuk nentuin arah lempar tombak.
    /// </summary>
    public Vector3 GetCrosshairTargetPoint()
    {
        // Raycast dari tengah layar (crosshair)
        Ray ray = fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        
        if (Physics.Raycast(ray, out RaycastHit hit, maxThrowDistance, targetLayer))
        {
            // Kena target (buah)
            if (showDebugRay)
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 0.5f);
            }
            return hit.point;
        }
        else
        {
            // Tidak kena apa-apa, kembalikan titik di jarak maksimal
            Vector3 farPoint = ray.origin + ray.direction * maxThrowDistance;
            if (showDebugRay)
            {
                Debug.DrawRay(ray.origin, ray.direction * maxThrowDistance, Color.red, 0.5f);
            }
            return farPoint;
        }
    }

    /// <summary>
    /// Dapatkan arah lempar dari crosshair (normalized direction).
    /// </summary>
    public Vector3 GetCrosshairDirection()
    {
        Ray ray = fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        return ray.direction.normalized;
    }

    /// <summary>
    /// Cek apakah crosshair sedang menarget objek tertentu (buah).
    /// </summary>
    public bool IsCrosshairOnTarget(out GameObject targetObject)
    {
        Ray ray = fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        
        if (Physics.Raycast(ray, out RaycastHit hit, maxThrowDistance, targetLayer))
        {
            targetObject = hit.collider.gameObject;
            return true;
        }
        
        targetObject = null;
        return false;
    }
}