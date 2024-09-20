using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float timeFrightenedRutine;
    private float timeFrightenedRutineAux;
    private Coroutine frightenedRutine;
    private MovPacMan movPac;
    private  int cantPacDots;
    private GameManager gameManager;
    private SpawnerManager spawn;
    private Ghost [] ghostArray;

    private void Awake()
    {
        GameObject[] pactDotsArray = GameObject.FindGameObjectsWithTag("pacDots");
        cantPacDots = pactDotsArray.Length - 1;

        spawn = gameObject.GetComponent<SpawnerManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spawn.instantiatePrefabs();
        List<GameObject> listGhost = spawn.getListGhost();
        ghostArray = new Ghost[listGhost.Count];
        
        for(int i=0; i < listGhost.Count;i++)
        {
            GameObject ghost = listGhost[i];
            Ghost g = ghost.GetComponent<Ghost>();
            ghostArray[i] = g;
        }

        movPac = spawn.getPacMan().GetComponent<MovPacMan>();

        StartCoroutine(startGame());
    }

    public void resetPositions()
    {
        spawn.resetPositions();
        foreach(Ghost ghost in ghostArray)
            ghost.setCurrentNode();
        movPac.GetComponent<Animator>().SetBool("dead", false);
        movPac.resetPositionAndDirection();
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
        {
            gameManager.nextLevel();
            enabledMovementPrefabs(false);
        }
            
        if(activeFrightened)
        {
            // está fuera porque hay fantasmas que pueden estar escapando y otros en la ghostHouse o en dispersión
            foreach(Ghost ghost in ghostArray) 
                ghost.ChangedStateFrightened(true);

            if(frightenedRutine == null)
                frightenedRutine = StartCoroutine(frightenedTime());
            else 
                timeFrightenedRutineAux += timeFrightenedRutine; 
        }

        gameManager.incrementScore();
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
    
    private IEnumerator restartGame()
    {
        yield return new WaitForSeconds(3);
        resetPositions();
        StartCoroutine(startGame());
    }

    public void deathPacMan()
    { 
        enabledMovementPrefabs(false);
        bool continueGame =  gameManager.removeOneLive();

        if(continueGame)    
            StartCoroutine(restartGame());
    }

    private void enabledMovementPrefabs(bool value)
    {
        foreach(Ghost g in ghostArray)
        {
            g.stopStates();
            g.enabled = value;
        }
        movPac.enabled = value;
    }
}