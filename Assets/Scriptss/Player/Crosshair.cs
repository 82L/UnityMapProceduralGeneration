using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private Plane GroundLevel;

    void Start()
    {
        // Cursor.visible = false;
        GroundLevel = new Plane(Vector3.up, Vector3.zero);
    }

    void Update()
    {
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (GroundLevel.Raycast(ray, out distance))
        {
            Vector3 point = ray.GetPoint(distance);
            point.y += 0.1f;
            transform.position = point;
        }
    }
}
