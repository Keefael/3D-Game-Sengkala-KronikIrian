using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Game Settings")]
    public float gameTime = 60f;      // Durasi permainan
    public int winScore = 8;          // Minimal skor untuk menang
    public int maxTargets = 10;       // Total target di scene (untuk instant win)

    [Header("UI References")]
    public TextMeshProUGUI scoreText; 
    public TextMeshProUGUI timerText; 
    
    [Header("Panels")]
    public GameObject winPanel;       // Drag Panel Menang kesini
    public GameObject losePanel;      // Drag Panel Kalah kesini

    private float currentTime;
    private int currentScore;
    private bool isGameActive;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        ResetGame();
    }

    void Update()
    {
        if (!isGameActive) return;

        // Kurangi waktu
        currentTime -= Time.deltaTime;

        // Update UI Timer
        if (timerText != null)
        {
            timerText.text = $"Waktu: {Mathf.Ceil(currentTime)}";
        }

        // Cek jika waktu habis
        if (currentTime <= 0)
        {
            EndGameByTime();
        }
    }

    // Dipanggil oleh TargetDestroy saat cube hancur
    public void AddScore(int amount)
    {
        if (!isGameActive) return;

        currentScore += amount;
        
        // Update UI Skor
        if (scoreText != null)
        {
            scoreText.text = $"Skor: {currentScore}";
        }

        // LOGIKA INSTANT WIN: Jika semua target kena, langsung menang!
        if (currentScore >= maxTargets)
        {
            Debug.Log("[GAME] PERFECT! Semua target hancur!");
            ShowWinPanel();
        }
    }

    void ResetGame()
    {
        currentTime = gameTime;
        currentScore = 0;
        isGameActive = true;

        // Reset UI & Sembunyikan Panel
        if (scoreText != null) scoreText.text = "Skor: 0";
        if (timerText != null) timerText.text = $"Waktu: {gameTime}";
        
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        Debug.Log("[GAME] Permainan Dimulai!");
    }

    // Dipanggil saat waktu habis
    void EndGameByTime()
    {
        isGameActive = false;
        currentTime = 0;
        if (timerText != null) timerText.text = "Waktu: 0";

        // Cek kondisi menang/kalah berdasarkan skor akhir
        if (currentScore >= winScore)
        {
            ShowWinPanel();
        }
        else
        {
            ShowLosePanel();
        }
    }

    void ShowWinPanel()
    {
        isGameActive = false; // Stop game
        if (winPanel != null) winPanel.SetActive(true);
        Debug.Log($"[GAME] MENANG! Skor Akhir: {currentScore}");
    }

    void ShowLosePanel()
    {
        isGameActive = false; // Stop game
        if (losePanel != null) losePanel.SetActive(true);
        Debug.Log($"[GAME] KALAH! Skor Akhir: {currentScore} (Min: {winScore})");
    }
}