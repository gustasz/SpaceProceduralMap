using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeScript : MonoBehaviour
{
    public List<GameObject> AdjacentNodes;

    public LineRenderer lineRend;
    void Start()
    {
        lineRend.positionCount = 2;
        Transform first = AdjacentNodes.First().transform;
        Transform second = gameObject.transform;

        DrawLineBetweenObjects(first, second);
    }

    void DrawLineBetweenObjects(Transform firstT, Transform secondT)
    {
        lineRend.SetPosition(0, firstT.position);
        lineRend.SetPosition(1, secondT.position);
    }
}
