using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NuevaMejoraPacman", menuName = "Mejora/enfriamientoPoder")]
public class PowerCoolDown : Buff
{
    public float decrementCoolDown;

    public override void aplyEffect(int pos)
    {
        GameObject gameManager = GameObject.Find("GameManager");
        PowerManagerPacMan powerManager = gameManager.GetComponent<PowerManagerPacMan>();
        powerManager.reduceTimeCoolDownPower(decrementCoolDown,pos);
    }
    public override void generateParametersRandoms()
    {
        description = "reduces power cooldown "+nameBuff+" by "+decrementCoolDown+" seconds";
    }
}
