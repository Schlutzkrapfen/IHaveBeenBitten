using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ASyncLoader : MonoBehaviour
{

    public static ASyncLoader Instance
    {
        get { return instance; }
    }
    private static ASyncLoader instance;

    [Header("Menu screens")]
    [SerializeField] private GameObject loadingScreen;

    [Header("Slider")]
    [SerializeField] private Slider  loadSlider;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void LoadLevel(string levelToLoad)
    {
        loadingScreen.SetActive(true);
        StartCoroutine(LoadLevelAsync(levelToLoad));
    }


    IEnumerator LoadLevelAsync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
            float progessValue = Mathf.Clamp01(loadOperation.progress / 0.9f);

            loadSlider.value = progessValue;
            yield return null;
        }
    }
}
