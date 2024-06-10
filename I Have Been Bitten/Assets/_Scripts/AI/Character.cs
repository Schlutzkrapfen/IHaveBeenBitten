using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(Rigidbody),typeof(NavMeshAgent),typeof(Animator))]
public class Character : MonoBehaviour
{
    protected Animator animator;
    protected NavMeshAgent agent;
    protected CharacterStateMachine controller;
    protected Rigidbody rb;
    protected AudioSource audioSource;

    [SerializeField] protected float detectionRadius;
    [SerializeField] protected LayerMask targetLayer;

    [SerializeField]protected Transform target;
    public bool DebugCharacter = false;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        controller = new CharacterStateMachine();
        audioSource = GetComponent<AudioSource>();
    }
    protected virtual void Start()
    {

    }
    
    public bool IsAnimationFinished()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0))
        {
            return true;
        }
        return false;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        rb.angularVelocity = Vector3.zero;
    }
    protected virtual void OnDrawGizmos()
    {
   
        if(agent!=null && agent.hasPath)
        {
            for (var i = 0; i < agent.path.corners.Length-1; i++) 
            {
                Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.cyan);
            }
        }
    }
    public Transform FindTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, detectionRadius, targetLayer);

        Transform closestTarget = null;
        Transform player = null;
        float closestDistance = float.MaxValue;

        foreach (Collider target in targets)
        {
            Transform targetTransform = target.transform;

            if (targetTransform.tag == "Player")
            {
                player = targetTransform;
                continue; // Skip player for now
            }

            float distance = Vector3.Distance(targetTransform.position, transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = targetTransform;
            }
        }

        // If no non-player targets are found, return the player if it exists
        return closestTarget ?? player;
    }
    public float GetDistanceFromTarget()
    {
        if (target == null) return float.MaxValue;

        return Vector3.Distance(transform.position, target.position);
    }

    public virtual void ConvertToZombie()
    {
        
    }

    #region Getters
    public Animator GetAnimator() { return animator; }
    public NavMeshAgent GetAgent() { return agent; }
    public Transform GetTarget() { return target; }
    public Rigidbody GetRigidBody() { return rb; }
    public AudioSource GetAudioSource() { return audioSource; }

    public CharacterStateMachine GetController()
    {
        return controller;
        
    }
    #endregion
}
