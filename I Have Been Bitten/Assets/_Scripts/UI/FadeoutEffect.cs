using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FadeoutEffect : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float duration;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    private float timer;

    private void Start()
    {
        timer = 0;
    }
    private void Update()
    {
        if(timer < duration)
        {
            image.color = Color.Lerp(startColor, endColor, timer / duration);

            timer += Time.deltaTime;
            
        }
        else
        {
            gameObject.SetActive(false);
        }
    
    }
}
