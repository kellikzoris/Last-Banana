using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] private Color _startColor;
    [SerializeField] private Color _endColor;

    [SerializeField] private float _duration;

    private void Update()
    {
        GetComponent<Image>().color = Color.Lerp(_startColor, _endColor, Mathf.PingPong(Time.time, _duration));
    }
}