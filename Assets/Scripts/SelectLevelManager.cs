using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevelManager : MonoBehaviour
{
    /// <summary>
    /// Dipanggil saat tombol "1" di scene Select Level ditekan.
    /// Akan memuat scene "Level Prolog".
    /// </summary>
    public void OnLevel1Clicked()
    {
        // Pastikan nama scene di Build Settings persis "Level Prolog"
        SceneManager.LoadScene("Level Prolog");
    }
}