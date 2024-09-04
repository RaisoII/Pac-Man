using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private Node[] neighbors;
    Vector2[] possibleDirections;
    void Start()
    {
        searchDirections();
    }

    private void searchDirections()
    {
        possibleDirections = new Vector2[neighbors.Length];

        for(int i = 0; i < neighbors.Length; i++)
        {
            Node neighbor = neighbors[i];
            Vector2 distance = neighbor.transform.position - transform.position;
            possibleDirections[i] = distance.normalized;
        }
    }

    public Vector2[] getPossibleDirections() => possibleDirections;
    public Node[] getNeightbors() => neighbors; 
    
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
}
