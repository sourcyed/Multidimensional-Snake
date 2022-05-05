using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[DefaultExecutionOrder(10)]
public class Tutorial : MonoBehaviour
{
    [SerializeField] string[] tutorialStrings;
    [SerializeField] TextMeshProUGUI tutorialText;
    [SerializeField] Animation tutorialAnimation;
    [SerializeField] Snake snake;
    [SerializeField] GameObject collectable;
    [SerializeField] BombSpawner bombSpawner;

    int currentTutorialStage;

    private void Start()
    {
        tutorialText.text = tutorialStrings[0];
    }

    private void Update()
    {
        if (currentTutorialStage == 1)
        {
            if (Input.GetButtonDown("Submit"))
            {
                LoadNextTutorialStage();
            }
        }
    }

    public void GetPart()
    {
        if (currentTutorialStage != 1)
        {
            LoadNextTutorialStage();
        }
    }

    public void LoadNextTutorialStage()
    {
        currentTutorialStage++;
        if (currentTutorialStage < tutorialStrings.Length)
        {
            tutorialText.text = tutorialStrings[currentTutorialStage];
            tutorialAnimation.Play();
            snake.Deactivate(true);
            Invoke(nameof(ActivateSnake), 3);
        }
        else
        {
            SceneManager.LoadScene("Loading");
        }

        // Stage specific cases
        if (currentTutorialStage == 1)
        {
            collectable.SetActive(false);
            snake.canInteract = true;
        }
        else
        {
            collectable.SetActive(true);
        }
        if (currentTutorialStage == 4)
        {
            bombSpawner.SpawnBombs();
        }
    }

    void ActivateSnake()
    {
        snake.Activate();
    }
}
