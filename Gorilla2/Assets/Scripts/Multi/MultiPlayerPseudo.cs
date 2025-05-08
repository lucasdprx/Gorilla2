using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MultiPlayerPseudo : MonoBehaviour
{
    private int indexPlayer;
    
    [SerializeField] private List<TextMeshProUGUI> playerPseudos;
    
    public void OnPseudoChange(string pseudo)
    {
        print("Pseudo Change " + pseudo);
        if (indexPlayer < 0 || indexPlayer >= playerPseudos.Count || playerPseudos.Count == 0)
        {
            return;
        }
        playerPseudos[indexPlayer].text = pseudo;
    }

    public void OnSelect(int index)
    {
        print("Select " + index);
        indexPlayer = index;
    }
}
