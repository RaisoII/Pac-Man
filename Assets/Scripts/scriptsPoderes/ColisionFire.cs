using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionFire : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private float timeFire;

    private IEnumerator timeRutine()
    {
        while(timeFire > 0)
        {
            timeFire -= Time.deltaTime;
            anim.SetFloat("duration",timeFire); 
            yield return null;
        }

        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Ghost"))
        {
            Ghost ghost = col.gameObject.GetComponent<Ghost>();
            if(ghost.getGhostState() != Ghost.GhostState.Death)
            {
                ghost.deathGhost();
            }
        }
    }

    public void setTimeFire(float time)
    {
        timeFire = time;
        anim.SetFloat("duration",timeFire); 
        StartCoroutine(timeRutine());
    }
}
