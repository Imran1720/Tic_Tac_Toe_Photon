using Photon.Pun;
using System.Collections.Generic;
using TicTacToe.Core;
using TicTacToe.Utility;
using TicTacToe.Utility.Events;
using UnityEngine;

namespace TicTacToe.Player
{
    public class PlayerView : MonoBehaviourPunCallbacks
    {
        [Header("Objects to be Spawned")]
        [SerializeField] private Transform crossPrefab;
        [SerializeField] private Transform circlePrefab;
        [SerializeField] private Transform linePrefab;

        private EventService eventService;
        private PlayerController playerController;

        private List<Transform> spawnedObjectList;

        private float turnDuration = .5f;
        private float timer;

        public void InitilizeData(EventService eventService, PlayerController controller)
        {
            this.eventService = eventService;
            playerController = controller;
            spawnedObjectList = new List<Transform>();

            AddEventListeners();
            OnNetworkReady(controller);


        }

        public void StartOfflineGame()
        {
            if (PhotonNetwork.OfflineMode)
            {
                playerController.CreateAIController();
                photonView.RPC("RPC_TriggerGameStartRpc", Photon.Pun.RpcTarget.All);
            }
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {

                if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
                {
                    photonView.RPC("RPC_TriggerGameStartRpc", Photon.Pun.RpcTarget.All);
                }
            }

            if (PhotonNetwork.CurrentRoom.PlayerCount > PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                PhotonNetwork.CloseConnection(newPlayer);
            }
        }

        private void OnNetworkReady(PlayerController controller)
        {
            int localPlayerID = PhotonNetwork.LocalPlayer.ActorNumber;
            playerController.OnNetworkSpawn(localPlayerID);

        }

        private void AddEventListeners()
        {
            eventService.OnGameEnded.AddListener(OnGameEnd);
            eventService.OnRematch.AddListener(OnRematchRpc);
            eventService.OnTileClicked.AddListener(OnTileClicked);
            //eventService.OnPlayerTurnChanged.AddListener(UpdatePlayerTurn);
            eventService.OnPlayerClickDetected.AddListener(OnPlayerClickDetectionHandeler);
        }

        public void RemoveEventListeners()
        {
            eventService.OnGameEnded.RemoveListener(OnGameEnd);
            eventService.OnRematch.RemoveListener(OnRematchRpc);
            eventService.OnTileClicked.RemoveListener(OnTileClicked);
            //eventService.OnPlayerTurnChanged.RemoveListener(UpdatePlayerTurn);
            eventService.OnPlayerClickDetected.RemoveListener(OnPlayerClickDetectionHandeler);
        }

        private void OnTileClicked(Vector2Int grid, PlayerType playerType)
        {
            RequestClickSound();
            if (playerController.IsTileSpawnable(grid))
            {
                SpawnObjectRpc(grid.x, grid.y, playerType);
            }
            //photonView.RPC("Rpc_HandleTileClick", RpcTarget.All, grid.x, grid.y, playerType);
            Rpc_HandleTileClick(grid.x, grid.y, playerType);
        }

        public void RequestClickSound()
        {
            eventService.OnRequestTileClickSound.InvokeEvent();
        }

        private void OnPlayerClickDetectionHandeler(Vector2Int grid, PlayerType localPlayer)
        {
            photonView.RPC("RPC_ChangePlayerTurn", RpcTarget.All, localPlayer);
            checkWinRpc();
        }

        private void Update()
        {

            if (PhotonNetwork.OfflineMode)
            {
                timer -= Time.deltaTime;

                if (timer <= 0 && playerController.GetCurrentPlayablePlayer() != PlayerType.NONE && playerController.GetCurrentPlayablePlayer() != playerController.GetLocalPlayerType())
                {

                    playerController.PerformAITurn();
                }
            }
        }

        public void StartAITimer()
        {
            timer = turnDuration;
        }

        // Server RPC's
        [PunRPC]
        private void Rpc_HandleTileClick(int x, int y, PlayerType playerType)
        {
            Vector2Int grid = new Vector2Int(x, y);
            playerController.HandleTileClick(grid, playerType);
        }

        private void checkWinRpc() => playerController.checkWin();

        [PunRPC]
        private void RPC_ChangePlayerTurn(PlayerType playerType)
        {
            playerController.ChangePlayerTurn(playerType);
        }

