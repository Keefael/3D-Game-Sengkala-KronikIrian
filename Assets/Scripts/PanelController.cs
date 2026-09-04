using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelController : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject losePanel;
    
    // Referensi ke script gameplay (opsional, untuk disable/enable)
    public MonoBehaviour gameplayScript; 
    
    void Start()
    {
        // Sembunyikan semua panel di awal
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    public void ShowWinPanel()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            
            // Unlock cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            // Pause game
            Time.timeScale = 0f;
            
            // Disable gameplay script jika ada
            if (gameplayScript != null)
                gameplayScript.enabled = false;
        }
    }

    public void ShowLosePanel()
    {
        if (losePanel != null)
        {
            losePanel.SetActive(true);
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            Time.timeScale = 0f;
            
            if (gameplayScript != null)
                gameplayScript.enabled = false;
        }
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Ganti dengan nama scene menu kamu
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        
        // Lock cursor kembali
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Enable gameplay script kembali
        if (gameplayScript != null)
            gameplayScript.enabled = true;
        
        // Sembunyikan panel
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
        
        // Reload scene saat ini
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}