using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PuzzleSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("Identitas Slot")]
    public int slotId;

    public bool isFilled = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Jika slot sudah terisi, abaikan klik
        if (isFilled) return;
        
        // Gunakan method getter untuk mendapatkan piece yang dipilih
        PuzzlePiece selected = PuzzleManager.Instance.GetSelectedPiece();
        
        // Jika tidak ada piece yang dipilih, abaikan
        if (selected == null) return;

        // Cek apakah piece yang dipilih cocok dengan slot ini
        if (selected.pieceId == this.slotId)
        {
            PlacePiece(selected);
        }
        else
        {
            // Jika salah, batalkan pilihan (bisa ditambah efek suara "err")
            PuzzleManager.Instance.Deselect();
        }
    }

    void PlacePiece(PuzzlePiece piece)
    {
        isFilled = true;
        PuzzleManager.Instance.Deselect();

        // Jalankan animasi terbang
        StartCoroutine(piece.FlyToSlot(this.transform, () => 
        {
            // Setelah animasi selesai, snap piece ke slot
            piece.SnapToSlot(this.transform);
            
            // ✅ PENTING: Beritahu Manager bahwa piece sudah berhasil snap
            PuzzleManager.Instance.OnPieceSnapped();
            
            // Log untuk debugging
            Debug.Log("Piece " + piece.pieceId + " berhasil terpasang di Slot " + slotId);
        }));
    }
}