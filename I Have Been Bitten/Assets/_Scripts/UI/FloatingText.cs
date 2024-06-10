using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public Transform mainCamera;
    public Transform objectPosition;
    public Transform worldSpaceCanvas;
    public TextMeshProUGUI text;

    public Vector3 offset;

    private void Start()
    {
        mainCamera = Camera.main.transform;
        objectPosition = transform.parent;
        worldSpaceCanvas = GameObject.Find("WorldCanvas").transform;
        text = GetComponent<TextMeshProUGUI>();
        transform.SetParent(worldSpaceCanvas);
        
    }
    
   private void Update()
   {
        if (GameObject.FindGameObjectWithTag("Human") && text.gameObject.activeInHierarchy)
        {
           transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
           transform.position = objectPosition.position + offset; 
        }
        if (!GameObject.FindGameObjectWithTag("Human") && text.gameObject.activeInHierarchy)
        {
            text.gameObject.SetActive(false);
        }
   }

}
