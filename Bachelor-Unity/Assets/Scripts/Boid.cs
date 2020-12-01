using System;
using System.Collections;
using System.Collections.Generic;
using NAudio.Midi;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Boid : MonoBehaviour
{
    public float boidSpeed = 5;

    [SerializeField] private SpriteRenderer _spriteRenderer = default;

    [SerializeField] private Color _normalColor = default;
    [SerializeField] private Color _activeColor = default;

    public SendController sendController;

    private Camera _mainCamera;
    private Vector2 _screenBounds;
    private float _objectWidth;
    private float _objectHeight;

    public int sendChannel = 1;
    public int sendCC = 1;

    public bool isActive = false; //Should Boid send MIDI or not.
    public bool isSelected = false;

    public SendModulator sendModulator = SendModulator.PosX;
    public enum SendModulator
    {
        PosX,
        PosY
    }
    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void SetSendMod(int mod)
    {
        switch (mod)
        {
            case 0:
                sendModulator = SendModulator.PosX;
                break;
            case 1:
                sendModulator = SendModulator.PosY;
                break;
            default:
                sendModulator = SendModulator.PosX;
                break;
        }
    }

    public void SelectBoid()
    {
        if (isSelected)
        {
            _spriteRenderer.color = _normalColor;
            isSelected = false;
        }
        else
        {
            _spriteRenderer.color = _activeColor;
            isSelected = true;
        }
    }
    private void Start()
    {
        _screenBounds = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _mainCamera.transform.position.z));
        var bounds = _spriteRenderer.bounds;
        _objectWidth = bounds.extents.x;
        _objectHeight = bounds.extents.y;
    }
    
    public int GetXPosAsClampedValue(int lowerBorder, int upperBorder)
    {
        var x = Mathf.InverseLerp(_screenBounds.x * -1,_screenBounds.x, transform.position.x);
        x *= upperBorder;
        int returnInt = Mathf.CeilToInt(x);
        returnInt = Mathf.Clamp(returnInt, lowerBorder, upperBorder);
        return returnInt;
    }
    
    public int GetYPosAsClampedValue(int lowerBorder, int upperBorder)
    {
        var y = Mathf.InverseLerp(_screenBounds.y * -1,_screenBounds.y, transform.position.y);
        y *= upperBorder;
        int returnInt = Mathf.CeilToInt(y);
        returnInt = Mathf.Clamp(returnInt, lowerBorder, upperBorder);
        return returnInt;
    }

    public int GetXPosAsMidiValue()
    {
        return GetXPosAsClampedValue(0, 127);
    }

    public int GetYPosAsMidiValue()
    {
        return GetYPosAsClampedValue(0, 127);
    }
    

    
    private void Update()
    {
        MoveBoid();
        if (sendController.isActive)
        {
            if (Settings.SendMode == SendMode.MIDI)
            {
                if (sendChannel >= 0 && sendCC >= 0 && sendChannel <=16)
                {
                    HandleMidiValue();    
                }   
            }
 
        }
        
    }

    private void HandleMidiValue()
    {
        //get last midi value, in order to not JUMP if there is a false value.
        int value = sendController.midiHanler.midiValue;
        bool hasChanged = false;
        switch (sendModulator)
        {
            case SendModulator.PosX:
                value = GetXPosAsMidiValue();
                hasChanged = true;
                break;
            case SendModulator.PosY:
                value = GetYPosAsMidiValue();
                hasChanged = true;
                break;
        }
        if (hasChanged && value>=0) sendController.midiHanler.midiValue = value;
    }



    private void OnMouseDown()
    {
        OpenBoidOptions();
    }

    private void OpenBoidOptions()
    {
        BoidOptions.activeBoid = this;
        UIManager.instance.boidOptionsPanel.SetActive(true);
    }

    private void MoveBoid()
    {
        TeleportToOtherSide();
        transform.position += transform.right * (boidSpeed * Time.deltaTime);
    }

    private void TeleportToOtherSide()
    {
        Vector3 viewPos = transform.position;
        if (viewPos.x >= (_screenBounds.x - _objectWidth))
        {
            viewPos.x = viewPos.x * -1 + _objectWidth;
            transform.position = viewPos;
        }

        if (viewPos.x <= _screenBounds.x *-1 + _objectWidth)
        {
            viewPos.x = viewPos.x * -1 - _objectWidth;
            transform.position = viewPos;
        }

        if (viewPos.y >= _screenBounds.y - _objectHeight)
        {
            viewPos.y = viewPos.y * -1 + _objectHeight;
            transform.position = viewPos;
        }

        if (viewPos.y <= _screenBounds.y *-1 + _objectHeight)
        {
            viewPos.y = viewPos.y * -1 - _objectHeight;
            transform.position = viewPos;
        }
    }
}
