using UnityEngine;
using UnityEngine.UI; // Untuk Button UI

public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pausePanel;      // Drag Panel_Pause ke sini
    public Button btnContinue;         // Drag Btn_Continue ke sini
    
    [Header("Game Controllers")]
    public TimerController timerCtrl;  // Drag GameController (yang ada script Timer) ke sini
    public DragDropPiece[] allPieces;  // Drag semua Piece_1,2,3 ke array ini

    private bool isPaused = false;

    void Start()
    {
        // Pastikan panel tersembunyi di awal
        pausePanel.SetActive(false);
        
        // Pasang event listener tombol Continue
        btnContinue.onClick.AddListener(ResumeGame);
    }

    // Dipanggil dari tombol Pause (buat tombol pause nanti)
    public void PauseGame()
    {
        if (isPaused) return;
        
        isPaused = true;
        Time.timeScale = 0f; // Bekukan waktu game
        
        pausePanel.SetActive(true);
        
        // Nonaktifkan interaksi drag pada semua piece
        foreach (var piece in allPieces)
        {
            piece.enabled = false;
        }
    }

    // Dipanggil saat tombol Continue ditekan
    public void ResumeGame()
    {
        if (!isPaused) return;
        
        isPaused = false;
        Time.timeScale = 1f; // Kembalikan waktu normal
        
        pausePanel.SetActive(false);
        
        // Aktifkan kembali interaksi drag
        foreach (var piece in allPieces)
        {
            piece.enabled = true;
        }
    }
}