using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{

    public CubeBehaviour cubePrefab;
    public Transform spawnPosition;
    public CubeConfig cubeConfig;
    public PlayerModel playerModel;

    private WaitForSeconds wait = new WaitForSeconds(0.5f);

    public UnityEvent OnLevelCleared = new UnityEvent();

    public Dictionary<int, List<CubeBehaviour>> cubes = new Dictionary<int, List<CubeBehaviour>>();
    void Start()
    {
        App.gameManager = this;
        App.collisionManager = new CollisionManager();
        playerModel = new PlayerModel();
        App.screenManager.Show<MenuScreen>();
    }

    public void StartGame()
    {
        playerModel.ResetScore();
        App.screenManager.Show<InGameScreen>();
        SpawnCube(2, spawnPosition.position);
    }

    public void RestartGame()
    {
        OnLevelCleared.Invoke();
        StartGame();
    }

    public void EndGame()
    {
        App.screenManager.Show<MenuScreen>();
        OnLevelCleared.Invoke();
    }

    public void GameOver()
    {
        bool newHighScore = playerModel.CheckHighScore();
        Dictionary<string, object> param = new Dictionary<string, object>
        {
            {"newHighScore", newHighScore}
        };
        App.screenManager.Show<GameOverScreen>(param);
        App.screenManager.Hide<InGameScreen>();
    }

    public CubeBehaviour SpawnCube(int value, Vector3 position, bool showParticles = false)
    {
        CubeModel model = new CubeModel(value);
        CubeBehaviour cube = Instantiate(cubePrefab, position, Quaternion.identity);
        cube.Init(model, cubeConfig.GetColor(model.Value), showParticles);
        if (!cubes.ContainsKey(model.Value))
        {
            cubes.Add(model.Value, new List<CubeBehaviour>());
        }
        cubes[model.Value].Add(cube);
        return cube;
    }

    public void StartSpawnCubeCoroutine()
    {
        StartCoroutine(SpawnNextCube());
    }

    private IEnumerator SpawnNextCube()
    {
        yield return wait;
        // TODO: Random starting value
        SpawnCube(2, spawnPosition.position);
    }
}
