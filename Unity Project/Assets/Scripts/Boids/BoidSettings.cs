using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// BoidSettings definieren die Globalen eigenschaften der Boidsimulation wie Geschwindigkeit, Distanz zu Nachbarn oder die Geichtungen der Verhaltensregeln.
/// </summary>
public class BoidSettings : MonoBehaviour
{
    //Simulationsparameter
    public float maxSpeed = 15f;
    public float maxForce = 0.2f;
    public float desiredSeparation = 1f;
    public float neighbourDistance = 4f;

    //Gewichtungen der Regeln
    public float separation = 1.3f;
    public float alignment = 1f;
    public float cohesion = 1.1f;
    public float wander = 0.3f;
    public float avoidWalls = 5;

    
    //UI Referenzen
    public static BoidSettings instance;
    
    public Slider maxSpeedSlider;
    public Slider maxForceSlider;
    public Slider desiredSeperationSlider;
    public Slider neighbourDistanceSlider;
    
    public Slider separationSlider;
    public Slider alignmentSlider;
    public Slider cohesionSlider;
    public Slider wanderSlider;
    public Slider avoidWallsSlider;
    
    //Lädt Settings anhand eines Objekts genriert durch JSON Daten,.
    public void UpdateSettings(BoidPresetSaveData loadedSave)
    {
        maxSpeed = loadedSave.maxSpeed;
        
        maxForce = loadedSave.maxForce;
        desiredSeparation = loadedSave.desiredSeparation;
        neighbourDistance = loadedSave.neighbourDistance;
        
        //Weights
        separation = loadedSave.separation;
        alignment = loadedSave.alignment;
        cohesion = loadedSave.cohesion;
        wander = loadedSave.wander;
        avoidWalls = loadedSave.avoidWalls;
    }
    
    //Alle funktionen dienen nur zur Integration in das UI
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }
    public void UpdateMaxSpeed()
    {
        maxSpeed = maxSpeedSlider.value;
    }
    public void UpdateMaxForce()
    {
        maxForce = maxForceSlider.value;
    } 
    public void UpdateDesiredSeparation()
    {
        desiredSeparation = desiredSeperationSlider.value;
    }
    public void UpdateNeighbourDistance()
    {
        neighbourDistance = neighbourDistanceSlider.value;
    }
    public void UpdateSeparation()
    {
        separation = separationSlider.value;
    }
    
    public void UpdateAlignment()
    {
        alignment = alignmentSlider.value;
    }
    public void UpdateCohesion()
    {
        cohesion = cohesionSlider.value;
    }
    public void UpdateWander()
    {
        wander = wanderSlider.value;
    }
    public void UpdateAvoidWalls()
    {
        avoidWalls = avoidWallsSlider.value;
    }



    public void LoadSliders()
    {
        maxSpeedSlider.value = maxSpeed;
        maxForceSlider.value = maxForce;
        desiredSeperationSlider.value = desiredSeparation;
        neighbourDistanceSlider.value = neighbourDistance;

        separationSlider.value = separation;
        alignmentSlider.value = alignment;
        cohesionSlider.value = cohesion;
        wanderSlider.value = wander;
        avoidWallsSlider.value = avoidWalls;
    }

}
