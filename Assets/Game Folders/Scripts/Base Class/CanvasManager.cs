using SengkalaDev;
using System;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    private Page[] allPages;

    private void Start()
    {
        allPages = GetComponentsInChildren<Page>(true);

        SengkalaDev.GameManager.Instance.OnChangeStated += Instance_OnChangeStated;
    }

    private void OnDestroy()
    {
        SengkalaDev.GameManager.Instance.OnChangeStated -= Instance_OnChangeStated;
    }

    private void Instance_OnChangeStated(SengkalaDev.GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                ChangePage(PageName.Menu);
                break;
            case GameState.IrianPedia:
                ChangePage(PageName.IrianPedia);
                break;
            case GameState.Credits:
                ChangePage(PageName.Credits);
                break;
            case GameState.Quit:
                break;
            case GameState.Level:
                ChangePage(PageName.Level);
                break;
        }
    }

    private void ChangePage(PageName pageName)
    {
        foreach (Page page in allPages) 
        {
            page.gameObject.SetActive(false);
        }

        Page findPage = Array.Find(allPages, x => x.pageName == pageName);
        if (findPage != null) 
        {
            findPage.gameObject.SetActive(true);
        }
    }
}
