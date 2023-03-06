using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour, IDataPersistence
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button level1Button;
    [SerializeField] private Button level2Button;

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

    public void OnLevelSelect(int level)
    {
        ToggleButtonState(false);
        Debug.Log("Loading level " + level);
        SceneManager.LoadSceneAsync("Scene" + level);
    }

    public void ToggleButtonState(bool state)
    {
        newGameButton.interactable = state;
        level1Button.interactable = state;
        level2Button.interactable = state;
    }

    private void Start()
    {
        if (!DataPersistanceManager.instance.hasGameData())
        {
            Debug.Log("No data was found, continue button is disabled");
            level1Button.interactable = false;
            level2Button.interactable = false;
        }

    }

    public void LoadData(GameData data)
    {
        if (data == null) {
            level1Button.interactable = false;
            level2Button.interactable = false;
        } else if (data.coinCount == 0)
        {
            level1Button.interactable = false;
            level2Button.interactable = false;
        } else if (data.coinCount >= 5)
        {
            level1Button.interactable = true;
            level2Button.interactable= true;
        } else
        {
            level1Button.interactable = true;
            level2Button.interactable = false;
        }
    }

    public void SaveData(GameData data)
    {

    }
}
