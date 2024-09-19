using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NuevaMejoraPacman", menuName = "Mejora/duracionPoder")]
public class PowerDuration : Buff
{
    public int incrementPowerDuration;

    public override void aplyEffect(int pos)
    {
        GameObject gameManager = GameObject.Find("GameManager");
        PowerManagerPacMan powerManager = gameManager.GetComponent<PowerManagerPacMan>();
        powerManager.incrementTimePower(incrementPowerDuration,pos);
    }

    public override void generateParametersRandoms()
    {
        
        description = "increases the duration of the power "+nameBuff+" by "+incrementPowerDuration+" seconds";
    }
}
