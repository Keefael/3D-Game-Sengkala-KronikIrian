using UnityEngine;

public class BotController : MonoBehaviour
{
    [Header("Bot Settings")]
    public float moveSpeed = 5f; 
    
    private bool isRaceActive = false;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isRaceActive)
        {
            transform.position += -Vector3.right * moveSpeed * Time.deltaTime;
        }
    }

    public void StartRacing()
    {
        isRaceActive = true;
        if (animator != null) animator.speed = 1f; 
    }

    // FUNGSI BARU: Hentikan gerak DAN animasi bot
    public void StopBot()
    {
        isRaceActive = false;
        
        if (animator != null) 
        {
            animator.speed = 0f; // Freeze animasi di frame terakhir
        }
        
        Debug.Log("🛑 BOT DIHENTIKAN TOTAL!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "FinishLine" && isRaceActive)
        {
            HandleBotWin();
        }
    }

    void HandleBotWin()
    {
        StopBot(); // Hentikan diri sendiri
        
        ClickableCube playerScript = FindObjectOfType<ClickableCube>();
        if (playerScript != null)
        {
            playerScript.TriggerLosePanel();
        }
    }
}