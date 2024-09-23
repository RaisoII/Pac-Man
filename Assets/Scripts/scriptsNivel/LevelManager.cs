using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float timeFrightenedRutine;
    [SerializeField] private float timeWaiting;
    private  GameObject[] arrayPacDots;
    private float timeFrightenedRutineAux;
    private Coroutine frightenedRutine;
    private MovPacMan movPac;
    private GameManager gameManager;
    private SpawnerManager spawn;
    private Ghost [] ghostArray;
    private int cantPacDots;
    private void Awake()
    {
        cantPacDots = GameObject.FindGameObjectsWithTag("pacDots").Length;

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
        ColisionPacMan colPac = spawn.getPacMan().GetComponent<ColisionPacMan>();
        colPac.setLevelManager(this);

        StartCoroutine(startGame());
    }

    public void resetPositions()
    {
        spawn.resetPositions();
        foreach(Ghost ghost in ghostArray)
            ghost.setCurrentNode();
        
        movPac.resetPositionAndDirection();

        foreach(GameObject pacDots in arrayPacDots)
            pacDots.GetComponent<PacDots>().restartPosition();
    }

    private IEnumerator startGame()
    {
        yield return new WaitForSeconds(timeWaiting);
        foreach(Ghost g in ghostArray)
        {
            g.enabled = true;
            g.startWaiting();
        }
        
        
        movPac.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        movPac.enabled = true;
        movPac.startMove();

        gameManager.startGame();
    }
    public void deletePacDot(bool activeFrightened,GameObject pacDot)
    {
        cantPacDots--;

        if(cantPacDots == 0)
        {
            gameManager.existNextLevel();
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
        gameManager.stopGame();
        yield return new WaitForSeconds(timeWaiting);
        resetPositions();
        StartCoroutine(startGame());
    }

    public void deathPacMan()
    { 
        GameObject pacMan = movPac.gameObject;
        pacMan.GetComponent<BoxCollider2D>().enabled = false;
        foreach(Transform child in pacMan.transform)
            Destroy(child.gameObject);

        stopMovPacDots();
        enabledMovementPrefabs(false);
        bool continueGame =  gameManager.removeOneLive();

        if(continueGame)    
            StartCoroutine(restartGame());
    }

    // una mejora sería consultar si el pacman tiene el poder del iman que hace que los pacdots se muevan, para evitar 
    // el foreach si es el caso que no tenga el poder
    private void stopMovPacDots()
    {
        arrayPacDots =  GameObject.FindGameObjectsWithTag("pacDots"); 
        
        foreach(GameObject pacDot in arrayPacDots)
        {
            GoToTargetIMAN gotoPacMan = pacDot.GetComponent<GoToTargetIMAN>();
            if(gotoPacMan != null)
            {
                gotoPacMan.stopCoroutine();
                Destroy(gotoPacMan);
            }
        }
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