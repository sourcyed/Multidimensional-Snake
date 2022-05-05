using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] Material disarmedMaterial;
    [SerializeField] GameObject skull;
    [SerializeField] GameObject flag;
    public bool disarmed = false;

    public void Disarm()
    {
        GetComponent<MeshRenderer>().material = disarmedMaterial;
        skull.SetActive(false);
        flag.SetActive(true);
        disarmed = true;
    }
}