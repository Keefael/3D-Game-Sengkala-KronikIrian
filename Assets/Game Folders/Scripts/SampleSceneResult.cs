using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SampleSceneResult : MonoBehaviour
{
    [Header("UI Elements")]
    public Text resultText;         // Text untuk menampilkan Menang/Kalah
    public Button btnNextLevel;     // Tombol lanjut ke level berikutnya
    public Button btnRestart;       // Tombol ulang level

    [Header("Level Info")]
    public int currentLevelIndex = 2; // SampleScene adalah Level 2

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowWinPanel()
    {
        gameObject.SetActive(true);
        if (resultText != null) resultText.text = "SELAMAT! ANDA MENANG!";
        
        if (btnNextLevel != null) btnNextLevel.gameObject.SetActive(true);
        if (btnRestart != null) btnRestart.gameObject.SetActive(false);
    }

    public void ShowLosePanel()
    {
        gameObject.SetActive(true);
        if (resultText != null) resultText.text = "YAHH... KAMU KALAH!";
        
        if (btnNextLevel != null) btnNextLevel.gameObject.SetActive(false);
        if (btnRestart != null) btnRestart.gameObject.SetActive(true);
    }

    public void OnNextLevelClicked()
    {
        int nextLevel = currentLevelIndex + 1;
        int currentMax = PlayerPrefs.GetInt("MaxLevelUnlocked", 1);
        
        if (nextLevel > currentMax)
        {
            PlayerPrefs.SetInt("MaxLevelUnlocked", nextLevel);
            PlayerPrefs.Save();
        }

        SceneManager.LoadScene("Lempar Lembing");
    }

    public void OnRestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}