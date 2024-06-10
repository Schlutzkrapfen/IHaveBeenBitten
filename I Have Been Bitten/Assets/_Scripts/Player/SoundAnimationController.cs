using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class SoundAnimationController : MonoBehaviour
{
    [SerializeField] private FirstPersonController firstPersonController;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Footstep()
    {
        firstPersonController.FootSteps();
        
    }


}
