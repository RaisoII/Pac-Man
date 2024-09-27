using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostInky : Ghost
{
    private GameObject blinky;
    protected override void Awake()
    {
        base.Awake();
        
        OnReachedDestination += HandleReachedDestination;
        blinky = GameObject.Find("ghostBlinky(Clone)");
    }
    
    public override void findPathStart()
    {
        startPath.Add(endNode);
        startPath.Add(endNode.getNeightbor(Vector2.right));
        startPath.Add(endNode.getNeightbor(Vector2.right).getNeightbor(Vector2.up));
    }

    private void HandleReachedDestination()
    {
        Vector2 pacManPosition = movPacMan.gameObject.transform.localPosition;
        pacManPosition = new Vector2(Mathf.RoundToInt(pacManPosition.x),Mathf.RoundToInt(pacManPosition.y));
        Vector2 direction = movPacMan.getDirection();
        targetVector = pacManPosition + (direction * 2);

        int posBlinkyX = Mathf.RoundToInt(blinky.transform.localPosition.x);
        int posBlinkyY = Mathf.RoundToInt(blinky.transform.localPosition.y);

        Vector2 posBlinky = new Vector2(posBlinkyX,posBlinkyY);

        float distance = getDistance(posBlinky,targetVector);
        distance *= 2;
        targetVector = new Vector2(posBlinky.x + distance,posBlinky.y + distance);
    }
}
