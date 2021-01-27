using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// Klasse um die Boids zu sammeln und Gruppiert zu verwalten
/// </summary>
public class BoidArea : MonoBehaviour
{
    public static BoidArea instance;

    public Vector2 screenBounds;
    [SerializeField] private Collider2D _collider2D;
    public Camera mainCam;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        mainCam = Camera.main;
        if (!mainCam) return;
        
        screenBounds = mainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCam.transform.position.z));
    }



    public void ResizeBoundingArea()
    {
        
    }
}
