using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionPacMan : MonoBehaviour
{
    private GameManager gameManager;
    private LevelManager levelManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnEnable()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("pacDots"))
        {
            bool activeFrightened = col.gameObject.GetComponent<PacDots>().getactiveFrightened();
            levelManager.deletePacDot(activeFrightened);
            Destroy(col.gameObject);
        }
        else if(col.CompareTag("Ghost"))
        {
            Ghost ghost = col.gameObject.GetComponent<Ghost>();
            if(ghost.getGhostState() == Ghost.GhostState.Frightened)
                col.gameObject.GetComponent<Ghost>().deathGhost();
            else
                levelManager.deathPacMan();
        }
    }
}
