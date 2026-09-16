using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroImageManager : MonoBehaviour
{
    [Header("Referensi UI")]
    [SerializeField] private Image introImage;       
    [SerializeField] private CanvasGroup canvasGroup; 

    [Header("Sprite Berdasarkan Scene (Assign di Inspector)")]
    [Tooltip("Masukkan gambar logo/judul untuk Puzzle Kaimana")]
    [SerializeField] private Sprite spritePuzzle;     
    [Tooltip("Masukkan gambar logo/judul untuk Bo Dawosara Raruko")]
    [SerializeField] private Sprite spriteBoDawosara; 
    [Tooltip("Masukkan gambar logo/judul untuk Aisoki")]
    [SerializeField] private Sprite spriteAisoki;     
    [Tooltip("Gambar default jika scene tidak dikenali")]
    [SerializeField] private Sprite spriteDefault;    

    [Header("Pengaturan Waktu (Detik)")]
    [SerializeField] private float fadeInDuration = 0.8f;
    [SerializeField] private float stayDuration = 2.5f;
    [SerializeField] private float fadeOutDuration = 0.8f;

    [Header("Efek Visual (Pop-up)")]
    [Tooltip("Kurve animasi agar gerakan membesar/mengecil terasa halus")]
    [SerializeField] private AnimationCurve popUpCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [Tooltip("Seberapa besar gambar memantul saat muncul (1.0 = normal, 1.15 = 15% lebih besar)")]
    [SerializeField] private float popScaleAmount = 1.15f;

    [Header("Audio (Opsional tapi Recommended)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip appearSound;   
    [SerializeField] private AudioClip disappearSound;

    [Header("Opsional: Countdown")]
    [SerializeField] private GameObject countdownObject;

    private Vector3 originalScale;

    void Start()
    {
        // Auto-setup komponen jika lupa dipasang di Inspector
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        
        // 1. SET SPRITE BERDASARKAN SCENE
        SetSpriteBerdasarkanScene();

        // 2. SETUP AWAL ANIMASI
        originalScale = transform.localScale;
        canvasGroup.alpha = 0f;
        transform.localScale = originalScale * 0.85f; // Mulai dari 85% ukuran asli
        
        // Mulai animasi
        StartCoroutine(AnimasiIntro());
    }

    void SetSpriteBerdasarkanScene()
    {
        string namaScene = SceneManager.GetActiveScene().name;

        if (introImage != null)
        {
            switch (namaScene)
            {
                case "ujicobascroll": 
                    introImage.sprite = spritePuzzle != null ? spritePuzzle : spriteDefault;
                    break;
                case "SampleScene": 
                    introImage.sprite = spriteBoDawosara != null ? spriteBoDawosara : spriteDefault;
                    break;
                case "Lempar Lembing": 
                    introImage.sprite = spriteAisoki != null ? spriteAisoki : spriteDefault;
                    break;
                default:
                    introImage.sprite = spriteDefault;
                    break;
            }
        }
    }

    IEnumerator AnimasiIntro()
    {
        // --- FASE 1: FADE IN & POP-UP ---
        if (appearSound != null) audioSource.PlayOneShot(appearSound);
        
        float elapsed = 0;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeInDuration;
            float curveValue = popUpCurve.Evaluate(t);
            
            // 1. Atur Transparansi
            canvasGroup.alpha = curveValue;
            
            // 2. Atur Skala (Efek memantul / pop-up)
            float currentScaleMultiplier;
            if (t < 0.5f) 
            {
                // Paruh pertama: membesar melebihi ukuran normal (overshoot)
                currentScaleMultiplier = Mathf.Lerp(0.85f, popScaleAmount, t * 2f);
            }
            else 
            {
                // Paruh kedua: kembali ke ukuran normal (1.0)
                currentScaleMultiplier = Mathf.Lerp(popScaleAmount, 1.0f, (t - 0.5f) * 2f);
            }
            
            transform.localScale = originalScale * currentScaleMultiplier;
            yield return null;
        }
        
        // Pastikan nilai akhir presisi
        canvasGroup.alpha = 1f;
        transform.localScale = originalScale;

        // --- FASE 2: BERTAHAN (DISPLAY) ---
        yield return new WaitForSeconds(stayDuration);

        // --- FASE 3: FADE OUT ---
        if (disappearSound != null) audioSource.PlayOneShot(disappearSound);
        
        elapsed = 0;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeOutDuration;
            
            // Fade out (kurva dibalik)
            canvasGroup.alpha = 1f - popUpCurve.Evaluate(t);
            
            // Sedikit mengecil saat hilang untuk efek dramatis yang halus
            transform.localScale = originalScale * Mathf.Lerp(1.0f, 0.95f, t);

            yield return null;
        }
        
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false); // Matikan objek agar tidak mengganggu gameplay

        // --- FASE 4: TRIGGER COUNTDOWN ---
        MulaiCountdownJikaAda();
    }

    void MulaiCountdownJikaAda()
    {
        if (countdownObject != null)
        {
            var countdownScript = countdownObject.GetComponent<MonoBehaviour>();
            if (countdownScript != null) countdownScript.Invoke("MulaiHitungMundur", 0.1f);
        }
        else
        {
            // Fallback: cari otomatis di scene
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