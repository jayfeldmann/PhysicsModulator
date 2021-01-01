
public class BoidPresetSaveData
{
    public float maxSpeed;
    public float maxForce;
    public float arriveRadius;
    public float desiredSeparation;
    public float neighbourDistance;

    public float separation;
    public float alignment;
    public float cohesion;
    public float wander;
    public float avoidWalls;

    public BoidPresetSaveData()
    {
        maxSpeed = BoidSettings.maxSpeed;
        maxForce = BoidSettings.maxForce;
        arriveRadius = BoidSettings.arriveRadius;
        desiredSeparation = BoidSettings.desiredSeparation;
        neighbourDistance = BoidSettings.neighbourDistance;
        
        separation = BoidSettings.separation;
        alignment = BoidSettings.alignment;
        cohesion = BoidSettings.cohesion;
        wander = BoidSettings.wander;
        avoidWalls = BoidSettings.avoidWalls;
    }
}
