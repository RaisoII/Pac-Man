using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private List<Node> neighbors;
    [SerializeField] private Node portalNode;
    private Vector2[] possibleDirections;

    public void setNeighbor(Node n) => neighbors.Add(n); 
    public void searchDirections()
    {
        possibleDirections = new Vector2[neighbors.Count];

        for(int i = 0; i < neighbors.Count; i++)
        {
            Node neighbor = neighbors[i];
            Vector2 distance = neighbor.transform.position - transform.position;
            possibleDirections[i] = distance.normalized;
        }
    }

    public Vector2[] getPossibleDirections() => possibleDirections;
    public List<Node> getNeightbors() => neighbors; 
    
    public Node getNeightbor(Vector2 dir)
    {
        Node neightbor = null;
        for(int i = 0 ; i < possibleDirections.Length; i++)
        {
            Vector2 direction = possibleDirections[i];
            if(direction == dir)
            {
                neightbor = neighbors[i];
                break;
            }
        }

        return neightbor;
    }

    public Vector2 getDirectionNode(Node node)
    {
        Vector2 dis = (Vector2)node.transform.position - (Vector2)transform.position;
        return dis.normalized;
    }
    
    public Node getPortalNode() => portalNode;
}
