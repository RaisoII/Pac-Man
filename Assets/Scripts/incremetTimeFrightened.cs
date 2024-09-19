using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NuevaMejoraPacman", menuName = "Mejora/tiempoAtemorizado")]
public class incremetTimeFrightened : Buff
{
    
    public int timeFrightened;

    public override void generateParametersRandoms()
    {
        timeFrightened = Random.Range(1,5);
        description =  "increases time Frightened by "+timeFrightened+"%";
    }

    public override void aplyEffect(int pos)
    {
        GameObject gameManager = GameObject.Find("GameManager");
        GameManager manager = gameManager.GetComponent<GameManager>();
        manager.cantTimeFrightened += timeFrightened;
    }
}
