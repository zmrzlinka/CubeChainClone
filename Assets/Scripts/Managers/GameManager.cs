using System.Collections;
using Models;
using UnityEngine;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{

    public CubeBehaviour cubePrefab;
    public Transform spawnPosition;
    public CubeConfig cubeConfig;

    private WaitForSeconds wait = new WaitForSeconds(0.5f);

    public UnityEvent OnLevelCleared = new UnityEvent();

    void Start()
    {
        App.gameManager = this;
        App.collisionManager = new CollisionManager();
        App.screenManager.Show<MenuScreen>();
    }

    public void StartGame()
    {
        App.screenManager.Show<InGameScreen>();
        SpawnCube(2, spawnPosition.position);
    }
    public void EndGame()
    {
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
