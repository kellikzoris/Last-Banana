using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    [SerializeField] Color _startColor;
    [SerializeField] Color _endColor;

    [SerializeField] float _duration;

    void Update()
    {
        GetComponent<SpriteRenderer>().color = Color.Lerp(_startColor, _endColor, Mathf.PingPong(Time.time, _duration));
    }
}
