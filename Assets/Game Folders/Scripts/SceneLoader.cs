using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Fungsi ini dipanggil oleh tombol Level 1, 2, 3
    public void LoadLevel(string namaScene)
    {
        SceneManager.LoadScene(namaScene);
    }
}