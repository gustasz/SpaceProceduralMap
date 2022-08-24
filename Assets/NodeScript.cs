using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeScript : MonoBehaviour
{
    public List<GameObject> AdjacentNodes;
    public bool IsActive = false;

    Color mainColor = Color.yellow;
    Color hoverColor = Color.gray;

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

        lineRend.enabled = false;
    }

    void DrawLineBetweenObjects(Transform firstT, Transform secondT, int i)
    {
        lineRend.SetPosition(i, firstT.position);
        lineRend.SetPosition(i+1, secondT.position);
    }

    private void OnMouseOver()
    {
        if (!IsActive)
        {
            lineRend.enabled = true;
            lineRend.startColor = hoverColor;
            lineRend.endColor = hoverColor;
            lineRend.sortingOrder = -2;
        }
    }
    private void OnMouseExit()
    {
        if (!IsActive)
        {
            lineRend.enabled = false;
            lineRend.startColor = mainColor;
            lineRend.endColor = mainColor;
            lineRend.sortingOrder = -1;
        }
        }
}
