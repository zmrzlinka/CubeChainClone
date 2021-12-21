using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScreen : ScreenBase
{

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject newHighScoreText;

    public override void Show(Dictionary<string, object> param)
    {
        base.Show();
        scoreText.text = "Score: " + App.gameManager.playerModel.GetScore();
        highScoreText.text = "High score: " + App.gameManager.playerModel.GetHighScore();

        newHighScoreText.SetActive((bool)param["newHighScore"]);
    }

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
