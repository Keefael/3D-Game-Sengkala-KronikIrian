using UnityEngine;

public class PuzzleSlot : MonoBehaviour
{
    [Header("Identitas Slot")]
    public int slotId;

    [HideInInspector] 
    public bool isFilled = false;

    // Script ini tidak memerlukan method Update atau PointerClick lagi.
    // Logika deteksi dan snapping sepenuhnya ditangani oleh PuzzlePiece saat OnEndDrag.
}