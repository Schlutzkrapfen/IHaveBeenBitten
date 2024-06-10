using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface ICharacter 
{
    public Animator GetAnimator();
    public NavMeshAgent GetAgent();
}
