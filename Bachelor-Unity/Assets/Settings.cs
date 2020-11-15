using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings instance;
    
    public static bool isActive = false;
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
    
}
