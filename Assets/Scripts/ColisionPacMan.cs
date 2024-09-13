using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionPacMan : MonoBehaviour
{
    private levelManager level;

    private void Start()
    {
        level = GameObject.Find("GeneralScripts").GetComponent<levelManager>();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("pacDots"))
        {
            bool activeFrightened = col.gameObject.GetComponent<PacDots>().getactiveFrightened();
            level.deletePacDot(activeFrightened);
            Destroy(gameObject);
        }
        else if(col.CompareTag("Ghost"))
        {
            if(level.getIsfrightenedTime())
            {
                Destroy(col.gameObject);
            }
            else
                level.deathPacMan();
        }
    }
}
