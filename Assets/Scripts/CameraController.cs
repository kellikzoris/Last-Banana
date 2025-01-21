using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Vector3 _offsetValue;
    [SerializeField] private bool _enableCameraTracking = false;

    private void Start()
    {
    }

    private void LateUpdate()
    {
        if (_enableCameraTracking)
        {
            Camera.main.transform.position = _player.transform.position + new Vector3(0, 0, -10) + _offsetValue;
        }
    }
}