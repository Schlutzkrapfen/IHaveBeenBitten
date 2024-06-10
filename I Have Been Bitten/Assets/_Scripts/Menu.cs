using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GameObject loader = GameObject.Find("LevelLoader");
        GameObject indicator = GameObject.Find("Level_Indicator");

        Destroy(loader);
        Destroy(indicator);
    }

    // Switches Scene into the Game when "Start" is clicked
    public void OnPlayButton()
    {
        if (ASyncLoader.Instance != null)
        {
            ASyncLoader.Instance.LoadLevel("Level_1");
        }
        else
        {
            SceneManager.LoadScene("Level_1");
        }
       
    }

    // Restarts the Game if the "Restart" Button is clicked
    // Sends Player to the Startscreen and destroys Gameobjects to reset values
    public void OnRestartButton()
    {
        if (ASyncLoader.Instance != null)
        {
            ASyncLoader.Instance.LoadLevel(GameManager.Instance.lastLevelSceneName);
        }
        else
        {
            SceneManager.LoadScene(GameManager.Instance.lastLevelSceneName);
        }


    }
    public void OnMainMenu()
    {
        if(ASyncLoader.Instance != null)
        {
            ASyncLoader.Instance.LoadLevel("StartScreen");
        }
        else
        {
            SceneManager.LoadScene("StartScreen");
        }
    }

    // Ends the application if "Quit" is clicked
    public void OnQuitButton()
    {
    #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
    #endif
        Application.Quit();
    }

}
