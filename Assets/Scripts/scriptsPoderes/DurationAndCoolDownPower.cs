using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NuevaMejoraPacman", menuName = "Mejora/duracionYenfriamientoPoder")]
public class DurationAndCoolDownPower : Buff
{
    public int incrementDuration;
    public float reductionCoolDown;

    public override void aplyEffect(Power power,int pos)
    {
        GameObject gameManager = GameObject.Find("GameManager");
        GameObject generalScript = GameObject.Find("scriptsGenerales");

        GeneratorPowers generatorPower = generalScript.GetComponent<GeneratorPowers>();
        PowerManagerPacMan powerManager = gameManager.GetComponent<PowerManagerPacMan>();
        
        powerManager.incrementTimePower(incrementDuration,pos);
        powerManager.reduceTimeCoolDownPower(reductionCoolDown,pos);
        
        power.setTimeCoolDown(power.getTimeCoolDown() - reductionCoolDown);
        power.setTimeDuration(power.getTimeDuration() + incrementDuration);
        
        generatorPower.showInfoPower();
    }
    public override void generateParametersRandoms()
    {
        incrementDuration = getBiasedRandom(1,5);
        reductionCoolDown = getBiasedRandom(1,10);

        description = "reduces cooldown by <color=green>"
                        +reductionCoolDown+" seconds \n </color> and duration by <color=green>"
                        +incrementDuration+" seconds </color> \n of power <color=yellow>"+compatibilityPower.getName+"</color>";
    }
}
