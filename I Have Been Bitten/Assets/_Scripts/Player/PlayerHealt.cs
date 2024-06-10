using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerHealt : MonoBehaviour
{
    [SerializeField] private float healt = 3;

    [SerializeField] private Slider healtbar; 
    // Start is called before the first frame update
    void Start()
    {
        
        //TestCase(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveHealt(float amount)
    {
        healt -= amount;
        healtbar.value = healt;
        if(healt <= 0)
        {
            GameOver();        
        }
    }

    //TODO: make more than destroying the GameObject and make some camera effekt 
    private void GameOver()
    {
        Destroy(this);
    }

    private void TestCase(float amount)
    {
        RemoveHealt(amount);
    }
}
