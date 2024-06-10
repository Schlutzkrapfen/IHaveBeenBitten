using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trigger : MonoBehaviour
{


    public List<Transform> collisions;
    public List<Interactable> Interactors;
    private int targetLayer = 6;

    private int zombieLayer = 7;

  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == targetLayer)
        {
            if (other.GameObject().GetComponent<Interactable>())
            {
                Interactors.Add(other.gameObject.GetComponent<Interactable>());
            }
            collisions.Add(other.gameObject.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == targetLayer|| other.gameObject.layer == zombieLayer
          )
        {
            collisions.Remove(other.gameObject.transform); 
            if (other.GameObject().GetComponent<Interactable>())
            {
                Interactors.Remove(other.gameObject.GetComponent<Interactable>());
            }
        }
    }
}
