using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerIMAN : MonoBehaviour, powerObject
{
    private float time;
    private IEnumerator powerActivate()
    {
        while(time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    public void setTime(float time)
    {
        this.time = time;
        StartCoroutine(powerActivate());
    }
}
