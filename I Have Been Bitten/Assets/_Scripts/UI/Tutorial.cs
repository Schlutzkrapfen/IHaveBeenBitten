using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject triggerBox;
    public Image papernote;
    public float waitTime;
    public float transitionTime;

    private Animator papernote_Animator;

    private void Awake()
    {
        papernote_Animator = papernote.GetComponent<Animator>();
        papernote_Animator.enabled = false;
    }

    private void Start()
    {
        StartCoroutine(WaitBeforeAnimation());
    }

    private IEnumerator WaitBeforeAnimation()
    {
        yield return new WaitForSeconds(waitTime);

        papernote_Animator.enabled = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        papernote_Animator.SetTrigger("Transition");
    }
}

