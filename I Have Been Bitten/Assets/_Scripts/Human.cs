using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Human : Interactable
{
    private Character character; 
    private void Start()
    {
        character = GetComponent<Character>();
    }

    protected override void BaseInteract()
    {
        if (character != null)
        {
            character.ConvertToZombie(); 
            Debug.Log("Human Eaten");
        }

       
    }
}

