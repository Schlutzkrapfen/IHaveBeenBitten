using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ragdollEnabler : MonoBehaviour
{
    private Animator animator;
    private Rigidbody mainRigidbody;
    private CapsuleCollider capsuleCollider;
    [SerializeField] private SphereCollider arm;

    [SerializeField] private Transform ragdollRoot;

    [SerializeField] private bool startRagdool = false;
    [SerializeField] private AudioClip deathSound;
    AudioSource audioSource;
	[Tooltip("what the minmal Pitch is that can randomly be generated")]
		[SerializeField]private float minPitch = 0.9f;
		[Tooltip("what the maximal Pitch is that can randomly be generated")]
		[SerializeField]private float maxPitch = 1.1f;
    private Rigidbody[] rigidbodies;

    private CharacterJoint[] joints;

    private Collider[] colliders;

    public int zombieDeadLayer = 0;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbodies = ragdollRoot.GetComponentsInChildren<Rigidbody>();
        joints = ragdollRoot.GetComponentsInChildren<CharacterJoint>();
        colliders = ragdollRoot.GetComponentsInChildren<Collider>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        mainRigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        if (startRagdool)
        {
            
            EnableRagdoll();
        }
        else
        {
            EnableAnimator();
        }

    }

    public void EnableRagdoll()
    {
        
        animator.enabled = false;
        
        foreach (CharacterJoint joint in joints)
        {
            joint.enableCollision = true;
        }

        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.velocity = Vector3.zero;

            rigidbody.detectCollisions =true ;
            rigidbody.useGravity = true;
        }
        mainRigidbody.detectCollisions =false ;
        mainRigidbody.useGravity = false;
        capsuleCollider.enabled = false;
        if (arm)
        {
            
            arm.enabled = false;
        }

        this.GameObject().layer = zombieDeadLayer;
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = zombieDeadLayer; // Set layer for each child game object
        }

        if (audioSource!= null && deathSound != null)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(deathSound);
        }

    }

    public void EnableAnimator()
    {
        animator.enabled = true;
        foreach (CharacterJoint joint in joints)
        {
            joint.enableCollision = false;
        }

        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        foreach (Rigidbody rigidbody in rigidbodies)
        {

            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
        }
    }
    // Update is called once per frame
   
}
