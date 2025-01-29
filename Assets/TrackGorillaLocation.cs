using System.Collections;
using UnityEngine;

public class TrackGorillaLocation : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Vector2 _cameraOffsetPosition = new Vector2(5, 15);
    [SerializeField] private bool _lockOnPlayer;
    [SerializeField] private RectTransform _maskImageRectTransform;
    [SerializeField] private float _lerpSpeed;
    [SerializeField] private int _circleSize;
    [SerializeField] private int _backgroundImageSize;

    public void SetTrackGorilla(int timeAmountForThisAction)
    {
        _lockOnPlayer = true;
        StartCoroutine(TimerForTrackGorillaLocation(timeAmountForThisAction));
    }

    private IEnumerator TimerForTrackGorillaLocation(float timeAmountForThisAction)
    {
        yield return new WaitForSeconds(timeAmountForThisAction);
        _lockOnPlayer = false;
    }

    private void Update()
    {
        if (_lockOnPlayer)
        {
            if (_maskImageRectTransform.sizeDelta.x > _circleSize)
            {
                _maskImageRectTransform.sizeDelta -= Vector2.one * Time.deltaTime * _lerpSpeed * 1000;
            }
            this.transform.position = Camera.main.WorldToScreenPoint(_player.transform.position) + (Vector3)_cameraOffsetPosition;
        }
        else
        {
            if (_maskImageRectTransform.sizeDelta.x <= _backgroundImageSize)
            {
                _maskImageRectTransform.sizeDelta += Vector2.one * Time.deltaTime * _lerpSpeed * 1000;
            }
        }
    }
}