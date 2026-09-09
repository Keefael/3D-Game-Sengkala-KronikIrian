using SengkalaDev;
using UnityEngine;
using UnityEngine.UI;

public class MenuPage : Page
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button irianPediaButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    protected override void Start()
    {
        base.Start();

        startButton.onClick.AddListener(() => SengkalaDev.GameManager.Instance.ChangeState(GameState.Level));
        irianPediaButton.onClick.AddListener(() => SengkalaDev.GameManager.Instance.ChangeState(GameState.IrianPedia));
        creditsButton.onClick.AddListener(() => SengkalaDev.GameManager.Instance.ChangeState(GameState.Credits));
        quitButton.onClick.AddListener(()=> Application.Quit());
    }
}
