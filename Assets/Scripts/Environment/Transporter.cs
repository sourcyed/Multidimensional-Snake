using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporter : MonoBehaviour
{
    public Vector3 teleportDistance;

    private void OnTriggerEnter(Collider other)
    {
        BodyPart bodyPart = other.GetComponent<BodyPart>();
        if (bodyPart)
        {
            if (bodyPart.isHead)
                other.transform.position += teleportDistance;
        }
    }
}
