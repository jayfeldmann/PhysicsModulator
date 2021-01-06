using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoidSettings : MonoBehaviour
{
    public float maxSpeed = 15f;
    public float maxForce = 0.2f;
    public float arriveRadius = 20;
    public float desiredSeparation = 1f;
    public float neighbourDistance = 4f;

    public float separation = 1.3f;
    public float alignment = 1f;
    public float cohesion = 1.1f;
    public float wander = 0.3f;
    public float avoidWalls = 5;

    public static BoidSettings instance;
    
    public Slider maxSpeedSlider;
    public Slider maxForceSlider;
    public Slider arriveRadiusSlider;
    public Slider desiredSeperationSlider;
    public Slider neighbourDistanceSlider;
    
    public Slider separationSlider;
    public Slider alignmentSlider;
    public Slider cohesionSlider;
    public Slider wanderSlider;
    public Slider avoidWallsSlider;

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
    public void UpdateArriveRadius()
    {
        arriveRadius = arriveRadiusSlider.value;
    }public void UpdateDesiredSeparation()
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


    public void UpdateSettings(BoidPresetSaveData loadedSave)
    {
        maxSpeed = loadedSave.maxSpeed;
        
        maxForce = loadedSave.maxForce;
        arriveRadius = loadedSave.arriveRadius;
        desiredSeparation = loadedSave.desiredSeparation;
        neighbourDistance = loadedSave.neighbourDistance;
        
        //Weights
        separation = loadedSave.separation;
        alignment = loadedSave.alignment;
        cohesion = loadedSave.cohesion;
        wander = loadedSave.wander;
        avoidWalls = loadedSave.avoidWalls;
    }

    public void LoadSliders()
    {
        maxSpeedSlider.value = maxSpeed;
        maxForceSlider.value = maxForce;
        arriveRadiusSlider.value = arriveRadius;
        desiredSeperationSlider.value = desiredSeparation;
        neighbourDistanceSlider.value = neighbourDistance;

        separationSlider.value = separation;
        alignmentSlider.value = alignment;
        cohesionSlider.value = cohesion;
        wanderSlider.value = wander;
        avoidWallsSlider.value = avoidWalls;
    }

}
