using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour
{
    Node[] nodes;
    void Awake()
    {
        prepareScenery();
    }

    private void prepareScenery()
    {
        GameObject[] nodesObject = GameObject.FindGameObjectsWithTag("Node");
        nodes = new Node[nodesObject.Length];

        for(int i = 0; i < nodesObject.Length; i++)
            nodes[i] = nodesObject[i].GetComponent<Node>();
    }

    public Node getNode(Vector2 pos)
    {
        Node searchNode = null;
        foreach(Node n in nodes)
        {
            if(n.transform.position.Equals(pos))
            {
                searchNode = n;
                break;
            }
        }
        return searchNode;
    }

}
