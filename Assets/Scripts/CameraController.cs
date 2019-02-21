using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform areaTransform;
    private void Awake()
    {
      //  areaTransform = GameObject.Find("GameManager").GetComponent<GameManager>().areaTransform;
    }

    public void SetCameraSize(Transform areaTransform)
    {
        Camera.main.orthographicSize = FindRequiredZoom(areaTransform);
    }

    private float FindRequiredZoom(Transform areaTransform)
    {
        float rate = 5.6f; //multiplier from localScale to camera size
        return Mathf.Max(areaTransform.localScale.y, areaTransform.localScale.x / Camera.main.aspect)*rate;
    }
}
