using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightFollower : MonoBehaviour
{
    Quaternion initialRotation;
    public Transform target;
    public bool following;

    private void Start()
    {
        initialRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        if (target != null && following)
        {
            transform.LookAt(target);
        }
    }

    public void StartFollowing()
    {
        following = true;
    }

    public void StopFollowing()
    {
        following = false;
        transform.rotation = initialRotation;
    }
}