        public void SpawnAIObject(int x, int y, PlayerType playerType)
        {
            SpawnObjectRpc(x, y, playerType);
        }

        private void SpawnObjectRpc(int x, int y, PlayerType playerType)
        {
            Vector2 spawnPosition = GameService.Instance.GetTileWorldPosition(x, y);
            Transform prefabToSpawn = GetPrefabToSpawn(playerType);

            GameObject spawnedObject = PhotonNetwork.Instantiate("Prefabs/" + prefabToSpawn.name, spawnPosition, Quaternion.identity);

            spawnedObjectList.Add(spawnedObject.transform);
        }

        // Client Host RPC's

        [PunRPC]
        private void RPC_TriggerGameStartRpc()
        {
            //photonView.RPC("RPC_UpdateCurrentPlayer", Photon.Pun.RpcTarget.All, PlayerType.CROSS);
            eventService.OnGameStarted.InvokeEvent(PlayerType.CROSS);
            UpdateCurrentPlayerOverNetwork(PlayerType.CROSS);
        }

        public void GameTie()
        {
            photonView.RPC("RPC_GameTie", Photon.Pun.RpcTarget.All);
        }

        [PunRPC]
        private void RPC_GameTie()
        {
            eventService.OnGameTied.InvokeEvent();
        }


        private Transform GetPrefabToSpawn(PlayerType localPlayer)
        {
            switch (localPlayer)
            {
                case PlayerType.CIRCLE: return circlePrefab;
                case PlayerType.CROSS:
                default: return crossPrefab;
            }
        }

        private void OnGameEnd(WinData data)
        {
            TriggerGameEnd(data);
        }

        private void TriggerGameEnd(WinData data)
        {
            GameObject spawnedLine = PhotonNetwork.Instantiate("Prefabs/" + linePrefab.name, GameService.Instance.GetTileWorldPosition(data.centerGrid.x, data.centerGrid.y), Quaternion.Euler(0, 0, data.Orientation));
            spawnedObjectList.Add(spawnedLine.transform);
            eventService.OnDisableInteraction.InvokeEvent();
        }

        ///private void UpdatePlayerTurn(PlayerType playerType)
        //{
        //    playerController.UpdatePlayablePlayer(playerType);
        //}

        public void GameWon(PlayerType playerType)
        {
            photonView.RPC("RPC_GameWon", Photon.Pun.RpcTarget.All, playerType);
        }

        [PunRPC]
        private void RPC_GameWon(PlayerType playerType)
        {
            eventService.OnGameWon.InvokeEvent(playerType);
        }

        private void OnRematchRpc()
        {
            foreach (Transform obj in spawnedObjectList)
            {
                PhotonNetwork.Destroy(obj.gameObject);
            }
            spawnedObjectList.Clear();

            playerController.OnRematch();
        }

        public void Rematch()
        {
            photonView.RPC("RPC_Rematch", Photon.Pun.RpcTarget.All);
        }

        [PunRPC]
        private void RPC_Rematch()
        {
            eventService.OnRematch.InvokeEvent();
        }


        [PunRPC]
        private void RPC_UpdateCurrentPlayer(PlayerType playerType)
        {
            playerController.UpdatePlayablePlayer(playerType);
            eventService.OnPlayerTurnChanged.InvokeEvent(playerType);

        }

        public void UpdateCurrentPlayerOverNetwork(PlayerType playerType)
        {
            photonView.RPC("RPC_UpdateCurrentPlayer", Photon.Pun.RpcTarget.All, playerType);
            eventService.OnBoardHighlightRequested.InvokeEvent(playerType);
        }

        public void updateNextPlayerOverNetwork(PlayerType playerType)
        {
            photonView.RPC("RPC_UpdateNextPlayer", Photon.Pun.RpcTarget.All, playerType);
        }

        [PunRPC]
        private void RPC_UpdateNextPlayer(PlayerType playerType)
        {
            playerController.UpdateNextPlayerType(playerType);
        }

        public void UpdateScoreOverNetwork(int crossScore, int circleScore)
        {
            photonView.RPC("RPC_UpdateScoreOverNetwork", Photon.Pun.RpcTarget.All, crossScore, circleScore);
        }

        [PunRPC]
        public void RPC_UpdateScoreOverNetwork(int crossScore, int circleScore)
        {
            eventService.OnScoreUpdated.InvokeEvent(crossScore, circleScore);
            playerController.UpdateScore(crossScore, circleScore);
        }
    }
}