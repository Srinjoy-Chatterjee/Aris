using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GetCurrentMoveSpeed : MonoBehaviour
{
    public float CurrentSpeed;
    public NavMeshAgent Agent;

    public float Speed => CurrentSpeed;
    private void Update()
    {
        CurrentSpeed = Agent.velocity.magnitude;
    }
}
