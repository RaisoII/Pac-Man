using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class levelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private float timeFrightenedRutine,timeFrightenedRutineAux;
    private Coroutine frightenedRutine;
    private Ghost[] ghostArray;
    private MovPacMan movPac;
    private  int cantPacDots;

    private void Start()
    {
        movPac = GameObject.FindGameObjectWithTag("PacMan").GetComponent<MovPacMan>();
        cantPacDots =  GameObject.FindGameObjectsWithTag("pacDots").Length;
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        ghostArray = new Ghost[ghosts.Length];
        
        for(int i = 0; i < ghosts.Length; i++) 
            ghostArray[i] =  ghosts[i].GetComponent<Ghost>();

        StartCoroutine(startGame());
    }

    private IEnumerator startGame()
    {
        yield return new WaitForSeconds(3);
        foreach(Ghost g in ghostArray)
        {
            g.enabled = true;
            g.startWaiting();
        }
        
        movPac.enabled = true;
    }

    public void deletePacDot(bool activeFrightened)
    {
        cantPacDots--;
        if(cantPacDots == 0)
            Debug.Log("ganaste pa");
        
        if(activeFrightened)
        {
            if(frightenedRutine == null)
            {
                foreach(Ghost ghost in ghostArray)
                    ghost.ChangedStateFrightened(true);

                frightenedRutine = StartCoroutine(frightenedTime());
            }
            else 
            {
                timeFrightenedRutineAux += timeFrightenedRutine; 
            }
        }

        int valueScore = int.Parse(score.text);
        score.text = ""+(valueScore + 10);
    }

    private IEnumerator frightenedTime()
    {
        timeFrightenedRutineAux = timeFrightenedRutine;
        while(timeFrightenedRutineAux > 0)
        {
            timeFrightenedRutineAux -= Time.deltaTime;
            yield return null;
        }

        frightenedRutine = null;

        foreach(Ghost ghost in ghostArray)
            ghost.ChangedStateFrightened(false);
    }

    public void deathPacMan()
    {
        foreach(Ghost g in ghostArray)
            g.enabled = false;

        movPac.enabled = false;
        Debug.Log("reset");
    }

    public bool getIsfrightenedTime() => frightenedRutine != null;
}
