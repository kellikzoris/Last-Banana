using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSceneWelcomeText : MonoBehaviour
{
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;
    [SerializeField] private Button _startButton;

    private void Start()
    {
        this.GetComponent<TMPro.TMP_Text>().text = "Let's find out who deserves the crown of the jungle!";
        _startButton.gameObject.SetActive(true);
        _yesButton.gameObject.SetActive(false);
        _noButton.gameObject.SetActive(false);
    }
}