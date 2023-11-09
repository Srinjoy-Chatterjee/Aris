using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiTest : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent Agent;
    // Start is called before the first frame update
    private void Awake()
    {
        if (!Agent.isOnNavMesh)
        {
            Start();
            Agent.enabled = false;
            Agent.enabled = true;
        }
    }
    void Start()
    {
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 100, 1))
        {
            transform.position = hit.position;
        }
        else
        {
            print("no sample");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
