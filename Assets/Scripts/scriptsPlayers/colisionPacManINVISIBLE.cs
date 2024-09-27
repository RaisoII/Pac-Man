using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colisionPacManINVISIBLE : MonoBehaviour
{
    private AudioClip eatOpenSound, eatCloseSound;
    private LevelManager levelManager;
     private bool isEating = false; // Indica si Pac-Man está comiendo

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
    }

    public void setSoundEat(AudioClip eatOpenSound, AudioClip eatCloseSound)
    {
        this.eatOpenSound = eatOpenSound;
        this.eatCloseSound = eatCloseSound;
    }
}
