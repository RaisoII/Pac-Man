using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionPacMan : MonoBehaviour
{
    [SerializeField] private AudioClip eatOpenSound, eatCloseSound,eatGhost;  // Sonido de abrir
    [SerializeField] private animationController animPacMan;
    private List<string> tagsDetection;

    private void Awake()
    {
        tagsDetection = new List<string>{"pacDots","Ghost"};
    }
    private bool isEating = false; // Indica si Pac-Man está comiendo

    private LevelManager levelManager;

    public void setLevelManager(LevelManager level) => levelManager = level; 

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!tagsDetection.Contains(col.tag))
            return;
        
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
            animPacMan.changedAnimationEating(isEating);
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
            {
                animPacMan.deathPacMan(true);
                levelManager.deathPacMan();
            }
        }
    }

    public void setSoundEat(AudioClip eatOpenSound,AudioClip eatCloseSound)
    {
        this.eatOpenSound = eatOpenSound;
        this.eatCloseSound = eatCloseSound;
    }

    public void setTag(string tag) => tagsDetection.Add(tag);

    public void removeTag(string tag) => tagsDetection.Remove(tag);

    public bool getEating() => isEating;
}
