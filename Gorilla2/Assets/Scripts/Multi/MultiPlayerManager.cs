using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MultiPlayerManager : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    private readonly List<PlayerInput> playerInputs = new List<PlayerInput>();
    private int playerConnected;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private GameObject panelConnectionPlayer;
    [SerializeField] private List<Image> playerImages;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += OnPlayerJoined;
    }
    
    private void OnPlayerLeft(PlayerInput player)
    {
        if (panelConnectionPlayer)
        {
            panelConnectionPlayer.SetActive(true);
        }
        
        UpdatePlayerCount(-1);
        SetAllPlayerInputActive(false);
        UpdatePlayerImage(player, Color.gray);
    }
    
    private void OnPlayerJoined(PlayerInput player)
    {
        playerInputs.Add(player);
        UpdatePlayerCount(1);
        player.deviceRegainedEvent.AddListener(OnPlayerRegained);
        player.deviceLostEvent.AddListener(OnPlayerLeft);
        
        SetActivePlayerImage(player, true);
    }
    private void OnPlayerRegained(PlayerInput player)
    {
        UpdatePlayerCount(1);
        UpdatePlayerImage(player, Color.white);
        StartCoroutine(WaitForReturnToTheGame(1));
    }

    private IEnumerator WaitForReturnToTheGame(float second)
    {
        yield return new WaitForSeconds(second);
        if (AllPlayersAreConnected() && panelConnectionPlayer)
        {
            panelConnectionPlayer.SetActive(false);
            SetAllPlayerInputActive(true);
        }
    }
    
    private bool AllPlayersAreConnected()
    {
        return playerInputs.All(playerInput => !playerInput.hasMissingRequiredDevices);
    }
    
    private void UpdatePlayerCount(int delta)
    {
        if (playerCountText == null)
        {
            return;
        }
        
        playerConnected += delta;
        playerCountText.text = "Player Connected: " + playerConnected;
    }

    private void SetAllPlayerInputActive(bool active)
    {
        foreach (PlayerInput playerInput in playerInputs)
        {
            if (active)
            {
                playerInput.ActivateInput();
            }
            else
            {
                playerInput.DeactivateInput();
            }
        }
    }

    private void UpdatePlayerImage(PlayerInput player, Color color)
    {
        int index = playerInputs.IndexOf(player);
        if (player == null || index < 0 || index >= playerImages.Count || playerImages[index] == null)
        {
            return;
        }
        
        playerImages[index].color = color;
    }
    
    private void SetActivePlayerImage(PlayerInput player, bool active)
    {
        int index = playerInputs.IndexOf(player);
        if (index >= 0 && index < playerImages.Count)
        {
            playerImages[index].gameObject.SetActive(active);
        }
    }
    
    private void OnDestroy()
    {
        playerInputManager.onPlayerJoined -= OnPlayerJoined;
        foreach (PlayerInput playerInput in playerInputs)
        {
            playerInput.onDeviceLost -= OnPlayerLeft;
            playerInput.deviceRegainedEvent.RemoveListener(OnPlayerRegained);
            playerInput.deviceLostEvent.RemoveListener(OnPlayerLeft);
        }
    }
}
