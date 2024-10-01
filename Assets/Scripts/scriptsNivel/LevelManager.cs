using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float timeFrightenedRutine,speedGhost;
    [SerializeField] private float timeWaitingStartGame;
    [SerializeField] private bool buffsGhost;
    [SerializeField] private TextBuffsInterface textBuffs;
    [SerializeField] private AudioClip soundStart,soundDeath,soundNormalGhost,soundFrigthenedGhost;
    [SerializeField] private GameObject prefabText;
    private  GameObject[] arrayPacDots;
    private float timeFrightenedRutineAux;
    private Coroutine frightenedRutine;
    private MovPacMan movPac;
    private GameManager gameManager;
    private SpawnerManager spawn;
    private Ghost [] ghostArray;
    private int cantPacDots;
    private int cantPacDotsConsume;
    private bool isFirstTimeStart;
    private ManagerKeys managerKeys;
    private void Awake()
    {
        ManagerSound.instance.StopAudioLoop();  // puede quedar un sonido loopeado del nivel anterior 
        isFirstTimeStart = true;
        cantPacDotsConsume = 0;
        cantPacDots = GameObject.FindGameObjectsWithTag("pacDots").Length;

        spawn = gameObject.GetComponent<SpawnerManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        managerKeys = gameManager.GetComponent<ManagerKeys>();
        
        timeFrightenedRutine += (timeFrightenedRutine * gameManager.getPercentageFrightened()) / 100f;
        spawn.instantiatePrefabs();
    
        List<GameObject> listGhost = spawn.getListGhost();
        ghostArray = new Ghost[listGhost.Count];
        
        for(int i=0; i < listGhost.Count;i++)
        {
            GameObject ghost = listGhost[i];
            Ghost g = ghost.GetComponent<Ghost>();
            ghostArray[i] = g;
            g.setSpeed(speedGhost - (speedGhost * gameManager.getPercentageSpeedGhost()) / 100f);
        }

        movPac = spawn.getPacMan().GetComponent<MovPacMan>();
        ColisionPacMan colPac = spawn.getPacMan().GetComponent<ColisionPacMan>();
        colPac.setLevelManager(this);
        movPac.enabled = false; // puede estar activado de otros niveles
        if(buffsGhost)
            textBuffs.showBuffs();
        else
            StartCoroutine(startGameRoutine());
    }

    public void startGame() // desde fuera
    {
        StartCoroutine(startGameRoutine());
    } 

    public void resetPositions()
    {
        spawn.resetPositions();
        foreach(Ghost ghost in ghostArray)
            ghost.setCurrentNode();
        
        movPac.resetPositionAndDirection();

        foreach(GameObject pacDots in arrayPacDots)
        {
            PacDots pac = pacDots.GetComponent<PacDots>();
            if(pac == null)
                Debug.Log(pacDots.name+ " es nulo");
            pac.restartPosition();

        }
            
        List<GameObject> prefabFire = GameObject.FindGameObjectsWithTag("Fire").ToList();
        while(prefabFire.Count > 0)
        {
            Destroy(prefabFire[0]);
            prefabFire.RemoveAt(0);
        }
    }

    private IEnumerator startGameRoutine()
    {
        ManagerSound.instance.PlaySFX(soundStart,false);
        yield return new WaitForSeconds(timeWaitingStartGame);
        
        if(ghostArray.Length > 0)
            ManagerSound.instance.PlaySFX(soundNormalGhost,true);
        
        foreach(Ghost g in ghostArray)
        {
            g.relive();
            g.startWaiting();

            if(isFirstTimeStart)
                g.findPathStart();

            g.enabled = true;
        }
        
        isFirstTimeStart = false;
        
        movPac.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        movPac.enabled = true;
        movPac.startMove();

        gameManager.startGame();
        managerKeys.enabled = true;
    }
    public void deletePacDot(bool activeFrightened,GameObject pacDot)
    {
        cantPacDots--;
        cantPacDotsConsume++;
        instantiateText();
        if(cantPacDots == 0)
        {
            ManagerSound.instance.StopAudioLoop(); // para los sonidos en loop
            gameManager.existNextLevel();
            enabledMovementPrefabs(false);
        }
            
        if(activeFrightened && cantPacDots != 0)
        {
            // está fuera porque hay fantasmas que pueden estar escapando y otros en la ghostHouse o en dispersión
            foreach(Ghost ghost in ghostArray) 
                ghost.ChangedStateFrightened(true);

            if(frightenedRutine == null)
            {
                ManagerSound.instance.StopAudioLoop();
                ManagerSound.instance.PlaySFX(soundFrigthenedGhost,true);
                frightenedRutine = StartCoroutine(frightenedTime());
            }
            else 
                timeFrightenedRutineAux += timeFrightenedRutine; 
        }

        gameManager.incrementScore();
    }

    private void instantiateText()
    {
        Vector2 dirPacMan = -1*movPac.getDirection();
        Vector2 posText = (Vector2)movPac.gameObject.transform.localPosition + dirPacMan + new Vector2(0,0.5f); // un poco mas arriba
        GameObject objectText = Instantiate(prefabText,posText,Quaternion.identity);
        objectText.GetComponent<DisappearText>().setCant(cantPacDotsConsume);
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
        
        ManagerSound.instance.StopAudioLoop();
        ManagerSound.instance.PlaySFX(soundNormalGhost,true);
    }
    
    private IEnumerator restartGameRoutine()
    {
        ManagerSound.instance.StopAudioLoop(); // para los sonidos en loop
        ManagerSound.instance.PlaySFX(soundDeath,false);
        gameManager.stopGame();
        yield return new WaitForSeconds(timeWaitingStartGame);
        resetPositions();
        StartCoroutine(startGameRoutine());
    }

    public void deathPacMan()
    { 
        managerKeys.enabled = false;
        GameObject pacMan = movPac.gameObject;
        pacMan.GetComponent<BoxCollider2D>().enabled = false;
        Transform TpacMan = pacMan.transform; 
        
        for(int i = 3; i < TpacMan.childCount; i++)
        {
            Transform child = TpacMan.GetChild(i);
            Destroy(child.gameObject);
        }
        
        stopMovPacDots();
        enabledMovementPrefabs(false);
        bool continueGame =  gameManager.removeOneLive();

        if(continueGame)    
            StartCoroutine(restartGameRoutine());
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