using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerExit : MonoBehaviour
{
    [SerializeField] private string LevelToLoad;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {   
            Level_Indicator.Instance.level++;
            ASyncLoader.Instance.LoadLevel(LevelToLoad);
            GameManager.Instance.lastLevelSceneName = LevelToLoad;
        }
    }

    // Loads Level when called by a Trigger
    internal void LoadLevel(int level)
    {
        switch (level)
        {
            case 1: break; // Scene gets loaded via the StartScreen
            case 2: SceneManager.LoadScene(level);  break;
            case 3: SceneManager.LoadScene(level);  break;
            case 4: SceneManager.LoadScene(level);  break;
            case 5: SceneManager.LoadScene(level); break;
            default: SceneManager.LoadScene(0); break;
        }
    }
}
