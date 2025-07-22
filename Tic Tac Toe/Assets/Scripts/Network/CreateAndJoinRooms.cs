using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomCode;
    [SerializeField] private TMP_InputField joinCode;
    [SerializeField] private TextMeshProUGUI connectingMessage;

    [SerializeField] private Button createRoom;
    [SerializeField] private Button joinRoom;

    [SerializeField] private Button MultiplayerButton;
    [SerializeField] private Button soloButton;
    [SerializeField] private Button closeLobbyButton;

    [SerializeField] private GameObject connectingScreen;

    private bool isConnectedAndReady = false;

    private void Start()
    {
        createRoom.interactable = false;
        joinRoom.interactable = false;

        createRoom.onClick.AddListener(CreateRoom);
        joinRoom.onClick.AddListener(JoinRoom);
        MultiplayerButton.onClick.AddListener(ConnectToNetwork);
        soloButton.onClick.AddListener(StartSolo);
        closeLobbyButton.onClick.AddListener(DisconnectFromServer);
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
        PhotonNetwork.LoadLevel("Tic_Tac_Toe");
    }



    private void DisconnectFromServer()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
            connectingMessage.text = "Disconnecting from network.";
            connectingScreen.SetActive(true);
        }
    }
    public override void OnLeftLobby()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        connectingScreen.SetActive(false);
    }

    private void StartSolo()
    {
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.JoinRoom(null);
        PhotonNetwork.LoadLevel("Tic_Tac_Toe_Solo");
    }

}
