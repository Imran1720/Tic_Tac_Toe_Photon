using Photon.Pun;
using TicTacToe.AI;
using TicTacToe.Utility;
using TicTacToe.Utility.Events;
using UnityEngine;

namespace TicTacToe.Player
{
    public class PlayerController
    {
        private WinDataSO winDataSO;
        private PlayerView playerView;
        private EventService eventService;

        private PlayerType localPlayerType;
        private PlayerType nextPlayer;
        private PlayerType[,] playerTypeGrid;
        private PlayerType currentPlayablePlayerType;

        private int crossScore;
        private int circleScore;
        private TicTacTieAIController ticTacTieAI;

        private bool gameTied = false;

        public PlayerController(EventService eventService, WinDataSO winDataSO, PlayerView playerView)
        {
            this.winDataSO = winDataSO;
            this.playerView = playerView;
            this.eventService = eventService;

            playerTypeGrid = new PlayerType[3, 3];

        }

        public void CreateAIController() => ticTacTieAI = new TicTacTieAIController(playerTypeGrid, .5f, winDataSO);

        public void OnNetworkSpawn(int localClientId)
        {
            localPlayerType = (localClientId == 1) ? PlayerType.CROSS : PlayerType.CIRCLE;
        }

        public void HandleTileClick(Vector2Int grid, PlayerType playerType)
        {
            if (playerType != currentPlayablePlayerType)
            {
                return;
            }

            if (playerTypeGrid[grid.x, grid.y] != PlayerType.NONE)
            {
                return;
            }

            playerTypeGrid[grid.x, grid.y] = playerType;

            eventService.OnPlayerClickDetected.InvokeEvent(grid, playerType);

            if (currentPlayablePlayerType != localPlayerType && PhotonNetwork.OfflineMode)
            {
                playerView.StartAITimer();
            }
        }

        public void PerformAITurn()
        {
            if (!IsGameOver())
            {
                Vector2Int aiMove = ticTacTieAI.GetMove();

                playerTypeGrid[aiMove.x, aiMove.y] = PlayerType.CIRCLE;

                playerView.SpawnAIObject(aiMove.x, aiMove.y, PlayerType.CIRCLE);
                ChangePlayerTurn(PlayerType.CIRCLE);
                checkWin();
            }
        }

        public void checkWin()
        {
            foreach (WinData data in winDataSO.winDataList)
            {
                if (CheckWinForGridPair(data))
                {
                    HandleWin(data);
                    return;
                }
            }

            if (IsTie())
            {
                UpdateNextPlayer(currentPlayablePlayerType);
                playerView.GameTie();
            }
        }

        public bool IsTileSpawnable(Vector2Int grid)
        {
            if (playerTypeGrid[grid.x, grid.y] != PlayerType.NONE || (currentPlayablePlayerType != localPlayerType))
            {
                return false;
            }
            return true;
        }

        public void ChangePlayerTurn(PlayerType playerType)
        {
            switch (playerType)
            {
                case PlayerType.CROSS:
                    SetCurrentPlayablePlayer(PlayerType.CIRCLE);
                    break;
                case PlayerType.CIRCLE:
                    SetCurrentPlayablePlayer(PlayerType.CROSS);
                    break;
            }
        }

        public void UpdatePlayablePlayer(PlayerType playerType)
        {
            currentPlayablePlayerType = playerType;
        }

        public void OnRematch()
        {
            gameTied = false;
            ResetGrid();
            if (PhotonNetwork.OfflineMode)
            {
                ticTacTieAI.InitializePriorityGrid();
            }
            SetCurrentPlayablePlayer(nextPlayer);
        }

        public void UpdateNextPlayerType(PlayerType playerType)
        {
            nextPlayer = playerType;
        }

        public void UpdateScore(int crossScore, int circleScore)
        {
            this.crossScore = crossScore;
            this.circleScore = circleScore;
        }

        //Getters
        public PlayerType GetLocalPlayerType() => localPlayerType;
        public PlayerType GetCurrentPlayablePlayer() => currentPlayablePlayerType;

        //Helper Functions

        private void HandleWin(WinData data)
        {
            UpdateNextPlayer(currentPlayablePlayerType);
            SetCurrentPlayablePlayer(PlayerType.NONE);
            PlayerType currentGridPlayerType = playerTypeGrid[data.centerGrid.x, data.centerGrid.y];

            if (currentGridPlayerType == PlayerType.CROSS)
            {
                crossScore++;
            }
            else
            {
                circleScore++;
            }
            UpdateScoreAccrossNetwork();

            playerView.GameWon(currentGridPlayerType);
            eventService.OnGameEnded.InvokeEvent(data);
        }

        private bool IsAWinPair(PlayerType aPlayer, PlayerType bPlayer, PlayerType cPlayer)
        {
            return aPlayer != PlayerType.NONE && aPlayer == bPlayer && bPlayer == cPlayer;
        }

        private bool CheckWinForGridPair(WinData data)
        {
            PlayerType firstGridPlayerType = playerTypeGrid[data.winGridPair[0].x, data.winGridPair[0].y];
            PlayerType secondGridPlayerType = playerTypeGrid[data.winGridPair[1].x, data.winGridPair[1].y];
            PlayerType thirdGridPlayerType = playerTypeGrid[data.winGridPair[2].x, data.winGridPair[2].y];

            return IsAWinPair(firstGridPlayerType, secondGridPlayerType, thirdGridPlayerType);
        }

        private bool IsTie()
        {
            for (int i = 0; i < playerTypeGrid.GetLength(0); i++)
            {
                for (int j = 0; j < playerTypeGrid.GetLength(1); j++)
                {
                    if (playerTypeGrid[i, j] == PlayerType.NONE)
                    {
                        return false;
                    }
                }
            }

            gameTied = true;
            return true;
        }

        public bool IsGameOver()
        {
            if (gameTied || currentPlayablePlayerType == PlayerType.NONE)
            {
                return true;
            }
            return false;
        }

        private void ResetGrid()
        {
            for (int i = 0; i < playerTypeGrid.GetLength(0); i++)
            {
                for (int j = 0; j < playerTypeGrid.GetLength(1); j++)
                {
                    playerTypeGrid[i, j] = PlayerType.NONE;
                }
            }
        }

        private void SetCurrentPlayablePlayer(PlayerType playerType)
        {
            playerView.UpdateCurrentPlayerOverNetwork(playerType);
        }

        private void UpdateNextPlayer(PlayerType playerType)
        {
            playerView.updateNextPlayerOverNetwork(playerType);
        }

        private void UpdateScoreAccrossNetwork()
        {
            playerView.UpdateScoreOverNetwork(crossScore, circleScore);
        }
    }
}
