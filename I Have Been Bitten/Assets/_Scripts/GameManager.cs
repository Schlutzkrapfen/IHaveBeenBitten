using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public bool Pause;
    [SerializeField] private GameObject pauseMenu;
    public string lastLevelSceneName;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Optionally, create a new GameObject if instance is null (but typically, the GameManager is pre-placed in the scene)
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(GameManager).ToString());
                    _instance = singletonObject.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Pause = false;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnPause();
        }

        if(Pause)
        {
            Cursor.lockState = Pause ? CursorLockMode.None:CursorLockMode.Locked;
        }

        if(SceneManager.GetActiveScene().name=="Startscreen")
        {
            pauseMenu.SetActive(false);
        }
    }

    public void OnPause()
    {
      
        Pause = !Pause;

        Cursor.visible = Pause;
        pauseMenu.SetActive(Pause);

        if (Pause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
