using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colisionPacManINVISIBLE : MonoBehaviour
{

    private LevelManager levelManager;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("pacDots"))
        {
            bool activeFrightened = col.gameObject.GetComponent<PacDots>().getactiveFrightened();
            levelManager.deletePacDot(activeFrightened,col.gameObject);
            Destroy(col.gameObject);
        }
    }

    public void setLevelManager(LevelManager levelManager) => this.levelManager = levelManager;
}
