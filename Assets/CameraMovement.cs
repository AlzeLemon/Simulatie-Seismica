using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Vector3 dragOrigin;
    Camera cam;
    public float mult = 50;
    public PointManager pm;
    void Start() {
        cam = GetComponent<Camera>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(2)) {
            dragOrigin = Input.mousePosition;
        }
        if (Input.GetMouseButton(2)) {
            transform.position += (dragOrigin - Input.mousePosition)/mult;
            dragOrigin = Input.mousePosition;
        }
        cam.orthographicSize -= 6*Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetKeyDown(KeyCode.E)) {
            pm.Explode(cam.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
