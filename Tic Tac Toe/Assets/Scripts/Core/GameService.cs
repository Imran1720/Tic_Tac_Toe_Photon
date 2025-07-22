using Photon.Pun;
using TicTacToe.Audio;
using TicTacToe.Board;
using TicTacToe.Player;
using TicTacToe.UI;
using TicTacToe.Utility;
using TicTacToe.Utility.Events;
using UnityEngine;
namespace TicTacToe.Core
{
    public class GameService : MonoBehaviour
    {
        public static GameService Instance { get; private set; }

        EventService eventService;
        BoardService boardService;
        private SoundService soundService;
        private PlayerService playerService;
        [SerializeField] private UIService uiService;

        [Header("Player Data")]
        [SerializeField] private WinDataSO winDataSO;
        [SerializeField] private PlayerView playerView;

        [Header("Sound Data")]
        [SerializeField] private AudioClipSO audioClipSO;
        [SerializeField] private AudioSource sfxAudioSource;
        [SerializeField] private AudioSource bgmAudioSource;

        [Header("Board Data")]
        [SerializeField] private GridData gridData;
        [SerializeField] private BoardView boardPrefab;


        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            eventService = new EventService();
            soundService = new SoundService(this, audioClipSO);
            boardService = new BoardService(boardPrefab, transform, gridData, eventService);
            playerService = new PlayerService(transform, eventService, playerView, winDataSO);
            uiService.InitilizeService(eventService, this);

            playerService.StartOfflineMode();
        }

        public EventService GetEventService() => eventService;

        public AudioSource GetBGMAudioSource() => bgmAudioSource;
        public AudioSource GetSFXAudioSource() => sfxAudioSource;

        public Vector2 GetTileWorldPosition(int gridX, int gridY)
        {
            return boardService.GetTileWorldPosition(gridX, gridY);
        }

        public PlayerType GetLocalPlayerType() => playerService.GetLocalPlayerType();
        public PlayerType GetCurrentPlayablePlayer() => playerService.GetCurrentPlayablePlayer();

        public void Rematch() => playerService.Rematch();
        private void OnDisable()
        {
            uiService.RemoveEventListeners();
            soundService.RemoveEventListeners();
            playerService.RemoveEventListeners();
        }
    }
}
