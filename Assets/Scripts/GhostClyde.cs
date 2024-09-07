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
        Invoke("findPathStart",1.5f);
    }

    private void findPathStart()
    {
        startPath.Add(endNode);
        startPath.Add(endNode.getNeightbor(Vector2.left));
        startPath.Add(endNode.getNeightbor(Vector2.left).getNeightbor(Vector2.up));
    }

    private void HandleReachedDestination()
    {
        float distance = getDistance(transform.localPosition,movPacMan.gameObject.transform.localPosition);
        if(distance < 8)
            ChangedState(GhostState.Scatter);
        else
        {
            targetVector = movPacMan.gameObject.transform.position;
            targetVector = new Vector2(Mathf.RoundToInt(targetVector.x),Mathf.RoundToInt(targetVector.y));
        }
    }

    private void DistanceForPackMan()
    {
        float distance = getDistance(transform.localPosition,movPacMan.gameObject.transform.localPosition);
        if(distance > 8)
            ChangedState(GhostState.Chasing);
    }
}
