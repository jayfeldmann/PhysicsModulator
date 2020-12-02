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
    
    public bool isSelected = false;

    public Rigidbody2D rigidBody2d;

    public GameObject target;

    //Boid specific movement Variables
    private float mass = 1;
    private Vector2 _velocity = new Vector2(0,0);
    private Vector2 _acceleration = new Vector2(0,0);
    private float maxSpeed = 4;
    private float maxForce = 0.1f;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    
    private void MoveBoid()
    {
        Vector2 targetPos = target.transform.position;
        TeleportToOtherSide();
        Debug.DrawRay(transform.position,transform.up*8);

        SeekTarget(targetPos);
        
        _velocity += _acceleration;
        _velocity = Vector2.ClampMagnitude(_velocity,maxSpeed);
        
        transform.position +=  new Vector3(_velocity.x,_velocity.y,0) * Time.deltaTime;

        _acceleration *= 0;

    }

    private void ApplyForce(Vector2 force)
    {
        _acceleration += force;
    }

    private void SeekTarget(Vector3 targetPosition)
    {
        Vector2 desiredVelocity = targetPosition - transform.position;
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;
        
        Vector2 steer = desiredVelocity - _velocity;
        
        steer = Vector2.ClampMagnitude(steer, maxForce);
        
        ApplyForce(steer);
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
        target = GameObject.Find("Target");
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
        switch (sendController.sendModulators[sendController.sendModulatorIndex])
        {
            case "PosX":
                value = GetXPosAsMidiValue();
                hasChanged = true;
                break;
            case "PosY":
                value = GetYPosAsMidiValue();
                hasChanged = true;
                break;
            default:
                value = GetXPosAsMidiValue();
                hasChanged = true;
                Debug.LogError("Modulator in BOID not found. Please check Sendcontroller Modulators list. Using Default Modulator X Position.");
                break;
        }
        if (hasChanged && value>=0) sendController.midiHanler.midiValue = value;
    }

    private void HandleOscValue()
    {
        int value = sendController.oscHandler.oscValue;
        bool hasChanged = false;
        var osc = sendController.oscHandler;
        switch (sendController.sendModulators[sendController.sendModulatorIndex])
        {
            //TODO: Add Upper and Lower border controls. After OSC Panel created.
            case "PosX":
                value = GetXPosAsClampedValue(osc.oscLowerValue,osc.oscUpperValue);
                hasChanged = true;
                break;
            case "PosY":
                value = GetYPosAsClampedValue(osc.oscLowerValue,osc.oscUpperValue);
                hasChanged = true;
                break;
            default:
                value = GetXPosAsClampedValue(osc.oscLowerValue,osc.oscUpperValue);
                hasChanged = true;
                Debug.LogError("Modulator in BOID not found. Please check Sendcontroller Modulators list. Using Default Modulator X Position.");
                break;
        }

        if (hasChanged) sendController.oscHandler.oscValue = value;
    }



    private void OnMouseDown()
    {
        if (!isSelected)
        {
            MidiOptions.sendController = this.sendController;
            UIManager.instance.midiOptionsPanel.SetActive(true);
            SelectBoid();
        }
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
