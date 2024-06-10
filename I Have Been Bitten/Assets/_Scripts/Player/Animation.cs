using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Animation : MonoBehaviour
{
    [SerializeField] private Animator anim;
	private StarterAssetsInputs _input;
    public bool attackanimationIsplaying;

    // Start is called before the first frame update
    private void Awake()
    {

    }
    void Start()
    {
	    _input = GetComponent<StarterAssetsInputs>();

        if (Level_Indicator.Instance != null)
        {
            Debug.Log("Current Level " + Level_Indicator.Instance.GetCurrentLevel());
            if (Level_Indicator.Instance.GetCurrentLevel() == 1)
            {
                AnimatorExtender.ChangeStartAnimation(anim, true);
                StartCoroutine(WaitforAnimationToStart(1f));
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    
        
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            AnimatorExtender.PlayAttackAnimation(anim,false);
            attackanimationIsplaying = false;
        }
        else
        {
            attackanimationIsplaying = true;
        }
       


    }

    public void ChangeAttackAnimation(bool newValue)
    {
         attackanimationIsplaying = true;
        AnimatorExtender.PlayAttackAnimation(anim,newValue);
    }

    public void ResetAttackAnimation()
    {
        anim.SetTrigger("RestartAnimation");
    }

    public void ChangeWalkAnimaiton(bool newValue)
    {
        AnimatorExtender.PlayWalkAnimation(anim,newValue);
    }
    IEnumerator WaitforAnimationToStart(float waitTimes)
    {
        yield return new WaitForSeconds(waitTimes);
        AnimatorExtender.ChangeStartAnimation(anim, false);
    }
    }
public static class AnimatorExtender
{
    public static void PlayAttackAnimation(Animator animator, bool naniewvalue)
    {
        animator.SetBool("Attack",naniewvalue);
    }

    public static void PlayWalkAnimation(Animator animator, bool newValue)
    {
        animator.SetBool("Walk", newValue);
    }
    public static void ChangeStartAnimation(Animator animator, bool newValue)
    {
        animator.SetBool("Start", newValue);
}
  
    }
