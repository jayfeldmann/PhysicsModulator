using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Flowfield : MonoBehaviour
{
    public GameObject debugArrow;
    public Vector2[,] field;
    private int vertical, horizontal, cols, rows;
    private int resolution = 2;
    private Camera _mainCam;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        vertical = (int) _mainCam.orthographicSize;
        horizontal = (int)(vertical * ((float)Screen.width/(float)Screen.height));
        cols = horizontal * 2 ;
        rows = vertical * 2 ;
        field = new Vector2[cols,rows];
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                field[i,j] = new Vector2(0,Random.Range(0,10));
            }
        }

    }
}
