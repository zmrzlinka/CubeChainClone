using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class CubeBehaviour : MonoBehaviour
{

    public float startForce = 10f;
    public float jumpForce = 10f;

    public double cubeTimeToLeaveSpawnInSeconds = 2d;

    private double releaseCubeTime = double.NegativeInfinity;
    private double releasedCubeTimeDeadline = double.NegativeInfinity;
    private double releasedCubeTimeLeftSpawn = double.NegativeInfinity;

    private bool _isDragable = true;
    
    private bool isDragable { 
        set
        {
            _isDragable = value;
        }
        get
        {
            return _isDragable && App.screenManager.IsScreenDisplayed<InGameScreen>();
        }
    }

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
    private void Update()
    {
        if (isDragable == false)
        {
            if (Time.unscaledTimeAsDouble > releasedCubeTimeDeadline && releasedCubeTimeLeftSpawn < releaseCubeTime)
            {
                GameOver();
                Debug.Log("game over due to time");
            }
                
        }
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
            releaseCubeTime = Time.unscaledTimeAsDouble;
            releasedCubeTimeDeadline = releaseCubeTime + cubeTimeToLeaveSpawnInSeconds;
            
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
            GameOver();
        }

        if (other.gameObject.CompareTag("CubeSpawnArea"))
        {
            if (rb.isKinematic == false)
            {
                releasedCubeTimeLeftSpawn = Time.unscaledTimeAsDouble;
                Debug.Log("on cube spawn trigger exit");
            }
            
        }
    }

    public void GameOver()
    {
        App.screenManager.Show<GameOverScreen>();
        App.screenManager.Hide<InGameScreen>();
    }

    public void AddForce(Vector3 normalizedForce)
    {
        rb.AddForce(normalizedForce * jumpForce, ForceMode.Impulse);
    }
}
