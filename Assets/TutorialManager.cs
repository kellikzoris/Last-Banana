using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Image _eatBananaWithEButtonBackgroundImage;
    [SerializeField] private Image _approachMonkeyImage;
    [SerializeField] private Image _meleeAttackImage;
    [SerializeField] private Image _rightClickAttackImage;
    [SerializeField] private Image _lockedOnTargetImage;

    [SerializeField] private List<TMP_Text> _textFields;

    [SerializeField] private Color _targetColor;
    [SerializeField] private Color _disabledTargetColor;
    [SerializeField] private Color _disabledTextColor;

    private bool _completed = false;

    public void SetLockedOnTargetColor()
    {
        if (_completed)
        {
            return;
        }
        _lockedOnTargetImage.color = _targetColor;
        CheckIfAllDone();
    }

    public void SetRightClickAttackColor()
    {
        if (_completed)
        {
            return;
        }
        _rightClickAttackImage.color = _targetColor;
        CheckIfAllDone();
    }

    public void SetMeleeAttackColor()
    {
        if (_completed)
        {
            return;
        }
        _meleeAttackImage.color = _targetColor;
        CheckIfAllDone();
    }

    public void DoApproachMonkeyImage()
    {
        if (_completed)
        {
            return;
        }
        _approachMonkeyImage.color = _targetColor;
        CheckIfAllDone();
    }

    public bool GetIsTutorialCompleted()
    {
        return _completed;
    }

    public void SetIsTutorialCompleted()
    {
        foreach (TMP_Text textField in _textFields)
        {
            textField.color = _disabledTextColor;
        }

        _eatBananaWithEButtonBackgroundImage.color = _targetColor;
        _approachMonkeyImage.color = _targetColor;
        _meleeAttackImage.color = _targetColor;
        _rightClickAttackImage.color = _targetColor;
        _lockedOnTargetImage.color = _targetColor;
        CheckIfAllDone();
    }

    public void CheckIfAllDone()
    {
        _completed = _lockedOnTargetImage.color == _targetColor &&
            _eatBananaWithEButtonBackgroundImage.color == _targetColor &&
            _rightClickAttackImage.color == _targetColor &&
            _meleeAttackImage.color == _targetColor &&
            _approachMonkeyImage.color == _targetColor;

        if (_completed)
        {
            foreach (TMP_Text textField in _textFields)
            {
                textField.color = _disabledTextColor;
            }

            _eatBananaWithEButtonBackgroundImage.color = _disabledTargetColor;
            _approachMonkeyImage.color = _disabledTargetColor;
            _meleeAttackImage.color = _disabledTargetColor;
            _rightClickAttackImage.color = _disabledTargetColor;
            _lockedOnTargetImage.color = _disabledTargetColor;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_completed)
            {
                return;
            }
            _eatBananaWithEButtonBackgroundImage.color = _targetColor;
            CheckIfAllDone();
        }
    }
}