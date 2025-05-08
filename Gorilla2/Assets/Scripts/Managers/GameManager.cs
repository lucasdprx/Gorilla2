using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static HashSet<int> playerIdList { get; private set; } = new();
        public static void AddPlayer(int playerId) => playerIdList.Add(playerId);
        public static event Action onGameFinished;
        [SerializeField] private List<int> playerIdListToTest = new();

        private void Awake()
        {
            playerIdList.Clear();
        }

        private void Update()
        {
            playerIdListToTest = playerIdList.ToList();
        }

        public static void RemovePlayer(int playerId)
        {
            if (playerIdList.Contains(playerId))
            {
                playerIdList.Remove(playerId);
            }

            if (playerIdList.Count == 1)
            {
                onGameFinished?.Invoke();
                Debug.Log($"Congrats to player {playerIdList.First()} for winning the game!");
                playerIdList.Clear();
            }
        }

        public static void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public static void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}