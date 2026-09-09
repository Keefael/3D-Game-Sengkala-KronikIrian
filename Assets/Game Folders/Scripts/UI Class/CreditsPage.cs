using UnityEngine;
using UnityEngine.UI;

public class CreditsPage : Page
{
    [SerializeField] private Button homeButton;

    protected override void Start()
    {
        base.Start();

        homeButton.onClick.AddListener(() => SengkalaDev.GameManager.Instance.ChangeState(SengkalaDev.GameState.Menu));
    }
}
