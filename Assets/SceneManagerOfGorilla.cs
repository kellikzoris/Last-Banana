using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerOfGorilla : MonoBehaviour
{
    public enum ScenesToLoad
    {
        TigerFightScene,
        BullFightScene,
        NotEnteringScene
    }

    public ScenesToLoad selectedScene;

    [SerializeField] private GameObject doorWelcomeText;
    [SerializeField] private TMP_Text welcomeTextField;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private Button okButton;

    private bool _isTutorialCompleted;
    private bool _skipCheckingTutorialCompleted;

    private int _loadTigerFightCompleted = 0;
    private int _loadTutorialCompleted = 0;
    private int _loadBullFightCompleted = 0;

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Application ending after " + Time.time + " seconds");
    }

    private void Start()
    {
        _loadTigerFightCompleted = PlayerPrefs.GetInt("TigerFightCompleted", 0);
        _loadBullFightCompleted = PlayerPrefs.GetInt("BullFightCompleted", 0);

        Debug.Log($"StartCalledWithParameteres {_loadTutorialCompleted} and {_loadTigerFightCompleted} " +
            $"and {_loadBullFightCompleted}");

        if (_loadTigerFightCompleted == 1)
        {
            Door[] doors = FindObjectsOfType<Door>();
            foreach (Door door in doors)
            {
                if (door.thisDoorLeadsTo == ScenesToLoad.TigerFightScene)
                {
                    door.SetDoorOpen(true);
                    //door.gameObject.SetActive(false);
                }
            }
            TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();
            if (tutorialManager != null)
            {
                tutorialManager.SetIsTutorialCompleted();
            }
        }

        if (_loadBullFightCompleted == 1)
        {
            Door[] doors = FindObjectsOfType<Door>();
            foreach (Door door in doors)
            {
                if (door.thisDoorLeadsTo == ScenesToLoad.BullFightScene)
                {
                    door.SetDoorOpen(true);
                    //door.gameObject.SetActive(false);
                }
            }
            TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();
            if (tutorialManager != null)
            {
                tutorialManager.SetIsTutorialCompleted();
            }
        }

        StartGameWelcomeDialogue();
    }

    private void StartGameWelcomeDialogue()
    {
        if (_loadTigerFightCompleted == 1 || _loadBullFightCompleted == 1)
        {
            doorWelcomeText.gameObject.SetActive(false);
            return;
        }
        //welcomeTextField.text = "The jungle throne stands empty.";
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
    }

    public void SetSelectedScene(ScenesToLoad newScene)
    {
        selectedScene = newScene;
        if (!_skipCheckingTutorialCompleted)
        {
            var tutorialManager = FindObjectOfType<TutorialManager>();
            if (tutorialManager != null)
            {
                _isTutorialCompleted = tutorialManager.GetIsTutorialCompleted();
            }

            if (!_isTutorialCompleted)
            {
                ShowWelcomeText("Feeling brave? The tutorial might save your life!", true);
                return;
            }
        }

        switch (selectedScene)
        {
            case ScenesToLoad.TigerFightScene:
                ShowWelcomeText("Enter the Tiger's Domain? Only the strongest survive...", true);
                break;

            case ScenesToLoad.BullFightScene:
                //ShowWelcomeText("You're entering the zone of the Bull. Do you want to proceed?", true);
                ShowWelcomeText("Enter the Bull's Arena? Hope you're quick on your feet...", true);

                break;

            case ScenesToLoad.NotEnteringScene:
                //ShowWelcomeText("You're not qualified enough to enter this fight. Please turn back.", false);
                ShowWelcomeText("This door remains sealed... your journey is not yet complete.", false);

                break;
        }
    }

    public void ShowWelcomeText(string message, bool showYesButton, bool showOkButton = false)
    {
        if (welcomeTextField != null)
        {
            welcomeTextField.text = message;
        }

        if (yesButton != null)
        {
            noButton.gameObject.SetActive(true);
            yesButton.gameObject.SetActive(showYesButton);
            okButton.gameObject.SetActive(showOkButton);
        }

        if (showOkButton == true)
        {
            okButton.gameObject.SetActive(showOkButton);
            yesButton.gameObject.SetActive(false);
            noButton.gameObject.SetActive(false);
        }

        EnableDoorWelcomeText();
    }

    private void ResetSkipCheckingTutorialCompleted()
    {
        _skipCheckingTutorialCompleted = false;
    }

    public void LoadSelectedScene()
    {
        if (!_skipCheckingTutorialCompleted && !_isTutorialCompleted)
        {
            _skipCheckingTutorialCompleted = true;
            SetSelectedScene(selectedScene);
            return;
        }

        switch (selectedScene)
        {
            case ScenesToLoad.TigerFightScene:
                SceneManager.LoadScene("LastBanana_TigerFight");
                break;

            case ScenesToLoad.BullFightScene:
                SceneManager.LoadScene("LastBanana_BullFight");
                break;

            case ScenesToLoad.NotEnteringScene:
                // Handle the case if needed
                break;
        }
    }

    public void EnablePlayerAttackAndPlayerControls()
    {
        var player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.EnableControls();
        }

        var lineRendererDrawer = FindObjectOfType<LineRendererDrawer>();
        if (lineRendererDrawer != null)
        {
            lineRendererDrawer.EnableAttacks();
        }

        ResetSkipCheckingTutorialCompleted();
    }

    private void EnableDoorWelcomeText()
    {
        if (doorWelcomeText != null)
        {
            doorWelcomeText.SetActive(true);
        }
    }
}