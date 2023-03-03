using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;

    public void OnNewGameClicked()
    {
        ToggleButtonState(false);
        Debug.Log("new game");
        //New game function from Manager
        DataPersistanceManager.instance.NewGame();
        //Load the game scene
        SceneManager.LoadSceneAsync("Scene1");
    }

    public void OnContinueGameClicked()
    {
        ToggleButtonState(false);
        Debug.Log("continue game");
        //Load the game scene which will also load the save data because
        //onSceneLoad method in DataPersistanceManager
        SceneManager.LoadSceneAsync("Scene1");
    }

    public void ToggleButtonState(bool state)
    {
        newGameButton.interactable = state;
        continueGameButton.interactable = state;
    }
}
