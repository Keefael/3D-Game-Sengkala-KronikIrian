using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelWinHandler : MonoBehaviour
{
    [Header("PENTING: Isi angka level ini di Inspector!")]
    [Tooltip("1 = PuzzleFix, 2 = SampleScene, 3 = Lempar Lembing")]
    public int currentLevelIndex;

    // Panggil fungsi ini dari tombol "Next Level" atau "Finish" di Panel Win Anda
    public void OnLevelCompleted()
    {
        // 1. Hitung level berikutnya
        int nextLevelToUnlock = currentLevelIndex + 1;

        // 2. Ambil data level tertinggi yang sudah terbuka saat ini
        int currentMaxUnlocked = PlayerPrefs.GetInt("MaxLevelUnlocked", 1);

        // 3. Jika level berikutnya lebih tinggi dari yang sudah ada, update datanya
        if (nextLevelToUnlock > currentMaxUnlocked)
        {
            PlayerPrefs.SetInt("MaxLevelUnlocked", nextLevelToUnlock);
            PlayerPrefs.Save();
            Debug.Log("Level " + nextLevelToUnlock + " berhasil dibuka!");
        }

        // 4. Pindah ke scene berikutnya sesuai alur
        LoadNextScene();
    }

    void LoadNextScene()
    {
        if (currentLevelIndex == 1)
        {
            // Selesai PuzzleFix -> Masuk SampleScene
            SceneManager.LoadScene("SampleScene");
        }
        else if (currentLevelIndex == 2)
        {
            // Selesai SampleScene -> Masuk Lempar Lembing
            SceneManager.LoadScene("Lempar Lembing");
        }
        else if (currentLevelIndex == 3)
        {
            // Selesai Lempar Lembing -> Kembali ke MainMenu
            SceneManager.LoadScene("MainMenu");
        }
    }
}