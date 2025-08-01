using TicTacToe.Utility.Events;
using UnityEngine;

namespace TicTacToe.Board
{
    public class BoardService
    {
        private BoardController controller;
        private BoardView boardView;
        public BoardService(BoardView boardPrefab, Transform parentTransform, GridData gridData, EventService eventService)
        {
            boardView = GameObject.Instantiate(boardPrefab, parentTransform.position, Quaternion.identity);
            boardView.transform.SetParent(parentTransform, false);

            controller = new BoardController(boardView, gridData, eventService);
        }

        public Vector2 GetTileWorldPosition(int gridX, int gridY)
        {
            return controller.GetTileWorldPosition(gridX, gridY);
        }

    }
}
