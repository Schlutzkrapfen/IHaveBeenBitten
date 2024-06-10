using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public void Interact()
    {
        BaseInteract();
    }
    protected abstract void BaseInteract();
}

