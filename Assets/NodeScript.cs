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
        lineRend.positionCount = AdjacentNodes.Count * 2;

        int line = 0;

        foreach (var node in AdjacentNodes)
        {
            Transform first = gameObject.transform;
            Transform second = node.transform;

            DrawLineBetweenObjects(first, second,line);
            line += 2;
        }
    }

    void DrawLineBetweenObjects(Transform firstT, Transform secondT, int i)
    {
        lineRend.SetPosition(i, firstT.position);
        lineRend.SetPosition(i+1, secondT.position);
    }
}
