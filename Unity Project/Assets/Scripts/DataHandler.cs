using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    protected SendController _sendController;

    private void Awake()
    {
        _sendController = GetComponent<SendController>();
    }
    
}
