using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBlinky : Ghost
{
    protected override void Awake()
    {
        StartCoroutine(waitingTime());
    }

    private IEnumerator waitingTime()
    {
        yield return new WaitForSeconds(timeWaiting);
        Debug.Log("sale: "+gameObject.name);
    }
}
