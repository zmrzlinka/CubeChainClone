using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class CubeBehaviour : MonoBehaviour
{

    public float startForce = 10f;

    private bool isDragable;
    private float distToCamera;
    private Rigidbody rb;

    private CubeModel model;
    private CubeText[] cubeTexts;

    private void Start()
    {
        isDragable = true;
        distToCamera = Camera.main.WorldToScreenPoint(transform.position).z;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }
    public void Init(CubeModel model)
    {
        this.cubeTexts = GetComponentsInChildren<CubeText>();
        this.model = model;
        foreach (var text in cubeTexts)
        {
            text.UpdateText(this.model.Value);
        }
    }
    private void OnEnable()
    {
        App.gameManager.OnLevelCleared.AddListener(OnLevelCleared);
    }
    private void OnDisable()
    {
        App.gameManager.OnLevelCleared.RemoveListener(OnLevelCleared);
    }
    private void OnLevelCleared()
    {
        Destroy(this.gameObject);
    }
    private void OnMouseUp()
    {
        if (isDragable)
        {
            isDragable = false;
            rb.isKinematic = false;
            rb.AddForce(Vector3.forward * startForce, ForceMode.Impulse);
            App.gameManager.StartSpawnCubeCoroutine();
        }
    }

    private void OnMouseDrag()
    {
        if (isDragable)
        {
            Vector3 mousePositionWithDepth = Input.mousePosition;
            mousePositionWithDepth.z = distToCamera;

            Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePositionWithDepth);
            transform.position = new Vector3(Mathf.Clamp(mousePosInWorld.x, -2.5f, 2.5f), transform.position.y, transform.position.z);
        }
    }

}
