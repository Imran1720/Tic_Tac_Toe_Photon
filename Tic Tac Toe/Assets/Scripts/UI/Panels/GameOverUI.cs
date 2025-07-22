using TicTacToe.Core;
using TicTacToe.Player;
using TicTacToe.Utility.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    private GameService gameService;
    private EventService eventService;

    [SerializeField] private Transform background;
    [SerializeField] private TextMeshProUGUI prompt;

    [SerializeField] private Color winColor;
    [SerializeField] private Color loseColor;
    [SerializeField] private Color tieColor;

    [SerializeField] private Button rematchButton;

    public void InitializeServices(EventService eventService, GameService gameService)
    {
        this.eventService = eventService;
        this.gameService = gameService;

        rematchButton.onClick.AddListener(Rematch);
        AddEventListeners();
        Hide();
    }

    private void AddEventListeners()
    {
        eventService.OnGameWon.AddListener(OnWin);
        eventService.OnGameTied.AddListener(OnGameTie);
        eventService.OnRematch.AddListener(OnRematch);
    }

    public void RemoveEventListeners()
    {
        eventService.OnGameWon.RemoveListener(OnWin);
        eventService.OnGameTied.RemoveListener(OnGameTie);
        eventService.OnRematch.RemoveListener(OnRematch);
    }

    private void OnWin(PlayerType playerType)
    {
        if (GameService.Instance.GetLocalPlayerType() == playerType)
        {
            SetData("You win", winColor);
        }
        else
        {
            SetData("You Lose", loseColor);
        }
    }

    private void SetData(string message, Color color)
    {
        prompt.color = color;
        prompt.text = message;
        Show();
    }

    private void OnGameTie()
    {
        SetData("Tie", tieColor);
    }

    private void Rematch()
    {
        gameService.Rematch();
    }

    private void OnRematch()
    {
        Hide();
    }

    private void Hide()
    {
        rematchButton.gameObject.SetActive(false);
        background.gameObject.SetActive(false);
    }

    private void Show()
    {
        rematchButton.gameObject.SetActive(true);
        background.gameObject.SetActive(true);
    }
}
