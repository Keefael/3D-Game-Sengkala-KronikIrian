using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PieceContainerArranger : MonoBehaviour
{
    [Header("Referensi")]
    [Tooltip("Drag objek parent yang menampung semua SLOT puzzle di sini")]
    public Transform slotsParent; 

    [Header("Pengaturan Container")]
    public float spacing = 20f;
    public float padding = 10f;
    
    [Header("Pengaturan Ukuran")]
    [Range(0.1f, 2.0f)]
    public float sizeMultiplier = 1.0f;
    
    [Tooltip("Centang untuk lihat ukuran yang terbaca di Console")]
    public bool showDebugLog = true;

    void Start()
    {
        SetupContainerLayout();
        
        // Tunggu 1 frame agar layout selesai sebelum mengatur piece
        StartCoroutine(DelayedArrange());
    }

    IEnumerator DelayedArrange()
    {
        // Force update semua Canvas agar ukuran slot sudah final
        Canvas.ForceUpdateCanvases();
        
        // Tunggu 1 frame
        yield return null;
        
        // Force update lagi untuk memastikan
        Canvas.ForceUpdateCanvases();
        
        // Sekarang baru atur piece dan scroll
        ArrangePieces();
    }

    void SetupContainerLayout()
    {
        // 1. Hapus GridLayoutGroup jika ada (biang kerok ukuran terkunci)
        GridLayoutGroup gridLayout = GetComponent<GridLayoutGroup>();
        if (gridLayout != null) Destroy(gridLayout);

        // 2. Setup VerticalLayoutGroup
        VerticalLayoutGroup vLayout = GetComponent<VerticalLayoutGroup>();
        if (vLayout == null) vLayout = gameObject.AddComponent<VerticalLayoutGroup>();
        
        vLayout.padding = new RectOffset((int)padding, (int)padding, (int)padding, (int)padding);
        vLayout.spacing = spacing;
        vLayout.childAlignment = TextAnchor.UpperCenter;
        vLayout.childControlWidth = true;
        vLayout.childControlHeight = true;
        vLayout.childForceExpandWidth = false;
        vLayout.childForceExpandHeight = false;

        // 3. Setup ContentSizeFitter
        ContentSizeFitter sizeFitter = GetComponent<ContentSizeFitter>();
        if (sizeFitter == null) sizeFitter = gameObject.AddComponent<ContentSizeFitter>();
        sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        sizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
    }

    void ArrangePieces()
    {
        // Loop semua piece di dalam container
        foreach (Transform pieceTransform in transform)
        {
            PuzzlePiece piece = pieceTransform.GetComponent<PuzzlePiece>();
            if (piece == null) continue;

            // Reset anchor ke tengah agar tidak bergeser
            RectTransform pieceRect = pieceTransform.GetComponent<RectTransform>();
            pieceRect.anchorMin = new Vector2(0.5f, 0.5f);
            pieceRect.anchorMax = new Vector2(0.5f, 0.5f);
            pieceRect.pivot = new Vector2(0.5f, 0.5f);
            pieceRect.anchoredPosition = Vector2.zero;

            // Cari slot yang cocok
            Transform matchingSlot = FindMatchingSlot(piece.pieceId);
            
            if (matchingSlot != null)
            {
                RectTransform slotRect = matchingSlot.GetComponent<RectTransform>();
                
                // Ambil ukuran slot
                float slotWidth = slotRect.sizeDelta.x;
                float slotHeight = slotRect.sizeDelta.y;
                
                // Terapkan multiplier
                float finalWidth = slotWidth * sizeMultiplier;
                float finalHeight = slotHeight * sizeMultiplier;
                
                if (showDebugLog)
                {
                    Debug.Log($"Piece ID {piece.pieceId}: Slot sizeDelta = {slotWidth}x{slotHeight}, Final = {finalWidth}x{finalHeight}");
                }
                
                // Tambah/ambil LayoutElement
                LayoutElement layoutElement = pieceTransform.GetComponent<LayoutElement>();
                if (layoutElement == null)
                {
                    layoutElement = pieceTransform.gameObject.AddComponent<LayoutElement>();
                }
                
                // Set ukuran
                layoutElement.preferredWidth = finalWidth;
                layoutElement.preferredHeight = finalHeight;
                layoutElement.minWidth = finalWidth;
                layoutElement.minHeight = finalHeight;
                layoutElement.flexibleWidth = 0;
                layoutElement.flexibleHeight = 0;
            }
            else
            {
                Debug.LogWarning($"Slot dengan ID {piece.pieceId} tidak ditemukan!");
            }
        }

        // ✅ PERBAIKAN: Paksa Scroll View kembali ke posisi paling atas setelah piece selesai diatur
        ScrollRect scrollRect = GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1f; // 1 = Paling Atas, 0 = Paling Bawah
        }
    }

    Transform FindMatchingSlot(int id)
    {
        if (slotsParent == null) return null;

        foreach (Transform slotTransform in slotsParent)
        {
            PuzzleSlot slot = slotTransform.GetComponent<PuzzleSlot>();
            if (slot != null && slot.slotId == id)
            {
                return slotTransform;
            }
        }
        return null;
    }
}