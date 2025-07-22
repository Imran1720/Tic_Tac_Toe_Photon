using TicTacToe.Player;
using UnityEngine;

namespace TicTacToe.Utility.Events
{

    public class EventService
    {
        public EventController OnRematch;
        public EventController OnGameTied;
        public EventController OnDisableInteraction;
        public EventController OnRequestTileClickSound;

        public EventController<WinData> OnGameEnded;

        public EventController<int, int> OnScoreUpdated;

        public EventController<string> OnLobbyCodeCreated;

        public EventController<PlayerType> OnGameWon;
        public EventController<PlayerType> OnGameStarted;
        public EventController<PlayerType> OnPlayerTurnChanged;

        public EventController<Vector2Int, PlayerType> OnTileClicked;
        public EventController<Vector2Int, PlayerType> OnPlayerClickDetected;

        public EventService()
        {
            OnRematch = new EventController();
            OnGameTied = new EventController();
            OnDisableInteraction = new EventController();
            OnRequestTileClickSound = new EventController();

            OnGameEnded = new EventController<WinData>();

            OnScoreUpdated = new EventController<int, int>();

            OnLobbyCodeCreated = new EventController<string>();

            OnGameWon = new EventController<PlayerType>();
            OnGameStarted = new EventController<PlayerType>();
            OnPlayerTurnChanged = new EventController<PlayerType>();

            OnTileClicked = new EventController<Vector2Int, PlayerType>();
            OnPlayerClickDetected = new EventController<Vector2Int, PlayerType>();
        }
    }
}
