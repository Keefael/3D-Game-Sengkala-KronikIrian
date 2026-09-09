using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class DragDropPiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Referensi Slot Target (Utama)")]
    public RectTransform targetSlot; 

    [Header("Daftar Semua Slot (Untuk Smart Snap)")]
    public List<RectTransform> allAvailableSlots = new List<RectTransform>();

    [Header("Setting Snap & Animasi")]
    public float snapThreshold = 50f; 
    public float popScaleAmount = 1.25f; 
    public float popDuration = 0.2f;     

    // ✅ PUBLIC agar bisa dibaca oleh GameManager
    public bool isSnapped = false; 

    private RectTransform rectTransform;
    private Canvas canvas;
    private Transform originalParent;
    private Vector2 originalPosition;
    
    // Cache variabel untuk menghindari alokasi memori berulang di loop
    private Vector2 tempLocalPos; 

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalParent = transform.parent; 
        originalPosition = rectTransform.anchoredPosition;

        // ✅ SAFETY RESET: Paksa status false setiap kali scene dimuat
        // Ini mencegah bug "langsung menang" karena data tersimpan di Inspector
        isSnapped = false; 

        // Fallback safety: Jika array kosong, gunakan targetSlot manual
        if (allAvailableSlots.Count == 0 && targetSlot != null)
        {
            allAvailableSlots.Add(targetSlot);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isSnapped) return;
        
        // Gunakan 'false' pada parameter kedua agar world position tetap terjaga
        // Mencegah piece melompat tiba-tiba saat dipindah ke root Canvas
        rectTransform.SetParent(canvas.transform, false);
        rectTransform.SetAsLastSibling(); 
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isSnapped) return;
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isSnapped) return;

        RectTransform bestSlot = null;
        float closestDistance = float.MaxValue;

        // --- LOGIKA SMART SNAP ---
        foreach (var slot in allAvailableSlots)
        {
            // Konversi posisi mouse ke koordinat lokal parent slot
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                slot.parent as RectTransform, 
                eventData.position, 
                canvas.worldCamera, 
                out tempLocalPos
            );

            float dist = Vector2.Distance(tempLocalPos, slot.anchoredPosition);
            
            // Cari slot terdekat yang masih dalam jangkauan threshold
            if (dist < closestDistance && dist <= snapThreshold)
            {
                closestDistance = dist;
                bestSlot = slot;
               
                // Early exit: Kalau udah pas banget (< 5px), stop looping biar ringan
                if (dist < 5f) break; 
            }
        }

        if (bestSlot != null)
        {
            targetSlot = bestSlot;
            SnapToTarget();
        }
        else
        {
            ReturnToOriginalPlace();
        }
    }

    void SnapToTarget()
    {
        
        // Parent-kan ke board dengan menjaga world position
        rectTransform.SetParent(targetSlot.parent, false);
        rectTransform.anchoredPosition = targetSlot.anchoredPosition;
        
        isSnapped = true;
        StartCoroutine(AnimateSnap());
    }

    void ReturnToOriginalPlace()
    {
        rectTransform.SetParent(originalParent, false);
        rectTransform.anchoredPosition = originalPosition;
        rectTransform.SetAsLastSibling();
    }

    IEnumerator AnimateSnap()
    {
        Vector3 originalScale = Vector3.one;
        Vector3 popScale = new Vector3(popScaleAmount, popScaleAmount, 1f);
        float halfDuration = popDuration / 2f;
        float elapsed = 0f;
        
        // Fase Membesar
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / halfDuration);
            rectTransform.localScale = Vector3.Lerp(originalScale, popScale, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }
        
        // Fase Mengecil Kembali
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / halfDuration);
            rectTransform.localScale = Vector3.Lerp(popScale, originalScale, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }
        
        // Pastikan skala kembali presisi 1.0
        rectTransform.localScale = originalScale;
    }
}