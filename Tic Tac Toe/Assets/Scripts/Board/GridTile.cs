using System;
using TicTacToe.Core;
using TicTacToe.Player;
using TicTacToe.Utility.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace TicTacToe.Board
{
    public class GridTile : MonoBehaviour, IPointerClickHandler
    {

        [SerializeField] private Sprite crossSprite;
        [SerializeField] private Sprite circleSprite;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private Vector2Int gridPosition;
        private EventService eventService;

        private bool isInteractable = false;


        private void Awake()
        {
            spriteRenderer.enabled = false;
        }

        public void InitializeData(EventService eventService, int x, int y)
        {
            this.eventService = eventService;
            gridPosition.x = x;
            gridPosition.y = y;

            AddEventListeners();
        }

        private void AddEventListeners()
        {
            eventService.OnGameStarted.AddListener(OnGameStarted);
            eventService.OnDisableInteraction.AddListener(OnGameEnded);
            eventService.OnRematch.AddListener(OnRematch);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isInteractable)
            {
                GameService.Instance.GetEventService().OnTileClicked.InvokeEvent(gridPosition, GameService.Instance.GetLocalPlayerType());
            }
        }

        public void SetGridPosition()
        {

        }

        private void OnMouseOver()
        {
            if (isInteractable)
            {
                spriteRenderer.sprite = GetHoverSprite();
                spriteRenderer.enabled = true;
            }
        }

        private void OnMouseExit()
        {
            HideRenderer();
        }

        private void HideRenderer() => spriteRenderer.enabled = false;

        private void OnGameStarted(PlayerType player)
        {
            isInteractable = true;
        }

        private void OnGameEnded()
        {
            isInteractable = false;
        }

        private void OnRematch()
        {
            isInteractable = true;
        }

        public Sprite GetHoverSprite()
        {
            PlayerType playerType = GameService.Instance.GetLocalPlayerType();

            switch (playerType)
            {
                case PlayerType.CIRCLE: return circleSprite;
                case PlayerType.CROSS:
                default: return crossSprite;
            }

        }


    }
}
