using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; 

public class ClickableCube : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float drainPerClick = 5f;
    public float regenRate = 8f; 
    public float regenDelay = 1.0f; 

    [Header("Movement Settings")]
    public float thrustForce = 3f; 
    public float waterDrag = 2f; 

    [Header("Water Splash Settings")]
    public ParticleSystem waterSplash;
    public float minSpeedForSplash = 0.5f;

    [Header("UI & Animation Settings")]
    public float maxSpeedDisplay = 10f; 
    
    private float currentSpeedPercent;  
    private Vector3 velocity; 
    private float regenTimer = 0f;
    private float rowAnimationTimer = 0f; 

    // --- VARIABEL COUNTDOWN & ANIMASI POP-UP ---
    [Header("Countdown Settings")]
    public TextMeshProUGUI countdownText; 
    public float countdownDuration = 3f;  
    private bool isRaceStarted = false;   
    private bool isRaceFinished = false; 
    private float countdownTimer = 0f;
    
    private RectTransform countdownRect;
    private float popUpAnimTimer = 0f;
    private const float POP_UP_DURATION = 0.4f; 
    private string lastDisplayedNumber = "";    

    // --- VARIABEL FINISH LINE & UI ---
    [Header("Finish Line & UI Settings")]
    public GameObject winPanel; 
    public GameObject winButton;    // Tombol khusus saat Menang
    public GameObject retryButton;  // Tombol khusus saat Kalah (Retry)
    
    [Header("Bot Settings")]
    public GameObject botBoat; 
    private BotController botController; 

    private AudioSource audioSrc; 
    private Animator animator; 

    void Start()
    {
        currentStamina = maxStamina;
        audioSrc = GetComponent<AudioSource>(); 
        animator = GetComponent<Animator>(); 
        
        if (countdownText != null)
            countdownRect = countdownText.GetComponent<RectTransform>();

        // OTOMATIS MENCARI KOMPONEN BOT CONTROLLER
        if (botBoat != null)
        {
            botController = botBoat.GetComponent<BotController>();
        }

        // --- PASTIKAN PANEL DAN TOMBOL TERSEMBUNYI DI AWAL GAME ---
        if (winPanel != null) winPanel.SetActive(false);
        if (winButton != null) winButton.SetActive(false);
        if (retryButton != null) retryButton.SetActive(false);
        // ----------------------------------------------------------

        StartCountdown();
    }

    void Update()
    {
        // 1. LOGIKA COUNTDOWN
        if (!isRaceStarted && !isRaceFinished)
        {
            countdownTimer -= Time.deltaTime;
            
            string targetText = "";
            if (countdownTimer > 2f) targetText = "3";
            else if (countdownTimer > 1f) targetText = "2";
            else if (countdownTimer > 0f) targetText = "1";
            else targetText = "GO!";

            if (targetText != lastDisplayedNumber)
            {
                lastDisplayedNumber = targetText;
                if (countdownText != null) countdownText.text = targetText;
                popUpAnimTimer = POP_UP_DURATION; 
            }

            if (popUpAnimTimer > 0 && countdownRect != null)
            {
                popUpAnimTimer -= Time.deltaTime;
                float t = Mathf.Clamp01(popUpAnimTimer / POP_UP_DURATION);
                float currentScale = Mathf.Lerp(1.5f, 1f, 1f - t);
                countdownRect.localScale = new Vector3(currentScale, currentScale, 1f);
            }

            if (countdownTimer <= 0f)
            {
                isRaceStarted = true;
                
                if (botBoat != null && !botBoat.activeSelf)
                {
                    botBoat.SetActive(true);
                    if (botController != null) 
                    {
                        botController.StartRacing();
                    }
                }

                Invoke(nameof(HideCountdownText), 0.5f);
            }
            
            return; 
        }

        // 2. HARD STOP: JIKA RACE SUDAH SELESAI
        if (isRaceFinished)
        {
            velocity = Vector3.zero;
            rowAnimationTimer = 0f;
            return; 
        }

        // 3. DETEKSI INPUT (KLIK MOUSE PADA OBJEK ATAU TOMBOL SPACE)
        bool isInputDetected = false;

        // Opsi A: Klik Kiri Mouse (Harus mengenai objek ini)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    isInputDetected = true;
                }
            }
        }

        // Opsi B: Tombol Space (Langsung aktif tanpa perlu raycast)
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            isInputDetected = true;
        }

        // Jalankan aksi jika salah satu input terdeteksi
        if (isInputDetected)
        {
            HandleClick();
        }

        // 4. GERAKAN SMOOTH
        velocity *= (1f - waterDrag * Time.deltaTime);
        transform.position += velocity * Time.deltaTime;

        // 5. PARTIKEL AIR
        float currentSpeed = velocity.magnitude;
        currentSpeedPercent = Mathf.Clamp01(currentSpeed / maxSpeedDisplay);
        if (waterSplash != null)
        {
            if (currentSpeed >= minSpeedForSplash)
            {
                if (!waterSplash.isPlaying) waterSplash.Play();
                var emission = waterSplash.emission;
                emission.rateOverTime = Mathf.Lerp(10, 60, currentSpeedPercent);
            }
            else if (waterSplash.isPlaying) waterSplash.Stop();
        }

        // 6. REGEN STAMINA
        if (currentStamina < maxStamina)
        {
            if (currentStamina <= 0)
            {
                regenTimer += Time.deltaTime;
                if (regenTimer >= regenDelay) currentStamina = Mathf.Clamp(currentStamina + regenRate * Time.deltaTime, 0, maxStamina);
            }
            else
            {
                regenTimer = 0f;
                currentStamina = Mathf.Clamp(currentStamina + regenRate * Time.deltaTime, 0, maxStamina);
            }
        }

        // 7. TIMER ANIMASI DAYUNG
        if (rowAnimationTimer > 0)
        {
            rowAnimationTimer -= Time.deltaTime;
            if (animator != null) animator.SetFloat("RowTimer", rowAnimationTimer);
        }
        else if (animator != null && animator.GetFloat("RowTimer") > 0)
        {
            animator.SetFloat("RowTimer", 0f);
        }
    }

    void HandleClick()
    {
        if (!isRaceStarted || isRaceFinished) return; 

        if (currentStamina <= 0) return; 
        
        currentStamina = Mathf.Clamp(currentStamina - drainPerClick, 0, maxStamina);
        velocity += -transform.right * thrustForce; 
        
        rowAnimationTimer = 1.0f; 
        if (audioSrc != null) audioSrc.Play();
    }

    void StartCountdown()
    {
        isRaceStarted = false;
        isRaceFinished = false;
        countdownTimer = countdownDuration;
        lastDisplayedNumber = ""; 
        
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = "3";
            popUpAnimTimer = POP_UP_DURATION; 
        }
    }

    void HideCountdownText()
    {
        if (countdownText != null) countdownText.gameObject.SetActive(false);
    }

    // --- DETEKSI GARIS FINISH PLAYER ---
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "FinishLine" && isRaceStarted && !isRaceFinished) 
        {
            HandleWin();
        }
    }

    // --- FUNGSI PLAYER MENANG ---
    void HandleWin()
    {
        EndRace(); 
        
        if (winPanel != null)
        {
            TextMeshProUGUI panelText = winPanel.GetComponentInChildren<TextMeshProUGUI>();
            if (panelText != null) panelText.text = "SELAMAT! ANDA MENANG!";
            
            winPanel.SetActive(true);
            
            // --- ATUR VISIBILITAS TOMBOL SAAT MENANG ---
            if (winButton != null) winButton.SetActive(true);
            if (retryButton != null) retryButton.SetActive(false);
            // --------------------------------------------
        }
        
        Debug.Log("🏆 FINISH! Player Wins!");
    }

    // --- FUNGSI DIPANGGIL BOT SAAT BOT MENANG ---
    public void TriggerLosePanel()
    {
        EndRace(); 
        
        if (winPanel != null)
        {
            TextMeshProUGUI panelText = winPanel.GetComponentInChildren<TextMeshProUGUI>();
            if (panelText != null) panelText.text = "YAHH... KAMU KALAH!";
            
            winPanel.SetActive(true);
            
            // --- ATUR VISIBILITAS TOMBOL SAAT KALAH ---
            if (winButton != null) winButton.SetActive(false);
            if (retryButton != null) retryButton.SetActive(true);
            // -------------------------------------------
        }
        
        Debug.Log("💀 FINISH! Bot Wins!");
    }

    // --- FUNGSI UNIVERSAL UNTUK MENGAKHIRI RACE ---
    void EndRace()
    {
        isRaceFinished = true; 
        velocity = Vector3.zero; 
        rowAnimationTimer = 0f;
        
        if (animator != null) animator.SetFloat("RowTimer", 0f);
        if (waterSplash != null && waterSplash.isPlaying) waterSplash.Stop();

        // SURUH BOT BERHENTI TOTAL (GERAK + ANIMASI)
        if (botController != null)
        {
            botController.StopBot();
        }
    }

    // --- FUNGSI RESTART ---
    public void RestartRace()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    public float GetStaminaPercent() => currentStamina / maxStamina;
    public float GetSpeedPercent() => currentSpeedPercent;
}