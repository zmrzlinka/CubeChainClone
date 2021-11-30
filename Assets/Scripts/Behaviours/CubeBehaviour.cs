using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviour : MonoBehaviour
{

    private float distToCamera;

    void Start()
    {
        distToCamera = Camera.main.WorldToScreenPoint(transform.position).z;
    }

    void OnMouseDown()
    {
        Debug.Log("on mouse down");
    }

    void OnMouseUp()
    {
        Debug.Log("on mouse up");
    }

    void OnMouseDrag()
    {
        Vector3 mousePositionWithDepth = Input.mousePosition;
        mousePositionWithDepth.z = distToCamera;

        Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePositionWithDepth);
        transform.position = new Vector3(Mathf.Clamp(mousePosInWorld.x, -2.5f, 2.5f), transform.position.y, transform.position.z);
    }
}
