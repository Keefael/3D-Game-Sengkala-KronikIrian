using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class IntroTextManager : MonoBehaviour
{
    [Header("Referensi UI")]
    [SerializeField] private TextMeshProUGUI introText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Pengaturan Waktu (Detik)")]
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float stayDuration = 2.0f;
    [SerializeField] private float fadeOutDuration = 0.5f;

    [Header("Opsional: Countdown (isi hanya untuk Level 2)")]
    [SerializeField] private GameObject countdownObject; // Gunakan GameObject, bukan script spesifik

    void Start()
    {
        SetTeksBerdasarkanScene();
        
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        canvasGroup.alpha = 0;
        StartCoroutine(AnimasiIntro());
    }

    void SetTeksBerdasarkanScene()
    {
        string namaScene = SceneManager.GetActiveScene().name;
        string namaPermainan = "";

        switch (namaScene)
        {
            case "ujicobascroll": 
                namaPermainan = "Puzzle Kaimana";
                break;
            case "SampleScene": 
                namaPermainan = "Bo Dawosara Raruko";
                break;
            case "Lempar Lembing": 
                namaPermainan = "Aisoki";
                break;
            default:
                namaPermainan = "Permainan Tradisional";
                break;
        }

        if (introText != null) 
            introText.text = namaPermainan;
    }

    IEnumerator AnimasiIntro()
    {
        // FASE 1: FADE IN
        float elapsed = 0;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / fadeInDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;

        // FASE 2: BERTAHAN
        yield return new WaitForSeconds(stayDuration);

        // FASE 3: FADE OUT
        elapsed = 0;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / fadeOutDuration);
            yield return null;
        }
        canvasGroup.alpha = 0;

        // FASE 4: SELESAI
        gameObject.SetActive(false);

        // PANGGIL COUNTDOWN JIKA ADA
        MulaiCountdownJikaAda();
    }

    void MulaiCountdownJikaAda()
    {
        // Cara 1: Jika kamu assign manual di Inspector
        if (countdownObject != null)
        {
            // Coba cari script countdown di objek tersebut
            var countdownScript = countdownObject.GetComponent<MonoBehaviour>();
            if (countdownScript != null)
            {
                countdownScript.Invoke("MulaiHitungMundur", 0.1f);
            }
        }
        else
        {
            // Cara 2: Cari otomatis di scene (untuk Level 2)
            var countdownScripts = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            foreach (var script in countdownScripts)
            {
                if (script.GetType().Name == "CountdownTimer")
                {
                    script.Invoke("MulaiHitungMundur", 0.1f);
                    break;
                }
            }
        }
    }
}