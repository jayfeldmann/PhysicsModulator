/// <summary>
/// Definiert die Presetdatei für BoidPresets.
/// </summary>
public class BoidPresetSaveData
{
    //Simulationsparameter
    public float maxSpeed;
    public float maxForce;
    public float desiredSeparation;
    public float neighbourDistance;

    //Gewichtungen
    public float separation;
    public float alignment;
    public float cohesion;
    public float wander;
    public float avoidWalls;

    //Konstruktor zwingt ein Zuweisen von Werten um keine Null-Werte zu haben.
    public BoidPresetSaveData()
    {
        maxSpeed = BoidSettings.instance.maxSpeed;
        maxForce = BoidSettings.instance.maxForce;
        desiredSeparation = BoidSettings.instance.desiredSeparation;
        neighbourDistance = BoidSettings.instance.neighbourDistance;
        
        separation = BoidSettings.instance.separation;
        alignment = BoidSettings.instance.alignment;
        cohesion = BoidSettings.instance.cohesion;
        wander = BoidSettings.instance.wander;
        avoidWalls = BoidSettings.instance.avoidWalls;
    }
}
