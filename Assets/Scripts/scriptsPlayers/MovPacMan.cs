using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovPacMan : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] protected Vector2 direction;
    [SerializeField] private Vector2 nextDirection;
    private Node startNode,currentNode,previousNode,targetNode;
    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.left;
        //changedPosition(direction);
    }

    // Update is called once per frame
    void FixedUpdate()
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
        Node moveToNode  = currentNode.getNeightbor(dir); 
        return moveToNode;
    }

    private void changedPosition(Vector2 dir)
    {
        if(dir != direction)
            nextDirection = dir;

        if(currentNode != null)
        {
            Node moveToNode = canMove(dir);
            
            if(moveToNode != null)
            {
                direction = dir;
                targetNode = moveToNode;
                previousNode = currentNode;
                currentNode = null;
            }
        }
    }

    private void move()
    {
        if(targetNode !=  currentNode  && targetNode != null)
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
                currentNode = targetNode;
                transform.localPosition = currentNode.transform.position;

                Node portalNode = currentNode.getPortalNode();
                
                if(portalNode != null)
                {
                    transform.localPosition = portalNode.transform.position;
                    currentNode = portalNode;
                }

                Node moveToNode = canMove(nextDirection);
                if(moveToNode != null)
                    direction = nextDirection;
                
                if(moveToNode == null)
                    moveToNode = canMove(direction);

                if(moveToNode != null)
                {
                    targetNode = moveToNode;
                    previousNode = currentNode;
                    currentNode = null;
                }
                else
                    direction = Vector2.zero;
            }
            else
                transform.localPosition += (Vector3) direction * speed * Time.deltaTime;
        }
    }

    private bool onTarget()
    {
        float nodeTarget = distanceForNode(targetNode.transform.position);
        float nodeToSelf = distanceForNode(transform.localPosition);
        return nodeToSelf > nodeTarget;
    }

    // llamado cuando pierde una vida
    public void resetMov()
    {

    }

    private float distanceForNode(Vector2 targetPosition)
    {
        Vector2 dif = targetPosition - (Vector2)previousNode.transform.position;
        return dif.sqrMagnitude;
    }
    public void setNodeIni(Node ini)
    {
        currentNode = ini;
        startNode = ini;
    } 

    public void resetPositionAndDirection()
    {
        currentNode = startNode;
        targetNode = currentNode;
        direction = Vector2.left;
        nextDirection = Vector2.left;
    }

    public void startMove() => changedPosition(direction);

    public Vector2 getDirection() => direction;

    public float getSpeed() => speed;

    public bool getEating() => gameObject.GetComponent<ColisionPacMan>().getEating();
}