using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemConfiguration : MonoBehaviour
{
    private void Awake()
    {
        Application.runInBackground = true;
        Application.targetFrameRate = 30;
    }

    private void Start()
    {
        SceneManager.LoadScene("Loading");
    }
}
