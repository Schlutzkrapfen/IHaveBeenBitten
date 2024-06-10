using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Shooter : Character
{
    #region Public fields
    public float attackDistance = 1.5f;
    public GameObject zombiePrefab;
    public bool isBitten;
    public bool canShoot;
    public float fireRate;
    public bool InstaShoot = false;
    #endregion
    #region Serizalized fields
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject shootVfx;
    [SerializeField] private RigBuilder rigBuilder;
    [SerializeField] private Rig aimRig;
    [SerializeField] private MultiAimConstraint gunAim;
    [SerializeField] private MultiAimConstraint handAim;
    [SerializeField] private MultiAimConstraint spineAim;
    [SerializeField] private float rigLerpDuration;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private List<AudioClip> shootSounds;
    #endregion
    #region Private fields
    private float aimRigWeigth = 0;
    private Transform newTarget;
    private float rigLerpTimer;
    private float fireRateTimer;
    
    #endregion

    public AudioClip convertToZomby;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        controller.InitController(new ShooterIdle(this, controller, "Idle"));
        ClearAimConstrains();
        StartCoroutine("UpdateTarget");
        rigLerpTimer = 0;

        fireRateTimer = fireRate;
    }
    private void Update()
    {

        UpdateRigWeight();

        fireRateTimer -= Time.deltaTime;
        if (fireRateTimer < 0)
            fireRateTimer = 0;


        if (DebugCharacter)
            Debug.DrawRay(firepoint.position, firepoint.forward * attackDistance, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(firepoint.position, firepoint.forward, out hit, attackDistance))
        {
            
            if ((targetLayer & 1 << hit.collider.gameObject.layer) == 1 << hit.collider.gameObject.layer && fireRateTimer <= 0)
            {
                if (DebugCharacter) Debug.Log("Raycast hit: " + hit.collider.gameObject.name + " on layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer));
                canShoot = true;

            }
            else
            {
                canShoot = false;
            }
        }

        //update current state 
        controller.UpdateState();

    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ZombieAttack"))
        {
            isBitten = true;
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

    IEnumerator UpdateTarget()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);

            newTarget = FindTarget();

            if(target == null)
                target = newTarget;
            else if(target != newTarget){
                target = newTarget;
                SetAimContrains(AimToTarget(target, "Neck"));
            }
        }
    }


    #region Shooter utilities
    public void Shoot()
    {
        if (!canShoot) return;

        fireRateTimer = fireRate;


        GameObject go = Instantiate(bulletPrefab);

        go.transform.position = firepoint.position;
        go.GetComponent<Rigidbody>().velocity = firepoint.forward * bulletSpeed;
        GameObject vfx = Instantiate(shootVfx);
        vfx.transform.position = firepoint.position;

        int r = Random.Range(0, shootSounds.Count);

        audioSource.PlayOneShot(shootSounds[r]);
        
        aimRig.weight = 0;
        ResetRigLerpTimer();
    }
    public override void ConvertToZombie()
    {
        isBitten = true;
    }
    public void ResetRigLerpTimer()
    {
        rigLerpTimer = 0;
    }
    public void UpdateRigWeight()
    {
        if (rigLerpTimer < rigLerpDuration)
        {
            rigLerpTimer += Time.deltaTime;
            float t = rigLerpTimer / rigLerpDuration;
            float r = Mathf.Lerp(0, aimRigWeigth, t);
            aimRig.weight = r;
        }
    }
    public void ClearAimConstrains()
    {
        var data = gunAim.data.sourceObjects;
        data.SetTransform(0, null);
        gunAim.data.sourceObjects = data;

        data = handAim.data.sourceObjects;
        data.SetTransform(0, null);
        handAim.data.sourceObjects = data;

        data = spineAim.data.sourceObjects;
        data.SetTransform(0, null);
        spineAim.data.sourceObjects = data;

        rigBuilder.Build();
    }

    public bool CheckConstrains()
    {
        bool ret = true;

        var data = gunAim.data.sourceObjects;
        if (data.GetTransform(0) == null)
            return false;
        data = handAim.data.sourceObjects;
        if (data.GetTransform(0) == null)
            return false;
        data = spineAim.data.sourceObjects;
        if (data.GetTransform(0) == null)
            return false;

        return ret;
    }
    public Transform AimToTarget(Transform target, string objName)
    {
        if (target == null) return null;
        foreach (Transform child in target)
        {
            if (child.name == objName)
            {
                return child;
            }

            Transform result = AimToTarget(child, objName);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    #endregion

    #region Getters
    public Rig GetRig()
    {
        return aimRig;
    }

    public float GetFirerateTimer()
    {
        return fireRateTimer;
    }
    #endregion

    #region Setters
    public void SetAimWeigthRig(float value)
    {
        aimRigWeigth = value;
    }

    public void SetAimContrains(Transform target)
    {
        ClearAimConstrains();
        if (target == null) return;

        var data = gunAim.data.sourceObjects;
        data.SetTransform(0, target);
        gunAim.data.sourceObjects = data;

        data = handAim.data.sourceObjects;
        data.SetTransform(0, target);
        handAim.data.sourceObjects = data;

        data = spineAim.data.sourceObjects;
        data.SetTransform(0, target);
        spineAim.data.sourceObjects = data;


        rigBuilder.Build();
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
