using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuScreen : ScreenBase
{

    public TextMeshProUGUI highScoreText;

    public override void Show()
    {
        base.Show();
        highScoreText.text = "High score :\n" + App.gameManager.playerModel.GetHighScore();
    }

    public void StartGame()
    {
        App.gameManager.StartGame();
        Hide();
    }
}
