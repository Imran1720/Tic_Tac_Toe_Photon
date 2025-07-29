using Photon.Pun;
using System.Collections;
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

        [SerializeField] private GameObject settingScreen;
        [SerializeField] private Button settingButton;

        [SerializeField] private GameObject playerLeftScreen;
        [SerializeField] private TextMeshProUGUI playerLeftScreenMessage;

        [SerializeField] private Button closeButton;

        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider sfxSlider;

        private EventService eventService;
        private bool isPlayerLeaving = false;

        private void Awake()
        {
            closeButton.onClick.AddListener(CloseGame);
            settingButton.onClick.AddListener(ToggleSettingScreen);
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
            bgmSlider.onValueChanged.AddListener(OnBGMVoulmeChanged);
            sfxSlider.onValueChanged.AddListener(OnSFXVoulmeChanged);

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
            eventService.OnButtonClickRequested.InvokeEvent();
            isPlayerLeaving = true;

            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
        }

        public override void OnLeftRoom()
        {
            isPlayerLeaving = false;
            playerLeftScreen.SetActive(false);
            PhotonNetwork.LoadLevel("Lobby");
        }

        private void ToggleSettingScreen()
        {
            eventService.OnButtonClickRequested.InvokeEvent();
            if (settingScreen.activeSelf)
            {
                settingScreen.SetActive(false);
            }
            else
            {
                settingScreen.SetActive(true);
            }
        }

        private void OnSFXVoulmeChanged(float value)
        {
            eventService.OnSFXVolumeChanged.InvokeEvent(value);
        }

        private void OnBGMVoulmeChanged(float value)
        {
            eventService.OnBGMVolumeChanged.InvokeEvent(value);
        }

        public void SetAudioSliderValues(float bgmVol, float sfxVol)
        {
            bgmSlider.value = bgmVol;
            sfxSlider.value = sfxVol;
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            if (!isPlayerLeaving)
            {
                StartCoroutine(LeaveRoomAfter(2f));
            }
        }

        IEnumerator LeaveRoomAfter(float delay)
        {
            playerLeftScreen.SetActive(true);
            yield return new WaitForSeconds(delay);

            playerLeftScreenMessage.text = "Returning to lobby.";

            yield return new WaitForSeconds(delay);
            PhotonNetwork.LeaveRoom();
        }
    }
}
