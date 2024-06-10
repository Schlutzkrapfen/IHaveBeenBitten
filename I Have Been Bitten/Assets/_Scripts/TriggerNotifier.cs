using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerNotifier : MonoBehaviour
{
    public Tutorial triggerListener;

    private void OnTriggerEnter(Collider other)
    {
        triggerListener.OnTriggerEnter(other);
    }
}
