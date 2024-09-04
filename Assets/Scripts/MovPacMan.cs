using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovPacMan : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private Grid gridGame;
    private Vector2 direction;
    private Vector2 nextDirection;
    private Node actualNode,previousNode,targetNode;
    // Start is called before the first frame update
    void Start()
    {
        GameObject scripts = GameObject.Find("GeneralScripts");
    
        gridGame = scripts.GetComponent<Grid>();
        actualNode = gridGame.getNode(transform.position);
        Debug.Log(actualNode.transform.position);
        direction = Vector2.left;
        changedPosition(direction);
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
        move();
    }

    private void checkInput()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            changedPosition(Vector2.up);
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            changedPosition(Vector2.down);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            changedPosition(Vector2.right);
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            changedPosition(Vector2.left);
        }
    }

    private Node canMove(Vector2 dir)
    {
        Node moveToNode  = actualNode.getNeightbor(dir); 
        return moveToNode;
    }

    private void changedPosition(Vector2 dir)
    {
        if(dir != direction)
            nextDirection = dir;

        if(actualNode != null)
        {
            Node moveToNode = canMove(dir);
            
            if(moveToNode != null)
            {
                direction = dir;
                targetNode = moveToNode;
                previousNode = actualNode;
                actualNode = null;
            }
        }
    }

    private void move()
    {
        if(targetNode !=  actualNode  && targetNode != null)
        {
            if(nextDirection == direction*-1)
            {
                direction *= -1;
                Node tempNode = targetNode;
                targetNode = previousNode;
                previousNode = tempNode;
            }

            if(onTarget())
            {
                actualNode = targetNode;
                transform.localPosition = actualNode.transform.position;
                Node moveToNode = canMove(nextDirection);
                if(moveToNode != null)
                    direction = nextDirection;
                
                if(moveToNode == null)
                    moveToNode = canMove(direction);

                if(moveToNode != null)
                {
                    targetNode = moveToNode;
                    previousNode = actualNode;
                    actualNode = null;
                }
                else
                    direction = Vector2.zero;
            }
            else
                transform.localPosition += (Vector3) direction * speed * Time.deltaTime;
        }
    }

    bool onTarget()
    {
        float nodeTarget = distanceForNode(targetNode.transform.position);
        float nodeToSelf = distanceForNode(transform.localPosition);
        return nodeToSelf > nodeTarget;
    }

    private float distanceForNode(Vector2 targetPosition)
    {
        Vector2 dif = targetPosition - (Vector2)previousNode.transform.position;
        return dif.sqrMagnitude;
    }
}