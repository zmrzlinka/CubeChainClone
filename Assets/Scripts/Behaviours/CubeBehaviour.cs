using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

    private bool isDragable
    {
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
    internal Rigidbody rb;
    private Material mat;

    private CubeModel model;
    private CubeText[] cubeTexts;

    [SerializeField] private ParticleSystem spawnParticles;

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
                App.gameManager.GameOver();
            }
        }
    }

    public void Init(CubeModel model, Color color, bool showParticles)
    {
        this.cubeTexts = GetComponentsInChildren<CubeText>();
        this.model = model;
        foreach (var text in cubeTexts)
        {
            text.UpdateText(this.model.Value);
        }
        mat = GetComponent<Renderer>().material;
        mat.SetColor("_Color", color);  // TODO: set color according to the value
        if (showParticles)
        {
            var particles = spawnParticles.main;
            particles.startColor = color;
            spawnParticles.Play();
        }
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
        DOTween.Kill(rb);
        App.gameManager.cubes[model.Value].Remove(this);
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
        if (collision.gameObject.CompareTag("Cube"))
        {
            CubeBehaviour second = collision.gameObject.GetComponent<CubeBehaviour>();
            if (second.GetValue() == GetValue())
            {
                App.collisionManager.AddCollision(this, second);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("GameArea"))
        {
            App.gameManager.GameOver();
        }

        if (other.gameObject.CompareTag("CubeSpawnArea"))
        {
            if (rb.isKinematic == false)
            {
                releasedCubeTimeLeftSpawn = Time.unscaledTimeAsDouble;
            }
        }
    }

    public void AddForce(Vector3 normalizedForce)
    {
        rb.AddForce(normalizedForce * jumpForce, ForceMode.Impulse);
    }
    public void JumpToTheClosestCube()
    {
        CubeBehaviour closestCube = null;
        float closestDistance = Mathf.Infinity;
        List<CubeBehaviour> cubes = App.gameManager.cubes[model.Value];
        foreach (CubeBehaviour cube in cubes)
        {
            if (cube != this)
            {
                Vector3 distance = cube.transform.position - transform.position;
                if (distance.magnitude < closestDistance)
                {
                    closestDistance = distance.magnitude;
                    closestCube = cube;
                }
            }
        }
        if (closestCube == null)
        {
            AddForce(new Vector3(Random.Range(-0.1f, 0.1f), 1, Random.Range(-0.1f, 0.1f)).normalized);
        }
        else
        {
            rb.DOJump(closestCube.transform.position, 3, 1, 2);
        }
    }
}
