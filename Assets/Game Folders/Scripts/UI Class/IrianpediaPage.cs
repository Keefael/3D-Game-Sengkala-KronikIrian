using UnityEngine;
using UnityEngine.UI;

public class IrianpediaPage : Page
{
    [SerializeField] private Button homeButton;

    protected override void Start()
    {
        base.Start();

        homeButton.onClick.AddListener(() => SengkalaDev.GameManager.Instance.ChangeState(SengkalaDev.GameState.Menu));
    }
}
