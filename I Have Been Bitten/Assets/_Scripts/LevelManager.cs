using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    public TextMeshProUGUI humansAlive_Text;
    public int humansAlive;
    public GameObject playerPrefab;
    
    private Vector3 playerSpawn;
    internal GameObject[] humanSpawns;
    private int level = 1;
    private GameObject exit;
    private GameObject exitDoor;
    private bool exitActive = false;

    internal GameObject player;

    private void Awake()
    {
        SceneManager.sceneLoaded += LoadObjects;
    }
    private void Start()
    {
       
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LoadObjects;
    }

    private void Update()
    {
        CheckHumans();
    }

    void LoadObjects(Scene scene, LoadSceneMode mode)
    {
        if(scene.isLoaded) 
        {
            exit = GameObject.FindGameObjectWithTag("Exit");
            exitDoor = GameObject.FindGameObjectWithTag("Door");
            // playerSpawn = GameObject.Find("PlayerSpawn").transform.position;
            
             GameObject humans = GameObject.Find("LevelManager/Humans");
             GameObject player = GameObject.Find("PlayerCameraRoot");        
           
             humanSpawns = new GameObject[humans.transform.childCount];

             for (int i = 0; i < humans.transform.childCount; i++)
             {
                 humanSpawns[i] = humans.transform.GetChild(i).gameObject;
                 humansAlive++;
             }
            
            SetExitAndText();
        }

    }
    private void SetExitAndText()
    {   
        exitDoor.GetComponent<Animator>().enabled = false;
        exit.SetActive(false);
        exitActive = false;        

        humansAlive_Text.text = humansAlive.ToString();
    }
        
    private void CheckHumans()
    {
        if (!exitActive)
        {
            GameObject zombie = GameObject.FindGameObjectWithTag("Zombie");

            if (zombie)
            {
                if (humansAlive > 0)
                {
                    humansAlive--;
                }
                humansAlive_Text.text = humansAlive.ToString();
                zombie.tag = "Untagged";
            }
            
            if (humansAlive == 0)
            {
                exit.SetActive(true);
                exitActive = true;
                exitDoor.GetComponent<Animator>().enabled = true;
                exitDoor.GetComponent<AudioSource>().Play();
               
            }
            
        }
        
    }
    public int GetLevel()
    {
        return level;
    }

}
