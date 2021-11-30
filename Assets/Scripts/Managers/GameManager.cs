using UnityEngine;

public class GameManager : MonoBehaviour
{

    public CubeBehaviour cubePrefab;
    public Transform spawnPosition;

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

    public void SpawnCube()
    {
        Instantiate(cubePrefab, spawnPosition.position, Quaternion.identity);
    }
}
