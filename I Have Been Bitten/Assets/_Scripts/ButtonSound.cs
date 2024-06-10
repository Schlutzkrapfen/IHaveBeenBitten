using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ButtonSound : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] private AudioClip onHoverSound;
    [SerializeField] private AudioClip onClickSound;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.1f;
    }
    public void OnHover()
    {
        audioSource.PlayOneShot(onHoverSound);
    }

    public void OnClick()
    {
        audioSource.PlayOneShot(onClickSound);
    }
}
