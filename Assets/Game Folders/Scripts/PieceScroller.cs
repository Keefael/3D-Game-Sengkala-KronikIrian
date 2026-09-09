using UnityEngine;
using UnityEngine.UI;

public class PieceScroller : MonoBehaviour
{
    public RectTransform contentTransform; // GANTI INI: Drag PiecesContent ke sini
    public float scrollSpeed = 200f; // Pixel per klik

    public void ScrollUp()
    {
        if (contentTransform == null) return;
        
        // Geser KE ATAS (tambah Y)
        Vector3 pos = contentTransform.localPosition;
        pos.y += scrollSpeed;
        
        // Batasi agar tidak tembus atas (PosY max = 0 karena anchor top)
        // Kamu perlu tau batas bawahnya, tapi buat tes dulu biarin bebas
        contentTransform.localPosition = pos;
        
        Debug.Log("UP! PosY: " + pos.y);
    }

    public void ScrollDown()
    {
        if (contentTransform == null) return;
        
        // Geser KE BAWAH (kurang Y)
        Vector3 pos = contentTransform.localPosition;
        pos.y -= scrollSpeed;
        
        contentTransform.localPosition = pos;
        
        Debug.Log("DOWN! PosY: " + pos.y);
    }
}