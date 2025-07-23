using System.Collections.Generic;
using TicTacToe.Player;
using TicTacToe.Utility;
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
        private WinDataSO winDataSO;

        private List<Vector2Int> priorityGrids = new List<Vector2Int>();

        private int levelDifficulty = 1;

        public TicTacTieAIController(PlayerType[,] playerTypeGrid, float delay, WinDataSO winDataSO)
        {
            this.playerTypeGrid = playerTypeGrid;
            this.winDataSO = winDataSO;

            levelDifficulty = PlayerPrefs.GetInt("levelDifficaulty");

            aiMoveDelay = delay;
            timer = aiMoveDelay;
            this.winDataSO = winDataSO;
            InitializePriorityGrid();

        }

        public void InitializePriorityGrid()
        {
            priorityGrids.Add(new Vector2Int(0, 0));
            priorityGrids.Add(new Vector2Int(0, 2));
            priorityGrids.Add(new Vector2Int(2, 0));
            priorityGrids.Add(new Vector2Int(2, 2));
            priorityGrids.Add(new Vector2Int(1, 1));
        }

        public Vector2Int GetMove()
        {
            Vector2Int gridPosition;
            if (levelDifficulty == 2)
            {
                gridPosition = GetAIWinGrid();
                if (gridPosition.x != -1)
                {
                    return gridPosition;
                }

                gridPosition = GetPlayerWinBreakGrid();
                if (gridPosition.x != -1)
                {
                    return gridPosition;
                }


                gridPosition = GetRandomPriorityGrid();
                if (gridPosition.x != -1)
                {
                    return gridPosition;
                }
            }

            return GetRandomGrid();
        }

        private Vector2Int GetPlayerWinBreakGrid()
        {
            WinData data = GetPlayerWinGridPair();

            if (data.winGridPair != null)
            {
                for (int i = 0; i < data.winGridPair.Length; i++)
                {
                    if (playerTypeGrid[data.winGridPair[i].x, data.winGridPair[i].y] == PlayerType.NONE)
                    {
                        return data.winGridPair[i];
                    }
                }
            }
            return new Vector2Int(-1, -1);
        }

        private Vector2Int GetAIWinGrid()
        {
            WinData data = GetAIWinGridPair();

            if (data.winGridPair != null)
            {
                for (int i = 0; i < data.winGridPair.Length; i++)
                {
                    if (playerTypeGrid[data.winGridPair[i].x, data.winGridPair[i].y] == PlayerType.NONE)
                    {
                        return data.winGridPair[i];
                    }
                }
            }
            return new Vector2Int(-1, -1);
        }

        private WinData GetPlayerWinGridPair()
        {
            int count;
            foreach (WinData data in winDataSO.winDataList)
            {
                count = 0;

                for (int i = 0; i < data.winGridPair.Length; i++)
                {
                    if (playerTypeGrid[data.winGridPair[i].x, data.winGridPair[i].y] == PlayerType.CROSS)
                    {
                        count++;
                    }
                    if (playerTypeGrid[data.winGridPair[i].x, data.winGridPair[i].y] == PlayerType.CIRCLE)
                    {
                        count--;
                    }

                }
                if (count == 2)
                {
                    return data;
                }
            }
            return new WinData();
        }


        private WinData GetAIWinGridPair()
        {
            int count;
            foreach (WinData data in winDataSO.winDataList)
            {
                count = 0;

                for (int i = 0; i < data.winGridPair.Length; i++)
                {
                    if (playerTypeGrid[data.winGridPair[i].x, data.winGridPair[i].y] == PlayerType.CIRCLE)
                    {
                        count++;
                    }
                    if (playerTypeGrid[data.winGridPair[i].x, data.winGridPair[i].y] == PlayerType.CROSS)
                    {
                        count--;
                    }

                }
                if (count == 2)
                {
                    return data;
                }
            }
            return new WinData();
        }


        private Vector2Int GetRandomPriorityGrid()
        {
            if (priorityGrids.Count == 0)
            {
                return new Vector2Int(-1, -1);
            }

            int priority = Random.Range(0, priorityGrids.Count);

            Vector2Int grid = priorityGrids[priority];
            priorityGrids.RemoveAt(priority);

            if (playerTypeGrid[grid.x, grid.y] != PlayerType.NONE)
            {
                return GetRandomPriorityGrid();
            }

            return grid;
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
