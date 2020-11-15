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

    private Camera _mainCamera;
    private Vector2 _screenBounds;
    private float _objectWidth;
    private float _objectHeight;

    public int sendChannel = -1;
    public int sendCC = -1;

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

    public int GetXPosAsMidiValue()
    {
        var x = Mathf.InverseLerp(_screenBounds.x * -1,_screenBounds.x, transform.position.x);
        x *= 127;
        int returnInt = Mathf.CeilToInt(x);
        if(x>127) throw new Exception("x is over 127. Your Maths are Wrong!");
        return returnInt;
    }
    
    public int GetYPosAsMidiValue()
    {
        var y = Mathf.InverseLerp(_screenBounds.y * -1,_screenBounds.y, transform.position.y);
        y *= 127;
        int returnInt = Mathf.CeilToInt(y);
        if(y>127) throw new Exception("x is over 127. Your Maths are Wrong!");
        return returnInt;
    }
    
    private void Update()
    {
        MoveBoid();
        if (isActive)
        {
            if (sendChannel >= 0 && sendCC >= 0 && sendChannel <=16)
            {
                HandleMidi();    
            }    
        }
        
    }

    private void HandleMidi()
    {
        int value = -1;
        switch (sendModulator)
        {
            case SendModulator.PosX:
                value = GetXPosAsMidiValue();
                break;
            case SendModulator.PosY:
                value = GetYPosAsMidiValue();
                break;
        }
        if (value>=0) SendMidi(value);
    }

    private void SendMidi(int value)
    {
        var activeDevice = MidiDeviceManager.activeMidiDevice;
        if (activeDevice >=0)
        {
            using (MidiOut midiOut = new MidiOut(activeDevice))
            {
                MidiMessage message = MidiMessage.ChangeControl(sendCC,value,sendChannel);
                midiOut.Send(message.RawData);
            }
        }
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
