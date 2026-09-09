using UnityEngine;

public class CursorFix : MonoBehaviour
{
    [Header("Panel UI")]
    public GameObject winPanel;  // Drag panel Win ke sini
    public GameObject losePanel; // Drag panel Lose ke sini

    void Update()
    {
        // Cek apakah salah satu panel sedang aktif
        bool isPanelActive = false;

        if (winPanel != null && winPanel.activeSelf)
        {
            isPanelActive = true;
        }

        if (losePanel != null && losePanel.activeSelf)
        {
            isPanelActive = true;
        }

        // Jika ada panel yang aktif, paksa cursor muncul
        if (isPanelActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}