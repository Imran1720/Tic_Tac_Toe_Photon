using TicTacToe.Core;
using TicTacToe.Player;
using TicTacToe.Utility.Events;
using TMPro;
using UnityEngine;

namespace TicTacToe.UI
{
    public class PlayerUI : MonoBehaviour
    {
        private EventService eventService;

        [SerializeField] private Transform crossArrow;
        [SerializeField] private Transform circleArrow;
        [SerializeField] private Transform crossYouText;
        [SerializeField] private Transform circleYouText;


        [SerializeField] private TextMeshProUGUI crossScoreText;
        [SerializeField] private TextMeshProUGUI circleScoreText;

        private void Awake()
        {
            DisableUIElements();
        }

        public void InitializeServices(EventService eventService)
        {
            this.eventService = eventService;
            AddEventListeners();
        }

        private void AddEventListeners()
        {
            eventService.OnGameStarted.AddListener(OnGameStart);
            eventService.OnScoreUpdated.AddListener(OnScoreUpdatedRpc);
            eventService.OnPlayerTurnChanged.AddListener(updateArrowRpc);
        }
        public void RemoveEventListeners()
        {
            eventService.OnGameStarted.RemoveListener(OnGameStart);
            eventService.OnScoreUpdated.RemoveListener(OnScoreUpdatedRpc);
            eventService.OnPlayerTurnChanged.RemoveListener(updateArrowRpc);

        }

        private void OnGameStart(PlayerType playerType)
        {
            ShowPlayerYouText(playerType);
            updateArrowRpc(GameService.Instance.GetCurrentPlayablePlayer());
        }

        private void OnScoreUpdatedRpc(int crossScore, int circleScore)
        {
            crossScoreText.text = crossScore.ToString();
            circleScoreText.text = circleScore.ToString();
        }

        private void updateArrowRpc(PlayerType playerType)
        {
            if (playerType == PlayerType.CROSS)
            {
                SwitchArrowToCross(true);
            }
            else
            {
                SwitchArrowToCross(false);
            }
        }

        private void SwitchArrowToCross(bool isCross)
        {
            crossArrow.gameObject.SetActive(isCross);
            circleArrow.gameObject.SetActive(!isCross);
        }

        private void DisableUIElements()
        {
            crossArrow.gameObject.SetActive(false);
            circleArrow.gameObject.SetActive(false);
            crossYouText.gameObject.SetActive(false);
            circleYouText.gameObject.SetActive(false);
        }

        private void ShowPlayerYouText(PlayerType playerType)
        {
            switch (playerType)
            {
                case PlayerType.CROSS:
                    crossYouText.gameObject.SetActive(true);
                    break;
                case PlayerType.CIRCLE:
                    circleYouText.gameObject.SetActive(true);
                    break;
            }
        }
    }
}