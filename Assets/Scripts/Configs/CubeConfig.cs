using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="cubeConfig", menuName = "Configs/CubeConfig", order = 1)]
public class CubeConfig : ScriptableObject
{
    public List<Color> cubeColors;

    public Color GetColor(int value)
    {
        return cubeColors[(int)(Mathf.Log(value, 2) - 1) % cubeColors.Count];
    }
}
