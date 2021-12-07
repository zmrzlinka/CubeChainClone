using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class CubeBehaviour : MonoBehaviour
{

    public float startForce = 10f;
    public float jumpForce = 10f;

    private bool isDragable;
    private float distToCamera;
    private Rigidbody rb;
    private Material mat;

    private CubeModel model;
    private CubeText[] cubeTexts;

    private void Awake()
    {
        isDragable = true;
        distToCamera = Camera.main.WorldToScreenPoint(transform.position).z;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }
    public void Init(CubeModel model, Color color)
    {
        this.cubeTexts = GetComponentsInChildren<CubeText>();
        this.model = model;
        foreach (var text in cubeTexts)
        {
            text.UpdateText(this.model.Value);
        }
        mat = GetComponent<Renderer>().material;
        mat.SetColor("_Color", color);  // TODO: set color according to the value
    }
    private void OnEnable()
    {
        App.gameManager.OnLevelCleared.AddListener(DestroyCube);
    }
    private void OnDisable()
    {
        App.gameManager.OnLevelCleared.RemoveListener(DestroyCube);
    }
    public void DestroyCube()
    {
        Destroy(this.gameObject);
    }
    public void DisableKinematic()
    {
        isDragable = false;
        rb.isKinematic = false;
    }
    private void OnMouseUp()
    {
        if (isDragable)
        {
            DisableKinematic();
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

    public int GetValue()
    {
        return model.Value;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Cube"))
        {
            CubeBehaviour second = collision.gameObject.GetComponent<CubeBehaviour>();
            if(second.GetValue() == GetValue())
            {
                App.collisionManager.AddCollision(this, second);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("GameArea"))
        {
            Debug.Log("game over");     // TODO: add colliders to side walls
            // TODO: handle game over
        }
    }

    public void AddForce(Vector3 normalizedForce)
    {
        rb.AddForce(normalizedForce * jumpForce, ForceMode.Impulse);
    }
}
