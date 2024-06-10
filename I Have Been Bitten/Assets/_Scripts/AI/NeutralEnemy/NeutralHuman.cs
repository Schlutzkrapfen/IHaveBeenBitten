using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NeutralHuman : Character
{ 
    public bool isBitten = false;
    public GameObject zombiePrefab;
    [SerializeField,Range(0,100)]
    public int fearPercentage;
    public float fleeDistance = 2;
    public bool ShouldRun = true;
    public AudioClip fearAudioClip;
    public AudioClip convertToZomby;
    public void Zombify()
    {
        isBitten = true;
    }
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        controller.InitController(new NeutralHumanIdle(this,controller,"Idle"));  
    
    }
    private void Update()
    {
        controller.UpdateState();
        if(DebugCharacter)
            Debug.Log(controller.GetState().ToString());
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("ZombieAttack"))
        {
           isBitten=true;
        }
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
    public override void ConvertToZombie()
    {
        isBitten = true;
    }
    

      
}
