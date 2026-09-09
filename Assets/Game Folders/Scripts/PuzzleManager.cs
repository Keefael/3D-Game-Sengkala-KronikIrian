using UnityEngine;
using TMPro; // ✅ Menggunakan TextMeshPro

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    [Header("Pengaturan Game")]
    public int totalPieces = 24; // Total pieces yang harus dipasang
    public float gameTime = 120f; // Waktu dalam detik (misal 120 detik = 2 menit)

    [Header("UI References (Drag dari Inspector)")]
    public TextMeshProUGUI timerText; // ✅ Tipe data untuk TextMeshPro UI
    public GameObject winPanel;       // Panel Menang
    public GameObject losePanel;      // Panel Kalah

    private PuzzlePiece selectedPiece;
    private int snappedCount = 0;
    private float currentTime;
    private bool isGameActive = true;

    void Awake()
    {
        Instance = this;
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

    // --- Logika Select/Deselect Piece ---
    public void SelectPiece(PuzzlePiece piece)
    {
        if (!isGameActive) return; // Tidak bisa pilih piece jika game selesai

        if (selectedPiece != null && selectedPiece != piece)
        {
            selectedPiece.Deselect();
        }

        selectedPiece = piece;
        piece.Select();
    }

    public void Deselect()
    {
        if (selectedPiece != null)
        {
            selectedPiece.Deselect();
            selectedPiece = null;
        }
    }

    public PuzzlePiece GetSelectedPiece()
    {
        return selectedPiece;
    }

    // --- LOGIKA: Dipanggil saat piece berhasil snap ---
    public void OnPieceSnapped()
    {
        if (!isGameActive) return;

        snappedCount++;
        Debug.Log($"Pieces terpasang: {snappedCount} / {totalPieces}");

        // Cek jika semua pieces sudah terpasang
        if (snappedCount >= totalPieces)
        {
            GameOver(true); // Menang
        }
    }

    // --- Logika Menang / Kalah ---
    void GameOver(bool isWin)
    {
        isGameActive = false; // Hentikan game

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
}