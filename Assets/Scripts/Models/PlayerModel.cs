using UnityEngine;
using UnityEngine.Events;

public class PlayerModel
{

    private int score;
    private int highScore;

    public UnityEvent onScoreChanged = new UnityEvent();

    public PlayerModel()
    {
        score = 0;
        if(PlayerPrefs.HasKey("highScore"))
        {
            highScore = PlayerPrefs.GetInt("highScore");
        }
        else
        {
            highScore = 0;
        }
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        onScoreChanged.Invoke();
    }

    public void ResetScore()
    {
        score = 0;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetHighScore()
    {
        return highScore;
    }

    public bool CheckHighScore()
    {
        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highScore", highScore);
            Debug.Log("new high score");
            return true;
        }
        return false;
    }
}
