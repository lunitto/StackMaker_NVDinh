using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject mainmenuUI;
    public GameObject finishUI;

    public void OpenMainMenuUI()
    {
        mainmenuUI.SetActive(true);
        finishUI.SetActive(false);
    }

    public void OpenFinishUI()
    {
        mainmenuUI.SetActive(false);
        finishUI.SetActive(true);
    }

    public void PlayBtn()
    {
        mainmenuUI.SetActive(false);
        LevelManager.Instance.OnStart();
        GameManager.Instance.ChangeState(GameState.GamePlay);
    }

    public void RePlayBtn()
    {
        OpenMainMenuUI();
        LevelManager.Instance.LoadLevel();
        GameManager.Instance.ChangeState(GameState.MainMenu);
    }

    public void NextLevelBtn()
    {
        OpenMainMenuUI();
        LevelManager.Instance.NextLevel();
        GameManager.Instance.ChangeState(GameState.MainMenu);
    }
}
