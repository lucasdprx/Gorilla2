using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float speedZoom = 5;
    [SerializeField] private float minSize = 10;
    [SerializeField] private float maxSize = 20;
    [SerializeField] private float deadZone = 0.05f;
    
    private Camera mainCamera;
    private CameraTargetGroup cameraTargetGroup;

    private void Start()
    {
        cameraTargetGroup = GetComponent<CameraTargetGroup>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (cameraTargetGroup.targets.Count == 0)
        {
            return;
        }
        
        float orthographicSize = mainCamera.orthographicSize;
        orthographicSize += AreAllVisible() ? Time.deltaTime * -speedZoom : Time.deltaTime * speedZoom;
        orthographicSize = Mathf.Clamp(orthographicSize, minSize, maxSize);
        mainCamera.orthographicSize = orthographicSize;
    }

    private bool IsVisible(Vector3 target)
    {
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(target);
        return viewportPos.x > deadZone && viewportPos.x < 1 - deadZone &&
               viewportPos.y > deadZone && viewportPos.y < 1 - deadZone;
    }
    private bool AreAllVisible()
    {
        return cameraTargetGroup.targets.Where(target => !target).All(target => IsVisible(target.transform.position));
    }
}
