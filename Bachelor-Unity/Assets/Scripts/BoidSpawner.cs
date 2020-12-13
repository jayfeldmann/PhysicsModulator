using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BoidSpawner : MonoBehaviour
{
   public static BoidSpawner instance;
   public GameObject boidPrefab;
   public int boidCount;
   public float spawnOffset = 1.0f;

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

   private void SpawnBoid(Vector2 spawnPos)
   {
      var boid = Instantiate(boidPrefab, transform);
      boid.transform.position = spawnPos;
      var randomAngle = Random.Range(0, 360);
      boid.transform.Rotate(0,0,randomAngle);
      cachedBoids.Add(boid.GetComponent<Boid>());
   }
}
