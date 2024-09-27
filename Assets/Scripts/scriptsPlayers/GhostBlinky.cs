using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBlinky : Ghost
{
    protected override void Awake()
    {
        originalColor = render.color;
        OnReachedDestination += HandleReachedDestination;
    }

    public override void findPathStart()
    {

    }

    public override void startWaiting() => StartCoroutine(waitingTime());

    private IEnumerator waitingTime()
    {
        yield return new WaitForSeconds(timeWaiting);
        ChangedState(GhostState.Scatter);
    }

     // Maneja la llegada al destino
    private void HandleReachedDestination()
    {
        targetVector = movPacMan.gameObject.transform.position;
        targetVector = new Vector2(Mathf.RoundToInt(targetVector.x),Mathf.RoundToInt(targetVector.y));
    }
}