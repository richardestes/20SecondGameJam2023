using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapCondenser : MonoBehaviour
{
    public Tilemap TilemapTop;
    public Tilemap TilemapBottom;
    public float CondenseSpeed;
    public bool IsCondensing;

    private void Update()
    {
        if (IsCondensing)
        {
            var transform1 = TilemapTop.transform;
            transform1.position = new Vector3(0, transform1.position.y - (Time.deltaTime * CondenseSpeed), 0);
            var transform2 = TilemapBottom.transform;
            transform2.position = new Vector3(0, transform2.position.y + (Time.deltaTime * CondenseSpeed), 0);
        }
    }
}
