using System.Collections;
using Models;
using UnityEngine;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{

    public CubeBehaviour cubePrefab;
    public Transform spawnPosition;

    private WaitForSeconds wait = new WaitForSeconds(0.5f);

    public UnityEvent OnLevelCleared = new UnityEvent();

    void Start()
    {
        App.gameManager = this;
        App.screenManager.Show<MenuScreen>();
    }

    public void StartGame()
    {
        App.screenManager.Show<InGameScreen>();
        SpawnCube();
    }
    public void EndGame()
    {
        App.screenManager.Show<MenuScreen>();
        OnLevelCleared.Invoke();
    }

    public void SpawnCube()
    {
        CubeModel model = new CubeModel(2); // TODO: Random starting value
        CubeBehaviour cube = Instantiate(cubePrefab, spawnPosition.position, Quaternion.identity);
        cube.Init(model);
    }

    public void StartSpawnCubeCoroutine()
    {
        StartCoroutine(SpawnNextCube());
    }

    private IEnumerator SpawnNextCube()
    {
        yield return wait;
        SpawnCube();
    }
}
