using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveButtonScript : MonoBehaviour
{
    public void LeaveLevel()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
