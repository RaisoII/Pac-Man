using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class levelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    private Ghost[] ghostArray;
    private  int cantPacDots;

    private void Start()
    {
        cantPacDots =  GameObject.FindGameObjectsWithTag("pacDots").Length;
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        ghostArray = new Ghost[ghosts.Length];
        
        for(int i = 0; i < ghosts.Length; i++) 
            ghostArray[i] =  ghosts[i].GetComponent<Ghost>();
    }

    public void deletePacDot(bool activeFrightened)
    {
        cantPacDots--;
        if(cantPacDots == 0)
            Debug.Log("ganaste pa");
        
        if(activeFrightened)
        {
            foreach(Ghost ghost in ghostArray)
                ghost.ChangedStateFrightened();
        }

        int valueScore = int.Parse(score.text);
        score.text = ""+(valueScore + 50);
    }
}
