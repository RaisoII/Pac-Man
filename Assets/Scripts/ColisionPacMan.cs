using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionPacMan : MonoBehaviour
{
    private GameManager gameManager;
    private LevelManager levelManager;
    
    public void setLevelManager(LevelManager level) => levelManager = level; 

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("pacDots"))
        {
            bool activeFrightened = col.gameObject.GetComponent<PacDots>().getactiveFrightened();
            levelManager.deletePacDot(activeFrightened,col.gameObject);
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
