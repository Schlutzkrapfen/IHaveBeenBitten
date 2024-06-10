using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HungerTracker : MonoBehaviour
{
    [SerializeField] float maxHunger;
    Volume volume;
    float currentHunger;

    public void SetHunger(float hunger)
    {
        currentHunger = hunger;
    }

    
}
