using System;
using Managers;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EndMenuManager : MonoBehaviour
    {
        private CanvasGroup endMenuPanel;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private TextMeshProUGUI winText;

        private void Awake()
        {
            endMenuPanel = GetComponent<CanvasGroup>();
            endMenuPanel.alpha = 0;
            endMenuPanel.interactable = false;
            endMenuPanel.blocksRaycasts = false;
        }

        private void Start()
        {
            GameManager.onGameFinished += OnGameFinished;
            restartButton.onClick.AddListener(RestartGame);
            exitButton.onClick.AddListener(ExitGame);
        }

        private void ExitGame()
        {
            GameManager.ExitGame();
        }

        private void RestartGame()
        {
            GameManager.RestartGame();
        }

        private void OnGameFinished()
        {
            int winnerId = GameManager.playerIdList.Last();
            string winnerName = MultiPlayerManager.playerInputs[winnerId].GetComponent<PlayerInfos>().playerName.text;
            winText.text = winnerName + " wins the game !!!";
            ShowEndMenu();
        }

        private void ShowEndMenu()
        {
            endMenuPanel.alpha = 1;
            endMenuPanel.interactable = true;
            endMenuPanel.blocksRaycasts = true;
        }

        private void OnDestroy()
        {
            GameManager.onGameFinished -= OnGameFinished;
        }
    }
}