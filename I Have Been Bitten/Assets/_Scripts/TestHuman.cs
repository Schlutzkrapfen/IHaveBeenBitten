using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestHuman : Interactable
{
    [SerializeField]
    Interactor Zombie;


    protected override void BaseInteract()
    {
        Debug.Log("light picked up");
        
    }
}
