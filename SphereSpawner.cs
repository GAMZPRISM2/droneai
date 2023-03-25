using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    public int numberOfSpheres = 1;
    public float spawnRadius = 3f;
    public float maxHeight = 120f;
    public float minHeight = 5f;
    public BoxCollider spawnArea;

    void Start()
    {
        for (int i = 0; i < numberOfSpheres; i++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                Random.Range(minHeight, maxHeight),
                Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
            );

            if (Physics.CheckSphere(randomPos, spawnRadius))
            {
                i--;
                continue;
            }

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = randomPos;
            sphere.transform.localScale = new Vector3(spawnRadius * 2, spawnRadius * 2, spawnRadius * 2);
        }
    }
}
