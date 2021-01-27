using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

/// <summary>
/// Ein Boid definiert ein Physik Objekt, das die Regeln für die Flock Bewegungen nach Craig Reynolds.
/// Die Physikimplementierung basiert auf Daniel Shiffmans Java implementierung aus "The Nature of Code".
/// Auf ihnen basierende Funktionen sind stets dementsprechend gekennzeichnet.
/// Zusätzlich werden die Daten der Objekte vorbereitet und an die SendHandler (OSC und MIDI) weitergereicht.
/// </summary>
public class Boid : MonoBehaviour
{
    //Hilfsvariabeln, die zur Darstellung des Boid Objektes dienen
    [SerializeField] private SpriteRenderer _spriteRenderer = default;

    [SerializeField] private Color _normalColor = default;
    [SerializeField] private Color _activeColor = default;
    [SerializeField] private Color _sendingColor = default;

    public SendController sendController;

    //Hilfsvariablen, die die Ebene darstellen, auf der sich die Boids bewegen können.
    private Camera _mainCamera;
    private Vector2 _screenBounds;
    private float _objectWidth;
    private float _objectHeight;
    private int boundryOffset = 1;
    
    //Midi Einstellungen, auf die der Boid sendet.
    public int sendChannel = 1;
    public int sendCC = 1;
    
    //Hilfsvariable zur Darstellung
    public bool isSelected = false;

    //Bewegungsvariablen des Boids.
    [HideInInspector]
    public Vector2 velocity = new Vector2(0,0);
    private Vector2 _acceleration = new Vector2(0,0);

    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    
    //Update sorgt pro Frame für zwei Dinge:
    // 1. Die Berechnung der Position, Geschwindigkeit und Rotation des Boid.
    // 2. Die Verarbeitung dieser Daten zu Midi/OSC werten.
    private void Update()
    {
        SetBoidColor();
        if (SimulationController.simulateMovement)
        {
            MoveBoid();
        }
        if (sendController.isActive && Settings.SendMode != SendMode.DISABLED)
        {
            HandleSendValue();
        }
    }
    
    //Berechnet die Bewegungen der Boids.
    private void MoveBoid()
    {
       
        //Flock startet die Flocking Berechnung. Diese aktualisiert pro frame den Velocity-Vektor anhand der anderen Boids im Sichtfeld
        Flock(BoidSpawner.instance.cachedBoids);
        
        //Berechnete Velocity wird angewendet und der Boid rotiert in die passende Richtung.
        velocity += _acceleration;
        velocity = Vector2.ClampMagnitude(velocity,BoidSettings.instance.maxSpeed);
        LookAt2D(velocity);
        transform.position +=  new Vector3(velocity.x,velocity.y,0) * Time.deltaTime;

        _acceleration *= 0;

    }

    //Berechnung der Velocity auf Basis von Craig Reynolds Paper und Daniel Shiffmans Java Implementierung.
    private void Flock(List<Boid> boids)
    {
        //Die einzelnen Kraftvektoren der Verhaltensregeln der Boids werden unabhängig voneinander berechnet.
        Vector2 sep = Separate(boids);
        Vector2 ali = Align(boids);
        Vector2 coh = Cohesion(boids);
        Vector2 wander = Wander();
        Vector2 avoidWalls = AvoidWalls();
        
        //Jeder Vektor hat eine in den BoidSettings festgelegte Gewichtung. Diese wird hier angewendet.
        sep *= BoidSettings.instance.separation;
        ali *= BoidSettings.instance.alignment;
        coh *= BoidSettings.instance.cohesion;
        wander *= BoidSettings.instance.wander;
        avoidWalls *= BoidSettings.instance.avoidWalls;
        
        //Die daraus resultierenden Kraftvektoren werden hier auf den Velocity Vektor angewendet.
        ApplyForce(sep);
        ApplyForce(ali);
        ApplyForce(coh);
        ApplyForce(wander);
        ApplyForce(avoidWalls);
        Debug.DrawRay(transform.position,velocity.normalized*5);
    }

