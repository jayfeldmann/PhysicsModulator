using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidAreaTest : MonoBehaviour
{
    [SerializeField] private BoxCollider _boxCollider;
    private float objectWidth;

    private float objectHeight;
    
    void Start()
    {
        objectWidth = _boxCollider.bounds.extents.x;
        objectHeight = _boxCollider.bounds.extents.y;
    }
    private void LateUpdate()
    {
        Vector3 screenBounds = BoidArea.instance.screenBounds;
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x -objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y -objectHeight);
        transform.position = viewPos;
    }
}
