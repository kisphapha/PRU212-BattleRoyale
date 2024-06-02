using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float zoomOutOrthographicSize = 15f; // The target orthographic size when zoomed out
    public float zoomSpeed = 2f; // The speed at which the camera zooms

    private float originalOrthographicSize;
    private bool isZoomingIn = false;

    void Start()
    {
        virtualCamera = FindFirstObjectByType<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            originalOrthographicSize = virtualCamera.m_Lens.OrthographicSize;
        }
        else
        {
            Debug.LogError("Virtual Camera is not assigned.");
        }
    }
    public void setIsZoomingIn(bool isZoomingIn)
    {
        this.isZoomingIn = isZoomingIn;
    }
    void Update()
    {
        if (virtualCamera != null)
        {

            float targetOrthographicSize = isZoomingIn ? zoomOutOrthographicSize : originalOrthographicSize;
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(
                virtualCamera.m_Lens.OrthographicSize,
                targetOrthographicSize,
                Time.deltaTime * zoomSpeed
            );
        }
    }
}