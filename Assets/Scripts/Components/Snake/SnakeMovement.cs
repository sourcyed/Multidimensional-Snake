using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[DefaultExecutionOrder(1)]
public class SnakeMovement : MonoBehaviour
{
    public int speed;
    public bool canMove = true;

    public Vector3 direction;

    private void Start()
    {
        InvokeRepeating(nameof(MoveBodyParts), 0, 1f / speed);
    }

    private void Update()
    {
        if (canMove)
            CheckInput();
    }

    private void CheckInput()
    {
        Vector3 diff;
        if (transform.childCount <= 1)
        {
            diff = Vector3.zero;
        }
        else
        {
            diff = transform.GetChild(1).position - transform.GetChild(0).position;
        }

        float x = Mathf.Clamp(Mathf.Round(Input.GetAxisRaw("Horizontal")), -1, 1);
        if (x != 0 && diff.x != x)
        {
            direction.x = x;
            direction.y = 0;
        }
        float y = Mathf.Clamp(Mathf.Round(Input.GetAxisRaw("Vertical")), -1, 1);
        if (y != 0 && diff.y != y)
        {
            direction.x = 0;
            direction.y = y;
        }
    }

    private void MoveBodyParts()
    {
        if (canMove)
        {
            if (transform.childCount > 0 && direction != Vector3.zero)
            {
                Vector2 lastPos = transform.position;
                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform bodyPart = transform.GetChild(i);
                    Vector3 tempPos = lastPos;
                    if (i == 0)
                    {
                        lastPos = bodyPart.transform.position;
                        bodyPart.transform.position += direction;
                    }
                    else
                    {
                        lastPos = bodyPart.transform.position;
                        bodyPart.transform.position = tempPos;
                    }
                }
            }
        }
    }
}
