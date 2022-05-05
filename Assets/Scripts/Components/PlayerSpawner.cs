using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    [SerializeField] int minX;
    [SerializeField] int maxX;
    [SerializeField] int minY;
    [SerializeField] int maxY;

    [SerializeField] bool deactivateOnStart;
    [SerializeField] bool spawnOnStart;

    [HideInInspector] public Snake player;

    private void Start()
    {
        if (spawnOnStart)
            RenewPlayer();
    }

    public void RenewPlayer()
    {
        DestroyPlayer();
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX + 1), Random.Range(minY, maxY + 1));
        player = Instantiate(playerPrefab, randomPosition, Quaternion.identity).GetComponent<Snake>();
        if (deactivateOnStart)
            player.Deactivate();
    }

    public void ActivatePlayer()
    {
        if (player != null)
        {
            player.Activate();
        }
    }

    public void DestroyPlayer()
    {
        if (player != null)
        {
            Destroy(player.gameObject);
        }
    }
}
