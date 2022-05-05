using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] int spawnCount = 3;
    [SerializeField] GameObject bombPrefab;

    [SerializeField] int minX;
    [SerializeField] int maxX;
    [SerializeField] int minY;
    [SerializeField] int maxY;

    public void SpawnBombs()
    {
        GameObject[] bodyParts = GameObject.FindGameObjectsWithTag("BodyPart");

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject bomb = Instantiate(bombPrefab, transform);
            Vector3 randomPosition;
            do
            {
                randomPosition = new Vector3(Random.Range(minX, maxX + 1), Random.Range(minY, maxY + 1), 0);
                bomb.transform.position = randomPosition;
            } while (bodyParts.Select(x => x.transform.position).ToList().Contains(randomPosition));
        }
    }
}
