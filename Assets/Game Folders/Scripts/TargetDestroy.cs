using UnityEngine;

public class TargetDestroy : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Pastikan hanya tongkat yang bisa menghancurkan
        if (collision.gameObject.CompareTag("Spear"))
        {
            // Tambah Skor ke GameManager sebelum hancur
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(1);
            }

            // Hancurkan cube
            Destroy(gameObject);
        }
    }
}