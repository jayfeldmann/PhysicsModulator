using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Boid : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer = default;

    [SerializeField] private Color _normalColor = default;
    [SerializeField] private Color _activeColor = default;
    [SerializeField] private Color _sendingColor = default;

    public SendController sendController;

    private Camera _mainCamera;
    private Vector2 _screenBounds;
    private float _objectWidth;
    private float _objectHeight;
    private int boundryOffset = 1;
    
    public int sendChannel = 1;
    public int sendCC = 1;
    
    public bool isSelected = false;

    [FormerlySerializedAs("target")] public GameObject boidTarget;

    //Boid specific movement Variables
    [HideInInspector]
    public Vector2 velocity = new Vector2(0,0);
    private Vector2 _acceleration = new Vector2(0,0);
    // private float maxSpeed = 15;
    // private float maxForce = 0.2f;
    //private int arriveRadius = 20;
    
    // private float desiredSeparation = 1f;
    // private float neighbourDistance = 2.0f;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    
    private void FixedUpdate()
    {
        SetBoidColor();
        if (SimulationController.simulateMovement)
        {
            MoveBoid();
        }
        if (sendController.isActive)
        {
            if (Settings.SendMode == SendMode.MIDI)
            {
                if (sendChannel > 0 && sendCC > 0 && sendChannel <=16)
                {
                    HandleSendValue();   
                }   
            }
 
        }
        
    }
    
    private void MoveBoid()
    {
        //TeleportToOtherSide();

        
        Flock(BoidSpawner.instance.cachedBoids);
        
        //SeekTarget(targetPos);

        // Do not touch
        velocity += _acceleration;
        velocity = Vector2.ClampMagnitude(velocity,BoidSettings.instance.maxSpeed);
        LookAt2D(velocity);
        transform.position +=  new Vector3(velocity.x,velocity.y,0) * Time.deltaTime;
        // Do not touch
        
        _acceleration *= 0;

    }

    private void Flock(List<Boid> boids)
    {
        Vector2 sep = Separate(boids);
        Vector2 ali = Align(boids);
        Vector2 coh = Cohesion(boids);
        Vector2 wander = Wander();
        Vector2 avoidWalls = AvoidWalls();
        //Weighing Forces
        sep *= BoidSettings.instance.separation;
        ali *= BoidSettings.instance.alignment;
        coh *= BoidSettings.instance.cohesion;
        wander *= BoidSettings.instance.wander;
        avoidWalls *= BoidSettings.instance.avoidWalls;
        //Add Forces to Acceleration
        ApplyForce(sep);
        ApplyForce(ali);
        ApplyForce(coh);
        ApplyForce(wander);
        ApplyForce(avoidWalls);
        Debug.DrawRay(transform.position,velocity.normalized*5);
    }

    Vector2 Separate(List<Boid> boids) //Checking near boids and steers away;
    {
        Vector2 steer = new Vector2(0,0);
        int count = 0;
        foreach (var boid in boids)
        {
            float dist = GetBoidDistance(boid);
            if (dist>0 && dist<BoidSettings.instance.desiredSeparation)
            {
                Vector2 diff = transform.position - boid.transform.position;
                diff.Normalize();
                diff /= dist;
                steer += diff;
                count++;
            }
        }

        if (count>0)
        {
            steer /= (float) count;
        }

        if (steer.magnitude>0) //Reynolds Steering: steering = desiredVelocity - velocity
        {
            steer.Normalize();
            steer *= BoidSettings.instance.maxSpeed;
            steer -= velocity;
            steer = Vector2.ClampMagnitude(steer, BoidSettings.instance.maxForce);
        }
        return steer;
    }

    bool IsInView(Boid boid)
    {
        //return true;
        float sightDistance = 7f;
        float periphery = Mathf.PI / 3f;
        
        var boidPos = boid.transform.position;
        var pos = transform.position;
        Vector2 comparison = boidPos - pos;
        float d = Vector2.Distance(pos, boidPos);
        float diff = Vector2.Angle(comparison, velocity);
        diff *= Mathf.Deg2Rad;

        if (diff < periphery && d > 0 && d < sightDistance)
        {
            return true;
        }
        return false;
    }

    Vector2 Align(List<Boid> boids) //Calc Average velocity for nearby Boids
    {
        Vector2 sum = new Vector2(0,0);
        int count = 0;
        foreach (var boid in boids)
        {
            float dist = GetBoidDistance(boid);
            if (dist>0 && dist < BoidSettings.instance.neighbourDistance)
            {
                if (IsInView(boid))
                {
                    sum += boid.velocity;
                    count++;
                }
            }
        }

        if (count>0)
        {
            sum /= (float) count;
            sum.Normalize();
            sum *= BoidSettings.instance.maxSpeed;
            Vector2 steer = sum - velocity;
            steer = Vector2.ClampMagnitude(steer, BoidSettings.instance.maxForce);
            return steer;
        }
        return Vector2.zero;
    }

    Vector2 Cohesion(List<Boid> boids)
    {
        Vector2 sum = new Vector2(0,0);
        int count = 0;
        foreach (var boid in boids)
        {
            float dist = GetBoidDistance(boid);
            if (dist > 0 && dist < BoidSettings.instance.neighbourDistance)
            {
                if (IsInView(boid))
                {
                    sum += new Vector2(boid.transform.position.x,boid.transform.position.y);
                    count++;
                }
            }
        }

        if (count > 0)
        {
            sum /= (float) count;
            return SeekTarget(sum);
        }

        return Vector2.zero;
    }

    float GetBoidDistance(Boid otherBoid)
    {
        return Vector2.Distance(transform.position, otherBoid.transform.position);
    }

    private Vector2 Wander()
    { 
        float spawnAreaAngle = Random.Range(-35,35);
        float spawnDistance = 2;
 
        Vector2 target = new Vector2(); 
        target = ExtensionMethods.Rotate(velocity,spawnAreaAngle) * spawnDistance;
        return SeekTarget(target);
    }
    
    private Vector2 SeekTarget(Vector3 targetPosition) //Seeks target Reynolds method. Closer to target, it slows down
    {
        Vector2 desiredVelocity = targetPosition - transform.position;

        float d = desiredVelocity.magnitude;
        desiredVelocity.Normalize();
        desiredVelocity *= BoidSettings.instance.maxSpeed;

        Vector2 steer = desiredVelocity - velocity;
        
        steer = Vector2.ClampMagnitude(steer, BoidSettings.instance.maxForce);

        return steer;
    }


    private float Heading2D(Vector2 input)
    {
        // Berechnet winkel zwichen OBEN und inputwinkel
        float angle = Vector2.Angle( Vector2.up,input);
        Vector3 cross = Vector3.Cross( Vector2.up,input);

        if (cross.z>0)
        {
            angle = 360 - angle;
        }

        return angle;
    }

    private void LookAt2D(Vector2 target)
    {
        float rot_z = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    private void ApplyForce(Vector2 force)
    {
        _acceleration += force;
    }

    private Vector2 AvoidWalls()
    {
        var pos = transform.position;
        Vector2 desired = new Vector2();
        bool isWall = false;
        if (pos.x >= _screenBounds.x - _objectWidth-boundryOffset)
        {
            desired = new Vector2(-BoidSettings.instance.maxSpeed,velocity.y);
            isWall = true;
        } 
        else if (pos.x <= _screenBounds.x *-1 + _objectWidth +boundryOffset)
        {
            desired = new Vector2(BoidSettings.instance.maxSpeed,velocity.y);  
            isWall = true;
        }

        if (pos.y >= _screenBounds.y - _objectHeight-boundryOffset)
        {
            desired = new Vector2(velocity.x,-BoidSettings.instance.maxSpeed);
            isWall = true;
        }
        else if (pos.y <= _screenBounds.y *-1 + _objectHeight+boundryOffset)
        {
            desired = new Vector2(velocity.x,BoidSettings.instance.maxSpeed);
            isWall = true;
        }

        if (isWall)
        {
            desired.Normalize();
            desired *= BoidSettings.instance.maxSpeed;
            var steer = desired-velocity;
            steer = Vector2.ClampMagnitude(steer,BoidSettings.instance.maxForce);
            return steer;
        }

        return Vector2.zero;
    }


    public void SelectBoid()
    {
        _spriteRenderer.color = _activeColor;
        isSelected = true;
    }

    public void DeselectBoid()
    {
        _spriteRenderer.color = _normalColor;
        isSelected = false;
    }


    private void Start()
    {
        _screenBounds = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _mainCamera.transform.position.z));
        var bounds = _spriteRenderer.bounds;
        _objectWidth = bounds.extents.x;
        _objectHeight = bounds.extents.y;
        boidTarget = GameObject.Find("Target");
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

    public int GetRotationAsClampedValue(int lowerBoarder, int upperBorder)
    {
        var curAngle = transform.rotation.eulerAngles.z;
        var midiAngle = Mathf.InverseLerp(0, 360, curAngle);
        midiAngle *= upperBorder;
        int returnValue = Mathf.CeilToInt(midiAngle);
        returnValue = Mathf.Clamp(returnValue, lowerBoarder, upperBorder);
        return returnValue;
    }

    public int GetVelocityAsClampedValue(int lowerBoarder, int upperBorder)
    {
        var velMagnitude = velocity.magnitude;
        var midiVel = Mathf.InverseLerp(0, BoidSettings.instance.maxSpeed, velMagnitude);
        midiVel *= upperBorder;
        int returnVal = Mathf.CeilToInt(midiVel);
        returnVal = Mathf.Clamp(returnVal, lowerBoarder, upperBorder);
        return returnVal;
    }

    private void SetBoidColor()
    {
        if (!isSelected)
        {
            if (sendController.isActive)
            {
                _spriteRenderer.color = _sendingColor;
            }
            else
            {
                _spriteRenderer.color = _normalColor;
            }
        }
    }
    
    void HandleSendValue()
    {
        bool modeIsOsc = Settings.SendMode == SendMode.OSC;
        int value = modeIsOsc ? sendController.oscHandler.oscValue : sendController.midiHandler.midiValue;
        bool hasChanged = false;
        var osc = sendController.oscHandler;
        int lower = modeIsOsc ? osc.oscLowerValue : 0;
        int upper = modeIsOsc ? osc.oscUpperValue : 127;

        switch (sendController.sendModulators[sendController.sendModulatorIndex])
        {
            case "PosX":
                value = GetYPosAsClampedValue(lower, upper);
                hasChanged = true;
                break;
            case "PosY":
                value = GetXPosAsClampedValue(lower, upper);
                hasChanged = true;
                break;
            case "Rotation":
                value = GetRotationAsClampedValue(lower, upper);
                hasChanged = true;
                break;
            case "Velocity":
                value = GetVelocityAsClampedValue(lower, upper);
                hasChanged = true;
                break;
            default:
                value = GetXPosAsClampedValue(lower, upper);
                hasChanged = true;
                Debug.LogError("Modulator in BOID not found. Please check Sendcontroller Modulators list. Using Default Modulator X Position.");
                break;
        }

        if (hasChanged)
        {
            if (modeIsOsc)
            {
                osc.oscValue = value;
            }
            else
            {
                sendController.midiHandler.midiValue = value;
            }
        }
    }
    
    private void OnMouseDown()
    {
        if (!isSelected && !BoidOptions.isActive)
        {
            BoidOptions.sendController = this.sendController;
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
