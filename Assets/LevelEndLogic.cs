using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelEndLogic : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjectsToDisable;
    [SerializeField] private GameObject gameObjectToEnable;
    [SerializeField] private TMP_Text textFieldToUpdate;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    public void CallLevelEndCondition(bool isPlayerWin)
    {
        foreach (GameObject item in gameObjectsToDisable)
        {
            if (item != null)
            {
                item.SetActive(false);
            }
        }

        if (gameObjectToEnable != null)
        {
            gameObjectToEnable.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name == "LastBanana_TigerFight")
        {
            if (textFieldToUpdate != null)
            {
                textFieldToUpdate.text = isPlayerWin
                    ? "You managed to beat the Tiger! Let's get back and do another fight."
                    : "The Tiger managed to beat you. Do you want to try again?";
            }
        }
        else
        {
            if (textFieldToUpdate != null)
            {
                textFieldToUpdate.text = isPlayerWin
                    ? "You managed to beat the Bull! Let's get back and do another fight."
                    : "The Bull managed to beat you. Do you want to try again?";
            }
        }

        SetupButtons(isPlayerWin);
    }

    private void SetupButtons(bool isPlayerWin)
    {
        if (yesButton != null)
        {
            yesButton.gameObject.SetActive(true);
            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(isPlayerWin ? ReturnBackToWelcomeScene : RestartTheScene);
        }

        if (noButton != null)
        {
            noButton.gameObject.SetActive(!isPlayerWin);
            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener(ReturnBackToWelcomeScene);
        }
    }

    public void RestartTheScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnBackToWelcomeScene()
    {
        SceneManager.LoadScene("LastBanana_WelcomeScene");
    }
}