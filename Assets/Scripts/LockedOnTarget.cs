using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockedOnTarget : MonoBehaviour
{
    [SerializeField] Image backgroundImage;
    [SerializeField] TMP_Text textField;
    Coroutine flashingEffectCO;


    private void Start()
    {
        backgroundImage = GetComponent<Image>();
        textField = GetComponentInChildren<TMP_Text>();
    }

    public void EnableTheTextAndAnimation()
    {
        backgroundImage.enabled = true;
        textField.enabled = true;
        flashingEffectCO = StartCoroutine(FlashingEffect());

    }

    public void DisableTheTextAndAnimation()
    {
        backgroundImage.enabled = false;
        textField.enabled = false;
        StopCoroutine(flashingEffectCO);
    }

    IEnumerator FlashingEffect()
    {
        yield return new WaitForSeconds(.25f);
        backgroundImage.color = Color.red;
        textField.color = Color.white;
        yield return new WaitForSeconds(.25f);
        backgroundImage.color = Color.white;
        textField.color = Color.red;
    }
}
