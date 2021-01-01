using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class BoidSettings : MonoBehaviour
{
    public static float maxSpeed = 15f;
    public static float maxForce = 0.2f;
    public static float arriveRadius = 20;
    public static float desiredSeparation = 1f;
    public static float neighbourDistance = 2f;

    public static float separation = 1.3f;
    public static float alignment = 1f;
    public static float cohesion = 1.1f;
    public static float wander = 0.3f;
    public static float avoidWalls = 1.2f;

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

    private void OnEnable()
    {
        LoadSliders();
    }

    public void UpdateSettings()
    {
        // Settings
        maxSpeed = maxSpeedSlider.value;
        maxForce = maxForceSlider.value;
        arriveRadius = arriveRadiusSlider.value;
        desiredSeparation = desiredSeperationSlider.value;
        neighbourDistance = neighbourDistanceSlider.value;
        
        //Weights
        separation = separationSlider.value;
        alignment = alignmentSlider.value;
        cohesion = cohesionSlider.value;
        wander = wanderSlider.value;
        avoidWalls = avoidWallsSlider.value;
    }

    public static void UpdateSettings(BoidPresetSaveData loadedSave)
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
