using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Tambahkan ini agar bisa mengontrol tombol

public class LevelSelectManager : MonoBehaviour
{
    [Header("Panel Sub-Level Prolog")]
    public GameObject prologGameplayPanel;

    [Header("Tombol Level 2 & 3 (Untuk dikunci)")]
    // Drag tombol UI Level 2 dan 3 ke sini dari Inspector
    public Button btnLevel2; 
    public Button btnLevel3;

    void Start()
    {
        // 1. Cek apakah data level sudah ada, jika belum buat defaultnya Level 1
        if (!PlayerPrefs.HasKey("MaxLevelUnlocked"))
        {
            PlayerPrefs.SetInt("MaxLevelUnlocked", 1);
            PlayerPrefs.Save();
        }

        // 2. Jalankan fungsi untuk mengunci/membuka tombol
        CekStatusLevel();
        
        // 3. Pastikan panel tertutup saat mulai
        if (prologGameplayPanel != null)
            prologGameplayPanel.SetActive(false);
    }

    void CekStatusLevel()
    {
        int maxLevel = PlayerPrefs.GetInt("MaxLevelUnlocked", 1);

        // Jika maxLevel masih 1, maka tombol 2 dan 3 dimatikan
        if (maxLevel < 2 && btnLevel2 != null)
        {
            btnLevel2.interactable = false; // Tidak bisa dipencet
        }

        if (maxLevel < 3 && btnLevel3 != null)
        {
            btnLevel3.interactable = false; // Tidak bisa dipencet
        }
    }

    // --- FUNGSI NAVIGASI UTAMA ---

    public void OnLevel1Clicked()
    {
        if (prologGameplayPanel != null)
            prologGameplayPanel.SetActive(true);
    }

    // --- FUNGSI TOMBOL GAMEPLAY ---
    // Pastikan nama scene di bawah ini SAMA PERSIS dengan di Build Settings

    public void StartPuzzleGame()
    {
        SceneManager.LoadScene("ujicobascroll"); // Sudah dikoreksi jadi PuzzleFix
    }

    public void StartSampleSceneGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void StartLemparLembingGame()
    {
        SceneManager.LoadScene("Lempar Lembing");
    }

    public void ClosePrologPanel()
    {
        if (prologGameplayPanel != null)
            prologGameplayPanel.SetActive(false);
    }
}