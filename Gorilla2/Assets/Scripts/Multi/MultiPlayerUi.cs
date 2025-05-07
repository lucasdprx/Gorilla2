using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class MultiPlayerUi : MonoBehaviour
{
    [SerializeField] private GameObject connectionPanel;
    [FormerlySerializedAs("playerImages")]
    [SerializeField] private List<Image> playerImagesConnected;
    [SerializeField] private List<GameObject> playerImages;
    [SerializeField] private List<GameObject> playerConnexion;

    private void Awake()
    {
        MultiPlayerManager.onPlayerLeftEvent += OnPlayerLeft;
        MultiPlayerManager.onPlayerJoinedEvent += OnPlayerJoined;
        MultiPlayerManager.onPlayerRegainedEvent += OnPlayerRegained;
    }
    private void OnPlayerLeft(PlayerInput player)
    {
        int index = MultiPlayerManager.playerInputs.IndexOf(player);
        SetPanelConnectionPlayerActive(true);
        UpdatePlayerImage(index, Color.gray);
    }
    private void OnPlayerJoined(PlayerInput player)
    {
        int index = MultiPlayerManager.playerInputs.IndexOf(player);
        playerConnexion[index].SetActive(true);
        SetActivePlayerImage(index, true);
        if (index >= 0 && index < playerImages.Count)
        {
            playerImages[index].SetActive(true);
        }
    }
    private void OnPlayerRegained(PlayerInput player)
    {
        int index = MultiPlayerManager.playerInputs.IndexOf(player);
        SetPanelConnectionPlayerActive(false);
        UpdatePlayerImage(index, Color.white);
    }

    private void UpdatePlayerImage(int index, Color color)
    {
        if (index < 0 || index >= playerImagesConnected.Count || playerImagesConnected[index] == null)
        {
            return;
        }
        
        playerImagesConnected[index].color = color;
    }

    private void SetActivePlayerImage(int index, bool active)
    {
        if (index >= 0 && index < playerImagesConnected.Count)
        {
            playerImagesConnected[index].gameObject.SetActive(active);
        }
    }

    private void SetPanelConnectionPlayerActive(bool active)
    {
        if (connectionPanel == null)
        {
            return;
        }
        
        if (active)
        {
            connectionPanel.SetActive(true);
        }
            
        else if (connectionPanel.activeSelf)
        {
            StartCoroutine(HidePanelAfterDelay(0.5f));
        }
    }
    
    private IEnumerator HidePanelAfterDelay(float second)
    {
        yield return new WaitForSeconds(second);
        if (MultiPlayerManager.AllPlayersAreConnected())
        {
            connectionPanel.SetActive(false);
            MultiPlayerManager.SetAllPlayerInputActive(true);
        }
    }
    
    private void OnDestroy()
    {
        MultiPlayerManager.onPlayerLeftEvent -= OnPlayerLeft;
        MultiPlayerManager.onPlayerJoinedEvent -= OnPlayerJoined;
        MultiPlayerManager.onPlayerRegainedEvent -= OnPlayerRegained;
    }
}
