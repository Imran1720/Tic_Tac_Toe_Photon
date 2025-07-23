using Photon.Pun;
using TicTacToe.Core;
using TicTacToe.Player;
using TicTacToe.Utility.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace TicTacToe.UI
{
    public class UIService : MonoBehaviourPunCallbacks
    {
        [Header("UI Panels")]
        [SerializeField] private PlayerUI playerUI;
        [SerializeField] private GameOverUI gameOverUI;

        [SerializeField] private GameObject connectingUI;
        [SerializeField] private TextMeshProUGUI lobbyCodeText;

        [SerializeField] private Button closeButton;

        private EventService eventService;
        private void Awake()
        {
            closeButton.onClick.AddListener(CloseGame);
            lobbyCodeText.gameObject.SetActive(false);

            if (!PhotonNetwork.OfflineMode)
            {
                SetLobbyCode();
            }
        }
        public void InitilizeService(EventService eventService, GameService gameService)
        {
            this.eventService = eventService;
            playerUI.InitializeServices(eventService);
            gameOverUI.InitializeServices(eventService, gameService);

            eventService.OnGameStarted.AddListener(OnGameStarted);
        }

        public void RemoveEventListeners()
        {
            playerUI.RemoveEventListeners();
            gameOverUI.RemoveEventListeners();
            eventService.OnGameStarted.RemoveListener(OnGameStarted);
        }

        private void SetLobbyCode()
        {
            string lobbyCode = PlayerPrefs.GetString("LobbyCode");
            photonView.RPC("RPC_LobbyCodeCreated", RpcTarget.MasterClient, lobbyCode);
        }

        [PunRPC]
        private void RPC_LobbyCodeCreated(string code)
        {
            lobbyCodeText.text = code;
            lobbyCodeText.gameObject.SetActive(true);
        }

        private void OnGameStarted(PlayerType playerType)
        {
            photonView.RPC("RPC_GameStarted", RpcTarget.All);
        }

        [PunRPC]
        private void RPC_GameStarted()
        {
            connectingUI.SetActive(false);
        }

        private void CloseGame()
        {
            photonView.RPC("RPC_LeaveRoom", RpcTarget.All);
        }
        [PunRPC]
        private void RPC_LeaveRoom()
        {
            PhotonNetwork.LoadLevel("Lobby");
        }
    }
}
