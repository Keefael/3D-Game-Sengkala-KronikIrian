using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class PuzzlePiece : MonoBehaviour, IPointerClickHandler
{
    [Header("Identitas Piece")]
    public int pieceId;

    private bool isSnapped = false;
    private Image imageComponent;
    private RectTransform rectTransform;
    private Color originalColor;

    void Awake()
    {
        imageComponent = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        originalColor = imageComponent.color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isSnapped) return;
        
        PuzzleManager.Instance.SelectPiece(this);
    }

    public void Select()
    {
        imageComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
        transform.localScale = Vector3.one * 1.1f;
    }

    public void Deselect()
    {
        imageComponent.color = originalColor;
        transform.localScale = Vector3.one;
    }

    public IEnumerator FlyToSlot(Transform targetSlot, System.Action onComplete)
    {
        // Pindah parent ke Canvas root
        RectTransform canvasRect = GetComponentInParent<Canvas>().transform as RectTransform;
        transform.SetParent(canvasRect, false);

        Vector3 startPos = transform.position;
        Vector3 endPos = targetSlot.position;
        
        // ✅ Ambil ukuran slot target
        RectTransform targetRectTransform = targetSlot.GetComponent<RectTransform>();
        Vector2 startSize = rectTransform.sizeDelta;
        Vector2 targetSize = targetRectTransform.sizeDelta;
        
        float duration = 0.3f;
        float elapsed = 0f;

        // Animasi terbang + resize
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float smoothT = Mathf.SmoothStep(0, 1, t);
            
            // Animasi posisi
            transform.position = Vector3.Lerp(startPos, endPos, smoothT);
            
            // ✅ Animasi ukuran (resize)
            rectTransform.sizeDelta = Vector2.Lerp(startSize, targetSize, smoothT);
            
            yield return null;
        }

        // Pastikan posisi dan ukuran pas di akhir
        transform.position = endPos;
        rectTransform.sizeDelta = targetSize;
        
        if (onComplete != null) onComplete();
    }

    public void SnapToSlot(Transform slotParent)
    {
        isSnapped = true;
        
        // ✅ Pastikan ukuran match dengan slot parent
        RectTransform parentRect = slotParent.GetComponent<RectTransform>();
        rectTransform.sizeDelta = parentRect.sizeDelta;
        
        // Parent-kan ke slot
        transform.SetParent(slotParent);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        imageComponent.color = originalColor;
    }
}