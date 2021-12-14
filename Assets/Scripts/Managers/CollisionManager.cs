using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager
{

    private Dictionary<CubeBehaviour, CubeBehaviour> collisions = new Dictionary<CubeBehaviour, CubeBehaviour>();

    public void AddCollision(CubeBehaviour first, CubeBehaviour second)
    {
        if(collisions.ContainsKey(second) &&
                collisions[second] == first)
        {
            int newValue = first.GetValue() * 2;

            App.gameManager.playerModel.AddScore(newValue);

            CubeBehaviour cube = App.gameManager.SpawnCube(newValue, Vector3.Lerp(first.transform.position, second.transform.position, 0.5f));
            cube.DisableKinematic();
            cube.AddForce(new Vector3(Random.Range(-0.1f, 0.1f), 1, Random.Range(-0.1f, 0.1f)).normalized);     // TODO: fire cubes in direction with cubes with same value
            collisions.Remove(second);
            first.DestroyCube();
            second.DestroyCube();
        }
        else
        {
            collisions.Add(first, second);
        }
    }
}
