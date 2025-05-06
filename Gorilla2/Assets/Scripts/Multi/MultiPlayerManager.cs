using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiPlayerManager : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    private List<PlayerInput> playerInputs = new List<PlayerInput>();
    private int playerConnected;
    
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private GameObject panelConnectionPlayer;


    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += OnPlayerJoined;
    }
    
    private void OnPlayerLeft(PlayerInput obj)
    {
        print("leave");
        playerInputs.Remove(obj);
        playerConnected--;
        playerCountText.text = "Player Connected: " + playerConnected;
        panelConnectionPlayer.SetActive(true);
        //SetAllPlayerInputActive(false);
    }
    
    private void OnPlayerJoined(PlayerInput obj)
    {
        print("join");
        playerInputs.Add(obj);
        playerConnected++;
        playerCountText.text = "Player Connected: " + playerConnected;
        obj.deviceRegainedEvent.AddListener(OnPlayerRegained);
        obj.deviceLostEvent.AddListener(OnPlayerLeft);
    }
    private void OnPlayerRegained(PlayerInput obj)
    {
        print("regain");
        playerConnected++;
        playerCountText.text = "Player Connected: " + playerConnected;
        if (AllPlayersIsConnected())
        {
            panelConnectionPlayer.SetActive(false);
            SetAllPlayerInputActive(true);
        }
    }

    private void OnDestroy()
    {
        playerInputManager.onPlayerJoined -= OnPlayerJoined;
        foreach (PlayerInput playerInput in playerInputs)
        {
            playerInput.onDeviceLost -= OnPlayerLeft;
        }
    }
    
    private bool AllPlayersIsConnected()
    {
        return playerInputs.All(playerInput => !playerInput.hasMissingRequiredDevices);
    }
    
    private void SetAllPlayerInputActive(bool active)
    {
        foreach (PlayerInput playerInput in playerInputs.ToList())
        {
            playerInput.enabled = active;
        }
    }
}
