using TicTacToe.Player;
using UnityEngine;

namespace TicTacToe.AI
{
    public class TicTacTieAIController
    {
        Vector2Int selectedGrid;

        private PlayerType[,] playerTypeGrid;

        private float aiMoveDelay;
        private float timer;

        private bool canReturnMove = false;

        public TicTacTieAIController(PlayerType[,] playerTypeGrid, float delay)
        {
            this.playerTypeGrid = playerTypeGrid;
            aiMoveDelay = delay;
            timer = aiMoveDelay;
        }

        public Vector2Int GetMove()
        {
            canReturnMove = false;
            timer = aiMoveDelay;

            return GetRandomGrid();
        }


        private Vector2Int GetRandomGrid()
        {
            int row = Random.Range(0, playerTypeGrid.GetLength(0));
            int col = Random.Range(0, playerTypeGrid.GetLength(1));

            if (playerTypeGrid[row, col] != PlayerType.NONE)
            {
                return GetRandomGrid();
            }

            return new Vector2Int(row, col);
        }

        public void StartTimer()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                canReturnMove = true;
            }
        }

        public bool CanAIMove() => canReturnMove;
    }
}
