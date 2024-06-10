using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DisablePlayerMovement : MonoBehaviour
{

    [SerializeField] public bool SkipInitialAnim;
    [SerializeField] FirstPersonController controller;
    [SerializeField] StarterAssetsInputs assetInput;
    [SerializeField] Interactor interactor;
    [SerializeField] Animation animation;
    [SerializeField] Animator animator;

    private void Start()
    {
        if(SkipInitialAnim)
        {
            GetComponent<Animator>().enabled = false;
        }
        if(GameObject.Find("LevelManager") != null)
        {
            if (Level_Indicator.Instance.GetCurrentLevel() == 1)
            {
                //just let it be
                animator.SetTrigger("Start");
            }
            else
            {
                GetComponent<Animator>().enabled = false;
            }
        }
    }

    public void EnableController()
    {
        controller.enabled = true;
        assetInput.enabled = true;
        interactor.enabled = true;
        animation.enabled = true;
        GetComponent<Animator>().enabled = false;
    }

    public void DisableController()
    {
        controller.enabled = false;
        assetInput.enabled = false;
        interactor.enabled = false;
        animation.enabled = false;
    }
}
