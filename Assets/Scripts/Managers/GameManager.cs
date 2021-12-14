using System.Collections;
using Models;
using UnityEngine;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{

    public static int Score { get; internal set; } = 0;

    public CubeBehaviour cubePrefab;
    public Transform spawnPosition;
    public CubeConfig cubeConfig;

    private WaitForSeconds wait = new WaitForSeconds(0.5f);

    public UnityEvent OnLevelCleared = new UnityEvent();

    public void AddScore(int scoreToAdd)
    {
        Score += scoreToAdd;
        //TODO: pÌsaù skÛre niekam na obrazovku v InGameScreene
        Debug.Log($"Actual score is: {Score}");
    }

    public void ResetScore()
    {
        Score = 0;
        Debug.Log("Score set to zero.");
    }

    void Start()
    {
        App.gameManager = this;
        App.collisionManager = new CollisionManager();
        App.screenManager.Show<MenuScreen>();
    }

    public void StartGame()
    {
        ResetScore();
        App.screenManager.Show<InGameScreen>();
        SpawnCube(2, spawnPosition.position);
    }

    public void RestartGame()
    {
        ResetScore();
        OnLevelCleared.Invoke();
        StartGame();
    }

    public void EndGame()
    {
        ResetScore();
        App.screenManager.Show<MenuScreen>();
        OnLevelCleared.Invoke();
    }

    public CubeBehaviour SpawnCube(int value, Vector3 position)
    {
        CubeModel model = new CubeModel(value);
        CubeBehaviour cube = Instantiate(cubePrefab, position, Quaternion.identity);
        cube.Init(model, cubeConfig.GetColor(model.Value));
        return cube;
    }

    public void StartSpawnCubeCoroutine()
    {
        StartCoroutine(SpawnNextCube());
    }

    private IEnumerator SpawnNextCube()
    {
        yield return wait;
        SpawnCube(2, spawnPosition.position);  // TODO: Random starting value
    }
}
