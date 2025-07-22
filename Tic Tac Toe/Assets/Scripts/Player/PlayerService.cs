using Photon.Pun;
using System;
using System.Collections.Generic;
using TicTacToe.Core;
using TicTacToe.Utility;
using TicTacToe.Utility.Events;
using Unity.Netcode;
using UnityEngine;

namespace TicTacToe.Player
{
    public class PlayerService
    {
        private PlayerView playerView;
        private PlayerController playerController;

        public PlayerService(Transform parent, EventService eventService, PlayerView playerView, WinDataSO winDataSO)
        {
            this.playerView = playerView;
            playerController = new PlayerController(eventService, winDataSO, playerView);
            playerView.InitilizeData(eventService, playerController);
        }

        public void Rematch() => playerView.Rematch();
        public void RemoveEventListeners() => playerView.RemoveEventListeners();

        public PlayerType GetLocalPlayerType() => playerController.GetLocalPlayerType();
        public PlayerType GetCurrentPlayablePlayer() => playerController.GetCurrentPlayablePlayer();
    }
}
