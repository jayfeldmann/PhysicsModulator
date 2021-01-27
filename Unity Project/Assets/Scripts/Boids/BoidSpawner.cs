using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

/// <summary>
/// Handles Initialisation of Boid Objects
/// </summary>
public class BoidSpawner : MonoBehaviour
{
   public static BoidSpawner instance;
   //Referenz zum Boid Prefab
   public GameObject boidPrefab;
   // Nummer der Boids in der Simulation
   public int boidCount;
   public float spawnOffset = 1.0f;

   // Hilfsvariablen zur Festlegung der Bildschirmränder
   public Camera mainCamera;
   public Vector2 screenBounds;

   public List<Boid> cachedBoids;

   private void Awake()
   {
      if (!instance)
      {
         instance = this;
      }
   }

   // Start legt den Spawnbereich fest, damit Boid Zufällig aber innerhalb des Felds initialisiert werden.
   private void Start()
   {
      screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

      for (int i = 0; i < boidCount; i++)
      {
         Vector2 randomSpawnPos = new Vector2();

         randomSpawnPos.x = Random.Range(screenBounds.x-spawnOffset, screenBounds.x * -1 +spawnOffset);
         randomSpawnPos.y = Random.Range(screenBounds.y-spawnOffset, screenBounds.y * -1 +spawnOffset);
         SpawnBoid(randomSpawnPos);
      }
   }

   // Initialisiert ein neues Boid Objekt
   private void SpawnBoid(Vector2 spawnPos)
   {
      var boid = Instantiate(boidPrefab, transform);
      boid.transform.position = spawnPos;
      var randomAngle = Random.Range(0, 360);
      boid.transform.Rotate(0,0,randomAngle);
      cachedBoids.Add(boid.GetComponent<Boid>());
   }
}
