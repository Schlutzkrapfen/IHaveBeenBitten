using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float force;
    [SerializeField] float lifeTime  = 2.0f;
    float timer;
    private void Start()
    {
        timer = lifeTime;
        rb = GetComponent<Rigidbody>();
        //rb.AddForce(-transform.forward * force, ForceMode.Impulse);
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleCollisions(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleCollisions(collision.gameObject);
    }

    private void HandleCollisions(GameObject go)
    {
        if(go.gameObject.layer == LayerMask.NameToLayer("Zombie"))
        {
            if (go.gameObject.CompareTag("Player"))
            {
                SceneManager.LoadScene("EndScreen");
            }
            ragdollEnabler ragdoll;
            if (go.gameObject.TryGetComponent<ragdollEnabler>(out ragdoll))
            {
                ragdoll.EnableRagdoll();
            }
        }
        if (go.gameObject.layer == LayerMask.NameToLayer("ZombieAttack")) return;

        Destroy(gameObject);
    }
}
