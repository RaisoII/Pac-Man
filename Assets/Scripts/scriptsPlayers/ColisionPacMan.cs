using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionPacMan : MonoBehaviour
{
    [SerializeField] private AudioClip eatOpenSound, eatCloseSound,eatGhost;  // Sonido de abrir

    private bool isEating = false; // Indica si Pac-Man está comiendo

    private LevelManager levelManager;

    public void setLevelManager(LevelManager level) => levelManager = level; 

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("pacDots"))
        {
            bool activeFrightened = col.gameObject.GetComponent<PacDots>().getactiveFrightened();
            levelManager.deletePacDot(activeFrightened,col.gameObject);
            
            if (!isEating)
            {
                ManagerSound.instance.PlaySFX(eatOpenSound,false);
                isEating = true; // Comienza a comer
            }
            else
            {
                ManagerSound.instance.PlaySFX(eatCloseSound,false);
                isEating = false;
            }

            Destroy(col.gameObject);
        }
        else if(col.CompareTag("Ghost"))
        {
            Ghost ghost = col.gameObject.GetComponent<Ghost>();
            if(ghost.getGhostState() == Ghost.GhostState.Frightened)
            {
                ManagerSound.instance.PlaySFX(eatGhost,false);
                col.gameObject.GetComponent<Ghost>().deathGhost();
            }
            else
                levelManager.deathPacMan();
        }
    }

    public void setSoundEat(AudioClip eatOpenSound,AudioClip eatCloseSound)
    {
        this.eatOpenSound = eatOpenSound;
        this.eatCloseSound = eatCloseSound;
    }
}
