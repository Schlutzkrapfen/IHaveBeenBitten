using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class Zombie : Character
{
    public float attackDistance = 1.5f;
    public GameObject attackObj;
    public AudioClip chaseSound;
    public AudioClip attackSound;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();   
        agent.enabled = true;
        controller.InitController(new ZombieIdle(this, controller, "Idle"));
        DisableAttack();
        StartCoroutine("UpdateTarget");
    }
    private void Update()
    {  
        rb.velocity = Vector3.zero;
        //update current state 
        controller.UpdateState();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
    IEnumerator UpdateTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            target = FindTarget();
        }
    }

    public void EnableAttack()
    {
       attackObj.SetActive(true);
    }
    public void DisableAttack()
    {
        attackObj.SetActive(false);
    }
}
