using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPinky : Ghost
{
    protected override void Awake()
    {
        base.Awake();
        OnReachedDestination += HandleReachedDestination;
    }
    
    public override void findPathStart()
    {
        startPath.Add(endNode);
        startPath.Add(endNode.getNeightbor(Vector2.up));
    }

    private void HandleReachedDestination()
    {
        Vector2 pacManPosition = movPacMan.gameObject.transform.localPosition;
        pacManPosition = new Vector2(Mathf.RoundToInt(pacManPosition.x),Mathf.RoundToInt(pacManPosition.y));
        Vector2 direction = movPacMan.getDirection();
        targetVector = pacManPosition + (direction * 4);
    }
}
