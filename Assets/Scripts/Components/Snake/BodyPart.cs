using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BodyPart : MonoBehaviour
{
    public Snake snake;
    public bool isHead;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            if (snake.transform.GetChild(snake.transform.childCount - 1) == transform)
            {
                snake.AddBodyPart(other.transform.position);
                other.GetComponent<CollectablePart>().Respawn();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!snake.godmode)
        {
            if (other.CompareTag("Wall") || other.CompareTag("BodyPart") || (other.CompareTag("Bomb") && !other.GetComponent<Bomb>().disarmed))
            {
                Lose();
            }
        }
    }

    void Lose()
    {
        if (snake != null)
        {
            snake.Lose();
        }
    }
}
