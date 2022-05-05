using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCleaner : MonoBehaviour
{
    public void CleanAllBombs()
    {
        foreach (GameObject bomb in GameObject.FindGameObjectsWithTag("Bomb"))
        {
            Destroy(bomb);
        }
    }
}
