using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostClyde : Ghost
{
    protected override void Awake()
    {
        base.Awake();
        
        OnReachedDestination += HandleReachedDestination;
        checkDistancePacMan += DistanceForPackMan;
        
        Invoke("findPathStart",3.5f);
    }

    private void findPathStart()
    {
        startPath.Add(endNode);
        startPath.Add(endNode.getNeightbor(Vector2.left));
        startPath.Add(endNode.getNeightbor(Vector2.left).getNeightbor(Vector2.up));
    }

    private void HandleReachedDestination()
    {
        float distance = getDistance(transform.position,movPacMan.gameObject.transform.position);
        if(distance < 8 && currentState != GhostState.Scatter)
            ChangedState(GhostState.Scatter);
        else
        {
            targetVector = movPacMan.gameObject.transform.position;
            targetVector = new Vector2(Mathf.RoundToInt(targetVector.x),Mathf.RoundToInt(targetVector.y));
        }
    }

    private void DistanceForPackMan()
    {
        float distance = getDistance(transform.position,movPacMan.gameObject.transform.position);
        if(distance > 8)
        {
            Debug.Log("cambia la perra: "+distance);
            ChangedState(GhostState.Chasing);
        }
    }
}
