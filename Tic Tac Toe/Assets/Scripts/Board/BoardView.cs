using TicTacToe.Utility.Events;
using UnityEngine;

namespace TicTacToe.Board
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private GameObject boardHighlight;

        [SerializeField] private GridTile clickableTilePrefab;
        public void SpawnTile(Vector2 position, Quaternion roation, int gridX, int gridY, EventService eventService)
        {
            GridTile tile = Instantiate(clickableTilePrefab, position, roation);
            MakeChildToCurrentObject(tile.transform);
            tile.InitializeData(eventService, gridX, gridY);
        }

        private void MakeChildToCurrentObject(Transform childTransform) => childTransform.SetParent(transform, false);

        public void EnableBoardHighlight()
        {
            boardHighlight.SetActive(true);
        }

        public void DisableBoardHighlight()
        {
            boardHighlight?.SetActive(false);
        }

    }
}