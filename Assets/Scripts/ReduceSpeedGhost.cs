using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NuevaMejoraPacman", menuName = "Mejora/velocidadFantasmas")]
public class ReduceSpeedGhost : Buff
{
    public float speedGhost;

    public override void generateParametersRandoms()
    {
        speedGhost = Random.Range(1,100);
        speedGhost = speedGhost / 100;
        description =  "reduces the speed of ghosts by "+speedGhost+"%";
    }

    public override void aplyEffect(int pos)
    {
        GameObject gameManager = GameObject.Find("GameManager");
        GameManager manager = gameManager.GetComponent<GameManager>();
        manager.cantSpeedGhost *= (1 + speedGhost / 100f);
    }
}
