using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EndMenuManager : MonoBehaviour
    {
        private CanvasGroup endMenuPanel;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button exitButton;

        private void Awake()
        {
            endMenuPanel = GetComponent<CanvasGroup>();
            endMenuPanel.alpha = 0;
            endMenuPanel.interactable = false;
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
            ShowEndMenu();
        }

        private void ShowEndMenu()
        {
            endMenuPanel.alpha = 1;
            endMenuPanel.interactable = true;
        }

        private void OnDestroy()
        {
            GameManager.onGameFinished -= OnGameFinished;
        }
    }
}