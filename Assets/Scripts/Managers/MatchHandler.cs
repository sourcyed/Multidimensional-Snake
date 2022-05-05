using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class MatchHandler : MonoBehaviourPunCallbacks
{
    byte EndMatchEventCode = 1;

    [SerializeField] UnityEvent PrepareMatchEvent;
    [SerializeField] UnityEvent StartMatchEvent;
    [SerializeField] UnityEvent EndMatchEvent;
    [SerializeField] UnityEvent WinEvent;
    [SerializeField] UnityEvent DrawEvent;
    [SerializeField] UnityEvent LoseEvent;

    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] TextMeshProUGUI roundText;
    [SerializeField] TextMeshProUGUI playerScoreText;
    [SerializeField] TextMeshProUGUI opponentScoreText;

    int round = 1;
    int playerScore = 0;
    int opponentScore = 0;

    bool gameFinished;

    private void Start()
    {
        PrepareMatch();
    }

    void Update()
    {
        if (gameFinished)
        {
            if (Input.GetButtonDown("Submit"))
            {
                PhotonNetwork.Disconnect();
            }
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!gameFinished)
        {
            if (focus == false)
            {
                PhotonNetwork.Disconnect();
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("Loading");
    }

    public void PrepareMatch()
    {
        PrepareMatchEvent?.Invoke();
        roundText.text = $"Round {round}/3";
        StartCoroutine(CountdownTimer(3));
    }

    public void StartMatch()
    {
        StartMatchEvent?.Invoke();
    }

    public void EndMatch()
    {
        if (gameFinished == false)
        {
            EndMatchEvent?.Invoke();
            UpdateScores();
            round++;
            if (round <= 3)
            {
                PrepareMatch();
            }
            else
            {
                if (playerScore > opponentScore)
                {
                    WinEvent?.Invoke();
                }
                else if (playerScore == opponentScore)
                {
                    DrawEvent?.Invoke();
                }
                else
                {
                    LoseEvent?.Invoke();
                }
                gameFinished = true;
            }
        }
    }

    private void UpdateScores()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            int score = (int)PhotonNetwork.PlayerList[i].CustomProperties["Score"];
            if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
            {
                playerScore += score;
            }
            else
            {
                opponentScore += score;
            }
        }
        playerScoreText.text = playerScore.ToString();
        opponentScoreText.text = opponentScore.ToString();
    }

    IEnumerator CountdownTimer(int countdownTime)
    {
        countdownText.text = countdownTime.ToString();
        for (int i = 1; i <= countdownTime; i++)
        {
            yield return new WaitForSeconds(1);
            countdownText.text = (countdownTime - i).ToString();
        }
        countdownText.text = "";
        StartMatch();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == EndMatchEventCode)
        {
            EndMatch();
        }
    }
}