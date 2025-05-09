using System;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    private Transform @object;

    private void Start()
    {
        @object = transform;
    }
    private void LateUpdate()
    {
        @object.rotation = Quaternion.identity;
    }
}
