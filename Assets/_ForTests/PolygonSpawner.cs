using System.Collections.Generic;
using UnityEngine;

namespace countMastersTest
{
    public class PolygonSpawner : MonoBehaviour
    {
        public List<GameObject> objects;
        public float objectRadius;
        public float maxHexagonRadius;

        void Start()
        {
            ArrangeObjectsInCircle(objects, objectRadius, maxHexagonRadius);
        }


        private void ArrangeObjectsInCircle(List<GameObject> objects, float objectRadius, float maxCircleRadius)
        {
            int objectsSpawned = 0;
            int currentRing = 0;

            while (objectsSpawned < objects.Count)
            {
                // Spawn the central object
                if (currentRing == 0)
                {
                    objects[objectsSpawned].transform.position = new Vector3(0, objects[objectsSpawned].transform.position.y, 0);
                    objectsSpawned++;
                    currentRing++;
                    continue;
                }

                float circumference = 2 * Mathf.PI * (currentRing * objectRadius * 2);
                int objectsInRing = Mathf.FloorToInt(circumference / (objectRadius * 2));
                float angleStep = 360f / objectsInRing;

                for (int i = 0; i < objectsInRing && objectsSpawned < objects.Count; i++)
                {
                    float angle = i * angleStep;
                    float x = (currentRing * objectRadius * 2) * Mathf.Cos(angle * Mathf.Deg2Rad);
                    float z = (currentRing * objectRadius * 2) * Mathf.Sin(angle * Mathf.Deg2Rad);
                    Vector3 position = new Vector3(x, objects[objectsSpawned].transform.position.y, z);

                    if (position.magnitude <= maxCircleRadius)
                    {
                        objects[objectsSpawned].transform.position = position;
                        objectsSpawned++;
                    }
                }
                currentRing++;
            }
        }
    }
}
