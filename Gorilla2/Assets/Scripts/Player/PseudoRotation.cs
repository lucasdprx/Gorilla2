using System;
using UnityEngine;

public class PseudoRotation : MonoBehaviour
{
    private Transform pseudo;

    private void Start()
    {
        pseudo = transform;
    }
    private void Update()
    {
        pseudo.rotation = Quaternion.identity;
    }
}
