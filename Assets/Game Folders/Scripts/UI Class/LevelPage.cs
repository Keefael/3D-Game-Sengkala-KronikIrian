using UnityEngine;
using UnityEngine.UI;

public class LevelPage : Page
{
    [SerializeField] private Button homeButton;

    [SerializeField] private Button Level1Button;
    [SerializeField] private Button Level2Button;
    [SerializeField] private Button Level3Button;
    [SerializeField] private Button Level4Button;

    protected override void Start()
    {
        base.Start();

        homeButton.onClick.AddListener(()=> SengkalaDev.GameManager.Instance.ChangeState(SengkalaDev.GameState.Menu));
    }
}
