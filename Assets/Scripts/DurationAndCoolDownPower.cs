using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NuevaMejoraPacman", menuName = "Mejora/duracionYenfriamientoPoder")]
public class DurationAndCoolDownPower : Buff
{
    public int incrementDuration;
    public float reductionCoolDown;

    public override void aplyEffect(int pos)
    {
        GameObject gameManager = GameObject.Find("GameManager");
        PowerManagerPacMan powerManager = gameManager.GetComponent<PowerManagerPacMan>();
        powerManager.incrementTimePower(incrementDuration,pos);
        powerManager.reduceTimeCoolDownPower(reductionCoolDown,pos);
    }
    public override void generateParametersRandoms()
    {
        incrementDuration = Random.Range(1,5);
        reductionCoolDown = Random.Range(1,10);
        description = "reduces cooldown by "+reductionCoolDown+" seconds and duration by "+incrementDuration+" seconds of power "+nameBuff;
    }
}
