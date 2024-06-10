using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Indicator : MonoBehaviour
{
    public static Level_Indicator Instance;

    public int level = 1;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public int GetCurrentLevel()
    {
        return level;
    }
}
