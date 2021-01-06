
public class BoidPresetSaveData
{
    public float maxSpeed;
    public float maxForce;
    public float desiredSeparation;
    public float neighbourDistance;

    public float separation;
    public float alignment;
    public float cohesion;
    public float wander;
    public float avoidWalls;

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
