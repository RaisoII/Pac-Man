using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NuevaMejoraPacman", menuName = "Mejora/velocidadPacman")]
public class IncrementSpeedPacMan : Buff
{
    public float speedPacMan;

    public override void generateParametersRandoms()
    {
        speedPacMan = Random.Range(1,100);
        speedPacMan = speedPacMan / 100;
        description =  "increases PacMan speed by "+speedPacMan+"%";
    }

    public override void aplyEffect(int pos)
    {
        GameObject gameManager = GameObject.Find("GameManager");
        GameManager manager = gameManager.GetComponent<GameManager>();
        manager.cantSpeedPacman *= (1 + speedPacMan / 100f);
    }
}
