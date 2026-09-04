using UnityEngine;
using UnityEngine.SceneManagement;

public class PrologLevelManager : MonoBehaviour
{
    /// <summary>
    /// Tombol Puzzle -> Load scene "Puzzle"
    /// </summary>
    public void StartPuzzle()
    {
        SceneManager.LoadScene("PuzzleFix");
    }

    /// <summary>
    /// Tombol Lempar Lembing -> Load scene "Lempar Lembing"
    /// </summary>
    public void StartLemparLembing()
    {
        SceneManager.LoadScene("Lempar Lembing");
    }

    /// <summary>
    /// Tombol SampleScene -> Load scene "SampleScene"
    /// </summary>
    public void StartSampleScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
    
    // Opsional: Jika ingin ada tombol kembali ke Select Level
    public void BackToSelectLevel()
    {
        SceneManager.LoadScene("Select Level");
    }
}