using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Snake : MonoBehaviour
{
    byte EndMatchEventCode = 1;

    [SerializeField] int startSize = 3;
    [SerializeField] GameObject bodyPartPrefab;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] bool online = true;
    public bool godmode;
    public bool canInteract = true;
    SnakeMovement movement;

    private void Awake()
    {
        online = PhotonNetwork.IsConnectedAndReady;
        movement = GetComponent<SnakeMovement>();
        for (int i = 0; i < startSize; i++)
        {
            GameObject bodyPart = AddBodyPart(Vector3.left * i, true);
            if (i == 0)
            {
                bodyPart.GetComponent<BodyPart>().isHead = true;
            }
        }
        UpdateScore();

        SpotlightFollower spotlightFollower = GameObject.FindObjectOfType<SpotlightFollower>();
        if (spotlightFollower)
        {
            spotlightFollower.target = transform.GetChild(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract)
            CheckInput();
    }

    public void Activate()
    {
        canInteract = true;
        movement.canMove = true;
    }

    public void Deactivate(bool resetDirection = false)
    {
        canInteract = false;
        movement.canMove = false;
        if (resetDirection)
            movement.direction = Vector3.zero;
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (transform.childCount > 3)
            {
                DropBomb();
            }
        }
    }

    public GameObject AddBodyPart(Vector3 pos, bool localSpace=false)
    {
        GameObject bodyPart = Instantiate(bodyPartPrefab, transform.position, Quaternion.identity);
        bodyPart.GetComponent<BodyPart>().snake = this;
        bodyPart.transform.parent = gameObject.transform;
        if (localSpace)
            bodyPart.transform.localPosition = pos;
        else
            bodyPart.transform.position = pos;

        UpdateScore();
        return bodyPart;
    }

    public void DropBomb()
    {
        Bomb bomb;
        if (online)
            bomb = PhotonNetwork.Instantiate(bombPrefab.name, transform.GetChild(transform.childCount - 1).transform.position, Quaternion.identity).GetComponent<Bomb>();
        else
            bomb = Instantiate(bombPrefab, transform.GetChild(transform.childCount - 1).transform.position, Quaternion.identity).GetComponent<Bomb>();
        bomb.Disarm();
        Destroy(transform.GetChild(transform.childCount - 1).gameObject);
        UpdateScore();
    }

    void UpdateScore()
    {
        UpdateScore(transform.childCount);
    }

    void UpdateScore(int score)
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            Hashtable playerProperties = new Hashtable();
            playerProperties["Score"] = score;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        }
    }

    public void Lose()
    {
        UpdateScore(0);
        movement.canMove = false;
        if (PhotonNetwork.IsConnectedAndReady)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
            PhotonNetwork.RaiseEvent(EndMatchEventCode, null, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}
