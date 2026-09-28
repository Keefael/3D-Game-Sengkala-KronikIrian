using UnityEngine;
using TMPro; 

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    [Header("Pengaturan Game")]
    public int totalPieces = 24; // Total pieces yang harus dipasang (Sesuaikan dengan jumlah di scene)
    public float gameTime = 120f; // Waktu dalam detik (misal 120 detik = 2 menit)

    [Header("UI References (Drag dari Inspector)")]
    public TextMeshProUGUI timerText; // Tipe data untuk TextMeshPro UI
    public GameObject winPanel;       // Panel Menang
    public GameObject losePanel;      // Panel Kalah

    private int snappedCount = 0;
    private float currentTime;
    private bool isGameActive = true;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Inisialisasi awal
        currentTime = gameTime;
        UpdateTimerUI();
        
        // Pastikan panel menang/kalah tersembunyi di awal
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    void Update()
    {
        if (!isGameActive) return; // Stop timer jika game sudah selesai

        currentTime -= Time.deltaTime;
        UpdateTimerUI();

        // Cek jika waktu habis
        if (currentTime <= 0)
        {
            GameOver(false); // Kalah
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            
            // Format waktu jadi 00:00
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // --- LOGIKA: Dipanggil oleh PuzzlePiece saat berhasil snap ---
    public void OnPieceSnapped()
    {
        if (!isGameActive) return;

        snappedCount++;
        Debug.Log($"Pieces terpasang: {snappedCount} / {totalPieces}");

        // Opsional: Tambahkan efek suara 'pop' atau 'klik' di sini

        // Cek jika semua pieces sudah terpasang
        if (snappedCount >= totalPieces)
        {
            GameOver(true); // Menang
        }
    }

    // --- Logika Menang / Kalah ---
    void GameOver(bool isWin)
    {
        isGameActive = false; // Hentikan game dan timer

        if (isWin)
        {
            Debug.Log("KAMU MENANG!");
            if (winPanel != null) winPanel.SetActive(true);
        }
        else
        {
            Debug.Log("WAKTU HABIS! KAMU KALAH.");
            if (losePanel != null) losePanel.SetActive(true);
        }
    }

    // --- Method Tambahan untuk Tombol UI (Opsional) ---
    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}