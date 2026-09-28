using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Identitas Piece")]
    public int pieceId;

    private bool isSnapped = false;
    private Image imageComponent;
    private RectTransform rectTransform;
    private Color originalColor;
    private CanvasGroup canvasGroup;

    private Transform originalParent;
    private Vector2 originalAnchoredPosition;
    private int originalSiblingIndex;
    
    // ✅ BARU: Simpan offset antara posisi piece dan posisi mouse saat mulai drag
    private Vector2 dragOffset;

    void Awake()
    {
        imageComponent = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        originalColor = imageComponent.color;

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isSnapped) return;

        originalParent = transform.parent;
        originalAnchoredPosition = rectTransform.anchoredPosition;
        originalSiblingIndex = transform.GetSiblingIndex();

        Canvas canvas = GetComponentInParent<Canvas>();
        transform.SetParent(canvas.transform, true);

        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling();

        // ✅ BARU: Hitung offset antara posisi mouse dengan posisi piece saat ini
        // Ini yang membuat piece menempel sempurna di jari/mouse
        Vector2 mouseLocalPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)transform.parent,
            eventData.position,
            eventData.pressEventCamera,
            out mouseLocalPos);
        
        dragOffset = rectTransform.anchoredPosition - mouseLocalPos;

        // Efek visual saat diangkat
        transform.localScale = Vector3.one * 1.1f;
        imageComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.9f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isSnapped) return;

        Vector2 mouseLocalPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)transform.parent,
            eventData.position,
            eventData.pressEventCamera,
            out mouseLocalPos))
        {
            // ✅ PERBAIKAN: Tambahkan offset agar piece tidak melompat ke posisi mouse
            rectTransform.anchoredPosition = mouseLocalPos + dragOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isSnapped) return;

        canvasGroup.blocksRaycasts = true;
        transform.localScale = Vector3.one;
        imageComponent.color = originalColor;

        GameObject dropTarget = eventData.pointerCurrentRaycast.gameObject;

        if (dropTarget != null && dropTarget.CompareTag("PuzzleSlot"))
        {
            PuzzleSlot targetSlot = dropTarget.GetComponent<PuzzleSlot>();
            
            if (targetSlot != null && !targetSlot.isFilled && targetSlot.slotId == this.pieceId)
            {
                SnapToSlot(dropTarget.transform);
                return; 
            }
        }

        StartCoroutine(FlyBackToOriginal());
    }

    private IEnumerator FlyBackToOriginal()
    {
        transform.SetParent(originalParent, true);
        Vector3 targetWorldPos = transform.position;
        
        rectTransform.anchoredPosition = originalAnchoredPosition;
        Vector3 startWorldPos = transform.position;

        Canvas canvas = GetComponentInParent<Canvas>();
        transform.SetParent(canvas.transform, true);
        transform.SetAsFirstSibling();

        float duration = 0.25f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);
            transform.position = Vector3.Lerp(startWorldPos, targetWorldPos, t);
            yield return null;
        }

        transform.SetParent(originalParent, false);
        rectTransform.anchoredPosition = originalAnchoredPosition;
        transform.SetSiblingIndex(originalSiblingIndex);
        
        canvasGroup.blocksRaycasts = true;
        imageComponent.color = originalColor;
    }

    public void SnapToSlot(Transform slotParent)
    {
        isSnapped = true;
        
        RectTransform parentRect = slotParent.GetComponent<RectTransform>();
        if (parentRect != null)
        {
            rectTransform.sizeDelta = parentRect.sizeDelta;
        }
        
        transform.SetParent(slotParent, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        imageComponent.color = originalColor;

        slotParent.GetComponent<PuzzleSlot>().isFilled = true;

        if (PuzzleManager.Instance != null)
        {
            PuzzleManager.Instance.OnPieceSnapped();
        }
    }
}