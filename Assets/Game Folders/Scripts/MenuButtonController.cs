using UnityEngine;
using SengkalaDev;

public class MenuButtonController : MonoBehaviour
{
    public void OnPlayButtonClicked()
    {
        Debug.Log("PLAY diklik - State: WorldMap");
        SengkalaDev.GameManager.Instance.ChangeState(GameState.WorldMap);
    }

    public void OnLevelButtonClicked()
    {
        SengkalaDev.GameManager.Instance.ChangeState(GameState.Level);
    }

    public void OnBackToMenuButtonClicked()
    {
        SengkalaDev.GameManager.Instance.ChangeState(GameState.Menu);
    }

    public void OnIrianPediaButtonClicked()
    {
        SengkalaDev.GameManager.Instance.ChangeState(GameState.IrianPedia);
    }

    public void OnCreditsButtonClicked()
    {
        SengkalaDev.GameManager.Instance.ChangeState(GameState.Credits);
    }

    public void OnQuitButtonClicked()
    {
        SengkalaDev.GameManager.Instance.ChangeState(GameState.Quit);
    }
}