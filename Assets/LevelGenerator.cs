using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    int nodeCount = 25;
    int nodeWidth = 10;
    int nodeHeight = 5;
    float maxX = 200f;
    float maxY = 100f;
    float startX = 0f;
    float startY = 0f;
    public GameObject nodePrefab;
    // Start is called before the first frame update
    void Start()
    {

        float x = 0;
        float y = 0;
        GameObject oldNode = null;
        GameObject node;
        List<GameObject> allNodes = new();

        float startXMove = (maxX - startX) / nodeWidth;
        float startYMove = (maxY - startY) / nodeHeight;
        float xMove = startXMove;
        float yMove = startYMove;

        float yAlign = startYMove / 2;

        Debug.Log(startXMove);
        Debug.Log(startYMove);
        y = startY;
        x = startX;

        List<int> selectedIds = new();

        //node = Instantiate(nodePrefab, new Vector3(x, y, 0), Quaternion.identity);
        //allNodes.Add(node);
        for (int k = 0; k < nodeWidth; k++)
        {
            selectedIds = GetDifferentIntList(0, nodeHeight, 2);

            for (int i = 0; i < selectedIds.Count; i++)
            {
                Debug.Log($"{selectedIds[0]} + {selectedIds[1]}");
                Debug.Log($"y = {selectedIds[i]} * {startYMove}");
                y = (selectedIds[i] + 1) * startYMove;
                node = Instantiate(nodePrefab, new Vector3(x + startXMove / 2, y - yAlign, 0), Quaternion.identity);

                if (allNodes.Count != 0)
                {
                    int closestIndex = 0;
                    float shortestDistance = Vector2.Distance(allNodes[0].transform.position, node.transform.position);
                    for (int m = 0; m < allNodes.Count; m++)
                    {
                        var dist = Vector2.Distance(allNodes[m].transform.position, node.transform.position);
                        if (dist < shortestDistance)
                        {
                            closestIndex = m;
                            shortestDistance = dist;
                        }
                    }
                    var randomNode = allNodes[closestIndex];
                    //var randomNode = allNodes.Last();
                    var randomScript = randomNode.GetComponent<NodeScript>();
                    randomScript.AdjacentNodes.Add(node);
                    var script = node.GetComponent<NodeScript>();
                    script.AdjacentNodes.Add(randomNode);
                    oldNode = node;
                }
                allNodes.Add(node);
            }
            selectedIds = new();
            x += startXMove;

            /*for(int j = 0; j < nodeHeight; j++)
            {
                y += startYMove;
                    node = Instantiate(nodePrefab, new Vector3(x + startXMove / 2, y - startYMove / 2, 0), Quaternion.identity);

                    int closestIndex = 0;
                    float shortestDistance = Vector2.Distance(allNodes[0].transform.position, node.transform.position);
                    for (int m = 0; m < allNodes.Count; m++)
                    {
                        var dist = Vector2.Distance(allNodes[m].transform.position, node.transform.position);
                        if (dist < shortestDistance)
                        {
                            closestIndex = m;
                            shortestDistance = dist;
                        }
                    }
                    var randomNode = allNodes[closestIndex];
                    //var randomNode = allNodes.Last();
                    var randomScript = randomNode.GetComponent<NodeScript>();
                    randomScript.AdjacentNodes.Add(node);
                    var script = node.GetComponent<NodeScript>();
                    script.AdjacentNodes.Add(randomNode);
                    oldNode = node;
                    allNodes.Add(node);
            }
            x += startXMove;
            y = startY;
        }

        /*for(int l = 0; l < allNodes.Count; l++)
        {
            foreach (var n in allNodes)
            {
                Vector2 point = new Vector2(allNodes[l].transform.position.x, allNodes[l].transform.position.y);
                Vector2 nPoint = new Vector2(n.transform.position.x, n.transform.position.y);
                if(point != nPoint && Vector2.Distance(point,nPoint) < 30)
                {
                    var randomScript = n.GetComponent<NodeScript>();
                    randomScript.AdjacentNodes.Add(n);
                    var script = allNodes[l].GetComponent<NodeScript>();
                    script.AdjacentNodes.Add(allNodes[l]);
                }
            }
        }*/


            /*int moveBy = ((int)maxX - (int)startX) / nodeCount / 2;

            x = Random.Range(startX, startX + 10);
            y = Random.Range(startY, maxY);
            node = Instantiate(nodePrefab, new Vector3(x, y, 0), Quaternion.identity);
            allNodes.Add(node);
            oldNode = node;

            for (int i = 0; i < nodeCount-1; i++)
            {
                Vector2 point;
                Vector2 oldPoint = new Vector2(oldNode.transform.position.x, oldNode.transform.position.y);
                do
                {
                    x = Random.Range(startX, maxX);
                    y = Random.Range(startY, maxY);
                    point = new Vector2(x, y);
                } while (Vector2.Distance(oldPoint, point) > 50 || Vector2.Distance(oldPoint, point) < 10);
                //int nodeIndex = Random.Range(0, allNodes.Count);
                //var randomNode = allNodes[nodeIndex];
                node = Instantiate(nodePrefab, new Vector3(x, y, 0), Quaternion.identity);
                int closestIndex = 0;
                float shortestDistance = Vector2.Distance(allNodes[0].transform.position, node.transform.position);
                for (int k = 0; k < allNodes.Count; k++)
                {
                    var dist = Vector2.Distance(allNodes[k].transform.position, node.transform.position);
                    if (dist < shortestDistance)
                    {
                        closestIndex = k;
                        shortestDistance = dist;
                    }
                }
                var randomNode = allNodes[closestIndex];
                var randomScript = randomNode.GetComponent<NodeScript>();
                randomScript.AdjacentNodes.Add(node);
                var script = node.GetComponent<NodeScript>();
                script.AdjacentNodes.Add(randomNode);
                oldNode = node;
                allNodes.Add(node);
                startX += moveBy;
                //startX = x;
                Debug.Log(startX);*/
        }

        List<int> GetDifferentIntList(int min, int max, int count)
        {

            if (max - min < count)
            {
                return null;
            }

            List<int> selectedIds = new();

            int first = Random.Range(min, max);
            selectedIds.Add(first);


            int temp;
            for (int i = 0; i < count-1; i++)
            {
                do
                {
                    temp = Random.Range(min, max);
                } while (selectedIds.Contains(temp));
                selectedIds.Add(temp);
            }
            return selectedIds;
        }
    }
}
