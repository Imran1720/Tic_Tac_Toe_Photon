using System;
using TicTacToe.Core;
using TicTacToe.Player;
using TicTacToe.Utility;
using TicTacToe.Utility.Events;
using UnityEngine;
namespace TicTacToe.Board
{
    public class BoardController
    {
        private int rows;
        private int columns;
        private float gridSize;

        private EventService eventService;
        private BoardView boardView;

        public BoardController(BoardView boardView, GridData gridData, EventService eventService)
        {
            this.boardView = boardView;
            InitializeData(gridData);
            SpawnBoardTiles(eventService);

            eventService.OnBoardHighlightRequested.AddListener(ToggleBoardHighlight);
        }

        private void InitializeData(GridData gridData)
        {
            rows = gridData.rowCount;
            gridSize = gridData.gridSize;
            columns = gridData.columnCount;
        }

        private void SpawnBoardTiles(EventService eventService)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    boardView.SpawnTile(GetTileWorldPosition(i, j), Quaternion.identity, i, j, eventService);
                }
            }
        }

        public Vector2 GetTileWorldPosition(int gridX, int gridY)
        {
            return new Vector2(gridY * gridSize - gridSize, gridSize - gridX * gridSize);
        }


        private void ToggleBoardHighlight(PlayerType playerType)
        {
            if (GameService.Instance.GetLocalPlayerType() == playerType)
            {
                EnableBoardHighlight();
            }
            else
            {
                DisableBoardHighlight();
            }
        }

        public void EnableBoardHighlight()
        {
            boardView.EnableBoardHighlight();
        }

        public void DisableBoardHighlight()
        {
            boardView.DisableBoardHighlight();
        }

    }
}