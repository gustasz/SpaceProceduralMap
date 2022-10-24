using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class NodePositionGenerator : MonoBehaviour
{
    // map where nodes are generated coordinates
    private float startX = 1f;
    private float startY = 0f;
    private float maxX = 200f;
    private float maxY = 100f;

    //map divided into boxes by the amount of height and width
    private int nodeHeight = 5;
    private int nodeWidth = 10;

    private List<GameObject> allNodes = new();
    private GameObject startingNode;
    private GameObject endingNode;

    public GameObject nodePrefab;
    public GameObject nodeSelectorPrefab;

    public GameObject PathDisplay;

    public List<GameObject> path = new();

    void Start()
    {
        RedrawOnClick();
    }

    public void RedrawOnClick()
    {
        foreach(var node in allNodes)
        {
            Destroy(node);
        }
        allNodes = new();
        path = new();

        GenerateAndConnectToClosestNodes();
        ConnectNodeToOtherCloserOnes();
        SelectStartingAndEndingNode();
        PathFindUsingBreadthFirstSearch();

        foreach (var p in path)
        {
            if (p != startingNode && p != endingNode)
            {
                p.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
    }

    private void GenerateAndConnectToClosestNodes()
    {
        float xMove = (maxX - startX) / nodeWidth;
        float yMove = (maxY - startY) / nodeHeight;
        float x = startX + xMove / 2;
        float y = startY + yMove / 2;

        for (int i = 0; i < nodeWidth; i++)
        {
            y = startY + yMove / 2;
            for (int j = 0; j < nodeHeight; j++)
            {
                int disablePosition = Random.Range(1, 3);

                if (disablePosition is 1)
                {
                    float xRandom = Random.Range(-xMove / 2 + 1, xMove / 2 - 1);
                    float yRandom = Random.Range(-yMove / 2 + 1, yMove / 2 - 1);
                    //xRandom = 0;
                    //yRandom = 0;
                    var currentNode = Instantiate(nodePrefab, new Vector3(x + xRandom, y + yRandom, 0), Quaternion.identity);
                    Debug.Log($"x={x} y={y}");

                    if (allNodes.Count is not 0)
                    {
                        float shortestDistance = float.MaxValue;
                        int closestNodeIndex = -1;
                        for (int o = 0; o < allNodes.Count; o++)
                        {
                            Vector2 curNodePosition = new Vector2(currentNode.transform.position.x, currentNode.transform.position.y);
                            Vector2 comparingNodePosition = new Vector2(allNodes[o].transform.position.x, allNodes[o].transform.position.y);

                            var distance = Vector2.Distance(curNodePosition, comparingNodePosition);
                            if (distance < shortestDistance)
                            {
                                closestNodeIndex = o;
                                shortestDistance = distance;
                            }
                        }

                        currentNode.GetComponent<NodeScript>().AdjacentNodes.Add(allNodes[closestNodeIndex]);
                        allNodes[closestNodeIndex].GetComponent<NodeScript>().AdjacentNodes.Add(currentNode);
                    }
                    allNodes.Add(currentNode);
                }
                y += yMove;
            }
            x += xMove;
        }
    }

    private void ConnectNodeToOtherCloserOnes()
    {
        //get longest distance
        for (int k = 0; k < allNodes.Count; k++)
        {
            var connectedNodes = allNodes[k].GetComponent<NodeScript>().AdjacentNodes;

            float longestDistance = 0f;
            int longestNodeIndex = -1;
            foreach (var node in connectedNodes)
            {
                Vector2 curNodePosition = new Vector2(node.transform.position.x, node.transform.position.y);
                Vector2 comparingNodePosition = new Vector2(allNodes[k].transform.position.x, allNodes[k].transform.position.y);

                var distance = Vector2.Distance(curNodePosition, comparingNodePosition);
                if (distance > longestDistance)
                {
                    longestNodeIndex = k;
                    longestDistance = distance;
                }
            }

            foreach (var aNode in allNodes)
            {
                Vector2 curNodePosition = new Vector2(aNode.transform.position.x, aNode.transform.position.y);
                Vector2 comparingNodePosition = new Vector2(allNodes[k].transform.position.x, allNodes[k].transform.position.y);

                var distance = Vector2.Distance(curNodePosition, comparingNodePosition);
                Debug.Log($"{longestDistance} >= {distance}");
                if (longestDistance/10*9 >= distance)
                {
                    if (!allNodes[k].GetComponent<NodeScript>().AdjacentNodes.Contains(aNode))
                    {
                        allNodes[k].GetComponent<NodeScript>().AdjacentNodes.Add(aNode);
                        aNode.GetComponent<NodeScript>().AdjacentNodes.Add(allNodes[k]);
                    }
                }
            }
        }
    }

    private void SelectStartingAndEndingNode()
    {
        var nodeSelector = Instantiate(nodeSelectorPrefab);

        float smallestX = maxX;
        float biggestX = startX;
        foreach (var node in allNodes)
        {
            if (node.transform.position.x < smallestX)
            {
                smallestX = node.transform.position.x;
                startingNode = node;
            }

            if (node.transform.position.x > biggestX)
            {
                biggestX = node.transform.position.x;
                endingNode = node;
            }
        }

        nodeSelector.GetComponent<NodeSelectorScript>().CurrentNode = startingNode;
        startingNode.GetComponent<NodeScript>().IsActive = true;
        startingNode.GetComponent<SpriteRenderer>().color = Color.blue;

        endingNode.GetComponent<SpriteRenderer>().color = Color.red;
    }

    private void PathFindUsingBreadthFirstSearch()
    {
        GameObject head = startingNode;
        GameObject tail = endingNode;
        GameObject current;
        List<GameObject> frontier = new();

        frontier.Add(head);
        Dictionary<GameObject, GameObject> cameFrom = new(); // path A->B is stored as cameFrom[B] == A
        cameFrom[head] = null;

        while (frontier.Count is not 0)
        {
            current = frontier.First();
            frontier.Remove(current);
            foreach (var next in current.GetComponent<NodeScript>().AdjacentNodes)
            {
                if (!cameFrom.ContainsKey(next))
                {
                    frontier.Add(next);
                    cameFrom[next] = current;
                }
            }
        }

        current = tail;
        while (current != head)
        {
            path.Add(current);
            current = cameFrom[current];
        }
        path.Add(head);
        path.Reverse();

        PathDisplay.GetComponent<TextMeshProUGUI>().text = path.Count.ToString();
    }
}
