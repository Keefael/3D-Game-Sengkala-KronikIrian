using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject panelWin;
    public GameObject panelLose;
    
    [Header("Timer UI")]
    public TextMeshProUGUI timerText;
    public float startTime = 90f; 
    
    [Header("Puzzle Pieces - URUTAN PENTING!")]
    // Drag pieces DI SINI dengan urutan yang benar (01, 02, 03...)
    // Jangan asal drag, harus sesuai urutan penyusunan puzzle!
    public List<DragDropPiece> sequentialPieces = new List<DragDropPiece>();
    
    private int nextPieceIndex = 1; // Index piece berikutnya yang akan muncul
    private float timeRemaining;
    private bool isGameActive = true;

    void Start()
    {
        timeRemaining = startTime;
        UpdateTimerDisplay();
        
        if (panelWin != null) panelWin.SetActive(false);
        if (panelLose != null) panelLose.SetActive(false);

        // ✅ SETUP AWAL: Hanya aktifkan piece pertama
        InitializeSequentialPieces();
    }

    void InitializeSequentialPieces()
    {
        if (sequentialPieces.Count == 0) return;

        // Matikan SEMUA pieces dulu
        foreach (var piece in sequentialPieces)
        {
            if (piece != null) piece.gameObject.SetActive(false);
        }

        // Nyalakan HANYA piece pertama (index 0)
        sequentialPieces[0].gameObject.SetActive(true);
        nextPieceIndex = 1;
        
        Debug.Log($"🧩 Piece pertama aktif: {sequentialPieces[0].name}");
    }

    void Update()
    {
        if (!isGameActive) return;

        // Timer Logic
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            TriggerLose();
        }

        // Cek Win Condition & Sequential Unlock
        CheckGameState();
    }

    void CheckGameState()
    {
        int totalSnapped = 0;
        bool lastActiveSnapped = false;

        foreach (var piece in sequentialPieces)
        {
            if (piece == null) continue;

            // Hitung total yang sudah snap (untuk win condition)
            if (piece.isSnapped) 
            {
                totalSnapped++;
                
                // Cek apakah piece yang BARU SAJA aktif ini sudah snap?
                // Kita pakai index nextPieceIndex - 1 sebagai referensi piece aktif terakhir
                if (sequentialPieces.IndexOf(piece) == nextPieceIndex - 1)
                {
                    lastActiveSnapped = true;
                }
            }
        }

        // ✅ LOGIKA UNLOCK: Jika piece aktif terakhir sudah snap, buka piece berikutnya
        if (lastActiveSnapped && nextPieceIndex < sequentialPieces.Count)
        {
            DragDropPiece nextPiece = sequentialPieces[nextPieceIndex];
            if (nextPiece != null)
            {
                nextPiece.gameObject.SetActive(true);
                Debug.Log($"✨ Piece baru terbuka: {nextPiece.name}");
                nextPieceIndex++;
            }
        }

        // Win Condition: Semua piece sudah snap
        if (totalSnapped == sequentialPieces.Count)
        {
            TriggerWin();
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TriggerWin()
    {
        isGameActive = false;
        if (panelWin != null) panelWin.SetActive(true);
        Debug.Log("🎉 MENANG! Semua puzzle selesai.");
    }

    void TriggerLose()
    {
        isGameActive = false;
        if (panelLose != null) panelLose.SetActive(true);
        Debug.Log("⏰ KALAH! Waktu habis.");
    }

    public void OnRestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnMainMenuClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}