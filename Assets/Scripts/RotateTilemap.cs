using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RotateTilemap : MonoBehaviour
{
    public Tilemap tilemap;      // Reference to the Tilemap you want to rotate
    public float rotationSpeed = 30.0f;  // Speed of rotation in degrees per second
    public Vector3 rotationPivot;  // The pivot point around which to rotate the Tilemap

    private void Update()
    {
        // Rotate the Tilemap continuously
        float rotationAngle = rotationSpeed * Time.deltaTime;
        tilemap.transform.RotateAround(rotationPivot, Vector3.back, rotationAngle);
    }
}
