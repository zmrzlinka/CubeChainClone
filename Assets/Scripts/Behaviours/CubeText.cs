using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CubeText : MonoBehaviour
{
    private TextMeshPro uiText;
    private void Awake()
    {
        uiText = GetComponent<TextMeshPro>();
    }
    public void UpdateText(int index)
    {
        uiText.text = index.ToString();
    }
}
