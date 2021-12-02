using System.Collections;
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
        Instantiate(cubePrefab, spawnPosition.position, Quaternion.identity);
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
