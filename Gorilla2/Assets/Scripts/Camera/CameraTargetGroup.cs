using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraTargetGroup : MonoBehaviour
{
    [Header("Update mode")]
    public UpdateMode mode = UpdateMode.LateUpdate;

    [Header("Freeze position")]
    public bool freezeX;
    public bool freezeY;
    
    public List<Transform> targets = new List<Transform>();
    
    private Transform cameraTransform;
    
    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        if (mode == UpdateMode.FixedUpdate)
        {
            FollowTargets();
        }
    }
    private void Update()
    {
        if (mode == UpdateMode.Update)
        {
            FollowTargets();
        }
    }
    private void LateUpdate()
    {
        if (mode == UpdateMode.LateUpdate)
        {
            FollowTargets();
        }
    }
    
    private void FollowTargets()
    {
        if (targets.Count == 0)
            return;

        Vector3 center = targets.Where(target => !target).Aggregate(Vector3.zero, (current, target) => current + target.position);
        center /= targets.Count;

        Vector3 position = cameraTransform.position;
        if (!freezeX)
            position.x = center.x;
        if (!freezeY)
            position.y = center.y;
        
        cameraTransform.position = position;
    }
    public enum UpdateMode
    {
        FixedUpdate,
        Update,
        LateUpdate
    }
}
