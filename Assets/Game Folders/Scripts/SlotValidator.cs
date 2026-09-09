using UnityEngine;

public class SlotValidator : MonoBehaviour
{
    public RectTransform[] allSlots;
    public float minSafeDistance = 10f; // Jarak minimal antar slot

    void Start()
    {
        ValidateSlots();
    }

    void ValidateSlots()
    {
        for (int i = 0; i < allSlots.Length; i++)
        {
            for (int j = i + 1; j < allSlots.Length; j++)
            {
                float dist = Vector2.Distance(
                    allSlots[i].anchoredPosition, 
                    allSlots[j].anchoredPosition
                );
                
                if (dist < minSafeDistance)
                {
                    Debug.LogWarning(
                        $"⚠️ OVERFLOW DETECTED: {allSlots[i].name} dan {allSlots[j].name} " +
                        $"terlalu dekat! Jarak: {dist:F1}px"
                    );
                    
                    // Highlight slot yang bermasalah di Scene View
                    allSlots[i].GetComponent<UnityEngine.UI.Image>().color = Color.red;
                    allSlots[j].GetComponent<UnityEngine.UI.Image>().color = Color.red;
                }
            }
        }
    }
}