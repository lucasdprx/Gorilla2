using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiPlayerPseudo : MonoBehaviour
{
    private int indexPlayer;
    [SerializeField] private List<TMP_InputField> pseudosInputField;
    [SerializeField] private List<TextMeshProUGUI> playersPseudoInGame;

    private void Start()
    {
        for (int i = 0; i < pseudosInputField.Count; i++)
        {
            if (PlayerPrefs.HasKey("Player" + i))
            {
                pseudosInputField[i].text = PlayerPrefs.GetString("Player" + i);
            }
        }
    }

    public void OnPseudoChange(string pseudo)
    {
        PlayerPrefs.SetString("Player" + indexPlayer, pseudo);
    }

    public void OnSelect(int index)
    {
        indexPlayer = index;
    }
    
    public void SetAllPseudo()
    {
        List<PlayerInput> players = MultiPlayerManager.playerInputs;
        for (int i = 0; i < players.Count; i++)
        {
            if (PlayerPrefs.HasKey("Player" + i))
            {
                players[i].GetComponent<PlayerInfos>().playerName.text = PlayerPrefs.GetString("Player" + i);
                playersPseudoInGame[i].text = PlayerPrefs.GetString("Player" + i);
            }
        }
    }
}
