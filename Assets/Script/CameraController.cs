using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    public float zoomedOutSize = 10f;
    public float normalSize = 5f;
    public float zoomSpeed = 2f;

    public void ZoomOut()
    {
        StartCoroutine(Zoom(zoomedOutSize));
    }

    public void ZoomIn()
    {
        StartCoroutine(Zoom(normalSize));
    }

    private IEnumerator Zoom(float targetSize)
    {
        while (Mathf.Abs(mainCamera.orthographicSize - targetSize) > 0.1f)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
            yield return null;
        }
        mainCamera.orthographicSize = targetSize;
    }
}