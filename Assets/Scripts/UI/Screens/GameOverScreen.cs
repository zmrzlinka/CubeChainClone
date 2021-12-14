using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : ScreenBase
{

    public void ReturnToMenu()
    {
        App.gameManager.EndGame();
        Hide();
    }

    public void RetryGame()
    {
        App.gameManager.RestartGame();
        Hide();
    }
}
