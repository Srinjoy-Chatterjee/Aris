using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class snap : MonoBehaviour
{
    public RamSpline spline;
    [ContextMenu("snap")]
    public void snapping()
    {
        List<Vector4> points = spline.controlPoints;
        for(int i = 0; i < points.Count; i++)
        {
            Vector3 origin =spline.transform.position+new Vector3(points[i].x, points[i].y, points[i].z) + Vector3.up;
            if(Physics.Raycast(origin,Vector3.down,out RaycastHit hit))
            {
                points[i] = new Vector4(hit.point.x, hit.point.y, hit.point.z, points[i].w);
                Debug.Log(hit.point);
            }
        }
    }
}
