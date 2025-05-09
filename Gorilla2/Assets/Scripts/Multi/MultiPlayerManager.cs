using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiPlayerManager : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    
    public CameraTargetGroup cameraTargetGroup;
    
    public readonly static List<PlayerInput> playerInputs = new List<PlayerInput>();
    
    public static event Action<PlayerInput> onPlayerJoinedEvent; 
    public static event Action<PlayerInput> onPlayerLeftEvent;
    public static event Action<PlayerInput> onPlayerRegainedEvent;
    
    private PlayerInputManager playerInputManager;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += OnPlayerJoined;
    }
    
    private void OnPlayerLeft(PlayerInput player)
    {
        SetAllPlayerInputActive(false);
        onPlayerLeftEvent?.Invoke(player);
    }
    
    private void OnPlayerJoined(PlayerInput player)
    {
        playerInputs.Add(player);
        int index = playerInputs.IndexOf(player);
        spawnPoints[index].gameObject.SetActive(true);
        player.transform.position = spawnPoints[index].position;
        player.deviceRegainedEvent.AddListener(OnPlayerRegained);
        player.deviceLostEvent.AddListener(OnPlayerLeft);
        player.DeactivateInput();
        PlayerInfos playerInfos = player.GetComponent<PlayerInfos>();
        playerInfos.playerName.text = "P" + (index + 1);
        
        cameraTargetGroup.targets.Add(player.transform);
        
        onPlayerJoinedEvent?.Invoke(player);
    }
    private void OnPlayerRegained(PlayerInput player)
    {
        onPlayerRegainedEvent?.Invoke(player);
    }
    
    public static bool AllPlayersAreConnected()
    {
        return playerInputs.All(playerInput => !playerInput.hasMissingRequiredDevices);
    }

    public static void SetAllPlayerInputActive(bool active)
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
    
    private void OnDestroy()
    {
        playerInputManager.onPlayerJoined -= OnPlayerJoined;
        foreach (PlayerInput playerInput in playerInputs)
        {
            playerInput.onDeviceLost -= OnPlayerLeft;
            playerInput.deviceRegainedEvent.RemoveListener(OnPlayerRegained);
            playerInput.deviceLostEvent.RemoveListener(OnPlayerLeft);
        }
        playerInputs.Clear();
    }
}
