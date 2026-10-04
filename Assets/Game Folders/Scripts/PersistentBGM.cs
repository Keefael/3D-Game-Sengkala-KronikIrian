using UnityEngine;

public class PersistentBGM : MonoBehaviour
{
    void Awake()
    {
        // Perintah ajaib ini yang membuat GameObject tidak ikut hancur saat pindah scene
        DontDestroyOnLoad(this.gameObject);
    }
}