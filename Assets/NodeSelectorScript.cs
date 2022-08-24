using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeSelectorScript : MonoBehaviour
{
    public GameObject CurrentNode;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                if(CurrentNode.GetComponent<NodeScript>().AdjacentNodes.Contains(hit.collider.gameObject))
                {
                    CurrentNode.GetComponent<LineRenderer>().enabled = false;
                    CurrentNode.GetComponent<NodeScript>().IsActive = false;
                    CurrentNode = hit.collider.gameObject;
                    CurrentNode.GetComponent<LineRenderer>().enabled = true;
                    CurrentNode.GetComponent<LineRenderer>().startColor = Color.yellow;
                    CurrentNode.GetComponent<LineRenderer>().endColor = Color.yellow;
                    CurrentNode.GetComponent<LineRenderer>().sortingOrder = -1;
                    CurrentNode.GetComponent<NodeScript>().IsActive = true;
                    CurrentNode.GetComponent<SpriteRenderer>().color = Color.blue; // set as visited

                }
                Debug.Log(hit.collider.gameObject.name);
                //hit.collider.attachedRigidbody.AddForce(Vector2.up);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentNode.GetComponent<LineRenderer>().enabled = true;
    }
}
