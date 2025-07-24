using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomCode;
    [SerializeField] private TMP_InputField joinCode;
    [SerializeField] private TextMeshProUGUI connectingMessage;

    [SerializeField] private Button joinRoom;
    [SerializeField] private Button createRoom;

    [SerializeField] private Button MultiplayerButton;
    [SerializeField] private Button closeMultiplayerButton;

    [SerializeField] private Button easyModeButton;
    [SerializeField] private Button mediumModeButton;

    [SerializeField] private GameObject connectingScreen;

    [SerializeField] private GameObject lobbyScreen;
    [SerializeField] private GameObject MultiplayerScreen;

    [SerializeField] private GameObject passwordWaringBoard;
    [SerializeField] private GameObject roomFullWaringBoard;

    private bool isConnectedAndReady = false;

    private bool isStartSoloModeRequested = false;

    private void Start()
    {
        createRoom.interactable = false;
        joinRoom.interactable = false;

        joinRoom.onClick.AddListener(JoinRoom);
        createRoom.onClick.AddListener(CreateRoom);
        MultiplayerButton.onClick.AddListener(ConnectToNetwork);
        easyModeButton.onClick.AddListener(StartEasyMode);
        mediumModeButton.onClick.AddListener(StartMediumMode);
        closeMultiplayerButton.onClick.AddListener(DisconnectFromServer);
        roomCode.onSelect.AddListener(HidePasswordWarning);
        joinCode.onSelect.AddListener(HideRoomWarning);

        if (PhotonNetwork.IsConnected && !PhotonNetwork.OfflineMode)
        {
            OpenMultiplayerScreen();
        }
        else
        {
            lobbyScreen.SetActive(true);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        roomFullWaringBoard.SetActive(true);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        passwordWaringBoard.SetActive(true);
    }
    private void OpenMultiplayerScreen()
    {
        MultiplayerScreen.SetActive(true);
        lobbyScreen.SetActive(false);
    }


    private void ConnectToNetwork()
    {
        PhotonNetwork.OfflineMode = false;
        PhotonNetwork.ConnectUsingSettings();

        connectingMessage.text = "Connecting to network.";
        connectingScreen.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        if (!PhotonNetwork.OfflineMode)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        isConnectedAndReady = true;
        createRoom.interactable = true;
        joinRoom.interactable = true;

        connectingScreen.SetActive(false);
    }

    private void CreateRoom()
    {
        if (!isConnectedAndReady)
        {
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        PlayerPrefs.SetString("LobbyCode", roomCode.text);

        PhotonNetwork.CreateRoom(roomCode.text, roomOptions);
    }

    private void JoinRoom()
    {
        if (!isConnectedAndReady)
        {
            return;
        }
        PhotonNetwork.JoinRoom(joinCode.text);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.OfflineMode)
        {
            PhotonNetwork.LoadLevel("Tic_Tac_Toe_Solo");
            isStartSoloModeRequested = false;
        }
        else
        {
            PhotonNetwork.LoadLevel("Tic_Tac_Toe");
        }
    }

    private void DisconnectFromServer()
    {
        PhotonNetwork.Disconnect();
    }



    public override void OnDisconnected(DisconnectCause cause)
    {
        MultiplayerScreen.SetActive(false);

        if (isStartSoloModeRequested)
        {
            PhotonNetwork.OfflineMode = true;
            PhotonNetwork.JoinRoom("offlineRoom");
        }
    }

    private void StartEasyMode()
    {
        StartSoloWithDifficulty(1);
    }

    private void StartMediumMode()
    {
        StartSoloWithDifficulty(2);
    }

    private void StartSoloWithDifficulty(int difficulty)
    {
        PlayerPrefs.SetInt("levelDifficaulty", difficulty);
        if (PhotonNetwork.IsConnected)
        {
            isStartSoloModeRequested = true;
            PhotonNetwork.Disconnect();
            connectingMessage.text = "Starting Solo mode...";
            connectingScreen.SetActive(true);
        }
        else
        {
            PhotonNetwork.OfflineMode = true;
            PhotonNetwork.JoinRoom("offlineRoom");
        }
    }

    private void HidePasswordWarning(string str)
    {
        passwordWaringBoard.SetActive(false);
    }

    private void HideRoomWarning(string str)
    {
        roomFullWaringBoard.SetActive(false);
    }
}
