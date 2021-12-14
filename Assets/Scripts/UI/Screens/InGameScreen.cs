using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameScreen : ScreenBase
{
    public TextMeshProUGUI scoreText;

    public override void Show()
    {
        base.Show();
        App.gameManager.playerModel.onScoreChanged.AddListener(UpdateScoreText);
        UpdateScoreText();
    }

    public override void Hide()
    {
        App.gameManager.playerModel.onScoreChanged.RemoveListener(UpdateScoreText);
        base.Hide();
    }

    public void ReturnToMenu()
    {
        App.gameManager.EndGame();
        Hide();
    }

    public void UpdateScoreText()
    {
        scoreText.text = App.gameManager.playerModel.GetScore().ToString();
    }
}
