using UnityEngine;
using UnityEngine.UI;

public class StaminaUIManager : MonoBehaviour
{
    public ClickableCube cube;
    
    [Header("UI References")]
    public Slider staminaSlider; // Slider Stamina (Biru/Hijau)
    public Slider speedSlider;   // Slider Speed Baru (Merah/Oranye)

    void Update()
    {
        if (cube != null)
        {
            // Update Bar Stamina
            if (staminaSlider != null)
            {
                staminaSlider.value = cube.GetStaminaPercent();
            }

            // Update Bar Speed (Momentum)
            if (speedSlider != null)
            {
                speedSlider.value = cube.GetSpeedPercent();
            }
        }
    }
}