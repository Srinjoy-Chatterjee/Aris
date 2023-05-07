using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleAINavigator : MonoBehaviour
{

    public bool DrawGizmos = false;

    private Vector3 _ClickPos;
    private NavMeshAgent _Agent;

    private void Awake()
    {
        _Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Camera cam = Camera.main;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0.5f;
            Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
            Vector3 clickDir = (worldPos - cam.transform.position).normalized;

            if (Physics.Raycast(cam.transform.position, clickDir, out RaycastHit hit))
            {
                _ClickPos = hit.point;
                _Agent.SetDestination(hit.point);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!DrawGizmos) return;

        Gizmos.color = Color.yellow * 0.5f;
        Gizmos.DrawSphere(_ClickPos, 0.3f);
    }

}
