using UnityEngine;
using UnityEngine.SceneManagement; // Wajib untuk ganti scene

public class MainMenuManager : MonoBehaviour
{
    [Header("Panel UI")]
    [Tooltip("Drag panel Irianpedia ke sini dari Hierarchy")]
    public GameObject irianpediaPanel;
    
    [Tooltip("Drag panel Credits ke sini dari Hierarchy")]
    public GameObject creditsPanel;

    // --- FUNGSI TOMBOL UTAMA ---

    /// <summary>
    /// Tombol START: Pindah ke scene Select Level
    /// Pastikan nama scene "SelectLevel" sudah ada di Build Settings
    /// </summary>
    public void OnStartClicked()
    {
        SceneManager.LoadScene("SelectLevel"); 
    }

    /// <summary>
    /// Tombol IRIANPEDIA: Menampilkan panel Irianpedia
    /// </summary>
    public void OnIrianpediaClicked()
    {
        if (irianpediaPanel != null)
            irianpediaPanel.SetActive(true);
    }

    /// <summary>
    /// Tombol CREDITS: Menampilkan panel Credits
    /// </summary>
    public void OnCreditsClicked()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(true);
    }

    /// <summary>
    /// Tombol QUIT: Keluar dari aplikasi
    /// Catatan: Quit tidak akan bekerja saat Play Mode di Editor, hanya saat Build
    /// </summary>
    public void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // --- FUNGSI TOMBOL CLOSE PANEL ---
    // Hubungkan fungsi ini ke tombol "X" atau "Close" pada masing-masing panel

    public void CloseIrianpediaPanel()
    {
        if (irianpediaPanel != null)
            irianpediaPanel.SetActive(false);
    }

    public void CloseCreditsPanel()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(false);
    }
}