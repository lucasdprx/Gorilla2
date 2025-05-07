using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Button firstSelected;
    
    private bool gameIsPaused;
    
    public void Escape(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
        {
            ResumeGame();
        }
    }
    public void ResumeGame()
    {
        firstSelected.Select();
        pauseMenuUI.SetActive(!gameIsPaused);
        Time.timeScale = gameIsPaused ? 1f : 0f;
        gameIsPaused = !gameIsPaused;
    }
    
    public void LoadScene(int index)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(index);
    }
}
