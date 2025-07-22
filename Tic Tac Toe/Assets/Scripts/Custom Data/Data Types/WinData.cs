using System;
using UnityEngine;
namespace TicTacToe.Utility
{
    [Serializable]
    public struct WinData
    {
        public Vector2Int[] winGridPair;
        public Vector2Int centerGrid;
        public float Orientation;
    }
}