    //Diese funktion definiert das Separationsverhalten der Boids, das besagt sich voneinander zu entfernen, wenn man zu nahe kommt.
    //Basierend auf Craig Reynolds und Daniel Shiffman 
    Vector2 Separate(List<Boid> boids)
    {
        Vector2 steer = new Vector2(0,0);
        int count = 0;
        
        //Relationen zu allen anderen boids berechnen, die in der richtigen Entfernung sind.
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

    //Nach Daniel Shiffman. Boid schaut in einem Winkel vor ihm, ob sich dort Boids befinden.
    //Diese funktion sorgt dafür, dass Boids, die sich hinter dem Boid befinden nicht in
    //physikberechnungen einbezogen werden.
    //Diese funktion hat die Lebendigkeit der Boid-Simulation drastisch erhöht.
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

    //Nach Craig Reynolds und Daniel Shiffman. Diese regel besagt, dass Boids immer versuchen, sich in die Geleiche blickrichtung zu bewegen wie
    //umliegende boids.
    Vector2 Align(List<Boid> boids) //Calc Average velocity for nearby Boids
    {
        Vector2 sum = new Vector2(0,0);
        int count = 0;
        foreach (var boid in boids)
        {
            float dist = GetBoidDistance(boid);
            if (dist>0 && dist < BoidSettings.instance.neighbourDistance)
            {
                //Nur Boids im Sichtfeld mit einbeziehen.
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

    //Nach Reynolds und Shiffman. Diese Regel lässt Boids zu anderen Boids hinbewegen.
    Vector2 Cohesion(List<Boid> boids)
    {
        Vector2 sum = new Vector2(0,0);
        int count = 0;
        foreach (var boid in boids)
        {
            float dist = GetBoidDistance(boid);
            if (dist > 0 && dist < BoidSettings.instance.neighbourDistance)
            {
                //Nur Boids im Sichtfeld mit einbeziehen.
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

    //Distanz zwischen Position und Position eines anderen Boids berechnen.
    float GetBoidDistance(Boid otherBoid)
    {
        return Vector2.Distance(transform.position, otherBoid.transform.position);
    }

    //Nach einer Idee Shiffman, aber eigene Implementierung. Diese funktion ist nicht zwangsläufig für den von Reynolds dagelegten Flocking algorithmus notwendig.
    //Hier wird ein Wander Movement definiert, dass die Boids zufällig bewegen lässt. Shiffman beschreibt, dass man wenn man nicht direkt die Rotation des Agenten
    //beeinflusst, sondern ihn nur ein anderes ziel zum hinbewegen gibt, die Bewegungen flüssiger werden.
    //Auf dieser Idee basiert diese Implementierung. Es werden in einem Winkel vor dem Boid ein Target mit zufälliger Position gespawned, das vom Boid anvisiert wird.
    //Das beeinträchtigt das Flockingverhalten nicht, sorgt aber für leichte Variationen in den Bewegungen, die diese natürlicher wirken lassen.
    private Vector2 Wander()
    { 
        float spawnAreaAngle = Random.Range(-35,35);
        float spawnDistance = 2;
 
        Vector2 target = new Vector2(); 
        target = ExtensionMethods.Rotate(velocity,spawnAreaAngle) * spawnDistance;
        return SeekTarget(target);
    }
    
    
    //Nach Reynold und Shiffman. Diese funktion wird verwendet, um eine fete Vektorposition anpeilen und sich dort hin bewegen zu können.
    private Vector2 SeekTarget(Vector3 targetPosition) 
    {
        Vector2 desiredVelocity = targetPosition - transform.position;

        float d = desiredVelocity.magnitude;
        desiredVelocity.Normalize();
        desiredVelocity *= BoidSettings.instance.maxSpeed;

        Vector2 steer = desiredVelocity - velocity;
        
        steer = Vector2.ClampMagnitude(steer, BoidSettings.instance.maxForce);

        return steer;
    }

    //Rotiert den Boid auf der Z-Achse.
    //Implementierung: https://answers.unity.com/questions/585035/lookat-2d-equivalent-.html
    private void LookAt2D(Vector2 target)
    {
        float rot_z = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    //Nach Shiffman. Nimmt einen Vector2 und addiert ihn zum Beschleunigungsvektor hinzu.
    private void ApplyForce(Vector2 force)
    {
        _acceleration += force;
    }

    //Nach Shiffman. Lenkt boids von Bildschirmrändern weg.
    //Die Brechnungen der Boidgröße und der Positionen der Bildschirmränder basieren auf diesem Blogeintrag:
    //https://pressstart.vip/tutorials/2018/06/28/41/keep-object-in-bounds.html
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


    //Ändert Farbe und Status, wenn ein Boid angeklickt wird.
    public void SelectBoid()
    {
        _spriteRenderer.color = _activeColor;
        isSelected = true;
    }

    //Änderungen bei Selektion wird rückgängig gemacht.
    public void DeselectBoid()
    {
        _spriteRenderer.color = _normalColor;
        isSelected = false;
    }

    //Initialisierung von Hilfsvariablen.
    private void Start()
    {
        _screenBounds = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _mainCamera.transform.position.z));
        var bounds = _spriteRenderer.bounds;
        _objectWidth = bounds.extents.x;
        _objectHeight = bounds.extents.y;
    }
    
    //Folgende Get Funktionen geben Position, Velocity und Rotationswerte der Boids als INT wert in einem Vordefinierten Bereich zurück.
    //Bei Midi werden alle Parameter auf die Werte 0-127 gemapped.
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

    //Ändert Farbe
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
    
    //Auf basis des ausgewählten Sendeverhaltens werden MIDI oder OSC daten Vorbereitet und an die Handler zum Senden weitergegeben.
    void HandleSendValue()
    {
        bool modeIsOsc = Settings.SendMode == SendMode.OSC;
        int value = modeIsOsc ? sendController.oscHandler.oscValue : sendController.midiHandler.midiValue;
        bool hasChanged = false;
        var osc = sendController.oscHandler;
        int lower = modeIsOsc ? osc.oscLowerValue : 0;
        int upper = modeIsOsc ? osc.oscUpperValue : 127;

        //Je nach im UI Dropdown ausgewähltgen Modus werden hier Position, Rotations oder Geschwindigkeitswerte verarbeitet.
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
    
    //Event für Mausklick. Öffnet Optionsfenster mit Referenz zum geklickten Boid.
    private void OnMouseDown()
    {
        if (!isSelected && !BoidOptions.isActive)
        {
            BoidOptions.sendController = this.sendController;
            UIManager.instance.midiOptionsPanel.SetActive(true);
            SelectBoid();
        }
    }

    //Funktion um Boid von einem Bildschirmrand zum Anderen zu Teleportieren. Wird in der aktuellen Implementierung nicht verwendet.
    //Könnte aber in Zukunft einen Schalter zum Aktivieren bekommen, um die Valuekurven der gesendeten Parameter zu beeinflussen.
    //Vorallem die Kurven von X und Y Positionsdaten wären beeinträchtigt.
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
