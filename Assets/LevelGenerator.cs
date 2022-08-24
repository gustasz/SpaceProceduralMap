using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    int nodeWidth = 10;
    int nodeHeight = 5;
    float maxX = 200f;
    float maxY = 100f;
    float startX = 0f;
    float startY = 0f;
    float connectLength = 30f;
    public GameObject nodePrefab;
    public GameObject nodeSelectorPrefab;
    // Start is called before the first frame update
    void Start()
    {
        float xMove = (maxX - startX) / nodeWidth;
        float yMove = (maxY - startY) / nodeHeight;
        float x = startX + xMove / 2;
        float y = startY + yMove / 2;
        float xRandom = 0;
        float yRandom = 0;
        List<GameObject> allNodes = new();
        for (int i = 0; i < nodeWidth; i++)
        {
            y = startY + yMove / 2;
            for (int j = 0; j < nodeHeight; j++)
            {

                xRandom = Random.Range(-xMove / 2 + 1, xMove / 2 - 1);
                yRandom = Random.Range(-yMove / 2 + 1, yMove / 2 - 1);
                var currentNode = Instantiate(nodePrefab, new Vector3(x + xRandom, y + yRandom, 0), Quaternion.identity);
                allNodes.Add(currentNode);
                y += yMove;
            }
            x += xMove;
        }

        for (int o = 0; o < allNodes.Count; o++)
        {
            for (int p = 0; p < allNodes.Count; p++)
            {
                if (o != p)
                {
                    var firstNode = allNodes[o];
                    var secondNode = allNodes[p];
                    if (Vector2.Distance(firstNode.transform.position, secondNode.transform.position) <= connectLength)
                    {
                        var firstPoint = firstNode.GetComponent<NodeScript>();
                        var secondPoint = secondNode.GetComponent<NodeScript>();
                        if (!firstPoint.AdjacentNodes.Contains(secondNode))
                        {
                            firstPoint.AdjacentNodes.Add(secondNode);
                            secondPoint.AdjacentNodes.Add(firstNode);
                        }
                    }

                }
            }
        }
        var nodeSelector = Instantiate(nodeSelectorPrefab);

        var iN = Random.Range(0, nodeHeight); // select a random node from the first row as the starting node

        nodeSelector.GetComponent<NodeSelectorScript>().CurrentNode = allNodes[iN];
        allNodes[iN].GetComponent<NodeScript>().IsActive = true;
        allNodes[iN].GetComponent<SpriteRenderer>().color = Color.blue;
    }

    //for(int i = 0; i < nodeWidth; i++)
    //Instantiate(nodePrefab,new Vector3())
}