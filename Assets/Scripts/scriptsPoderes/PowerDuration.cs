using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NuevaMejoraPacman", menuName = "Mejora/duracionPoder")]
public class PowerDuration : Buff
{
    public int incrementPowerDuration;

    public override void aplyEffect(Power power,int pos)
    {
        GameObject gameManager = GameObject.Find("GameManager");
        GameObject generalScript = GameObject.Find("scriptsGenerales");

        GeneratorPowers generatorPower = generalScript.GetComponent<GeneratorPowers>();
        PowerManagerPacMan powerManager = gameManager.GetComponent<PowerManagerPacMan>();
        
        powerManager.incrementTimePower(incrementPowerDuration,pos);
        power.setTimeDuration(power.getTimeDuration() + incrementPowerDuration);

        generatorPower.showInfoPower();
    }

    public override void generateParametersRandoms()
    {
        incrementPowerDuration = getBiasedRandom(1,10);
        
        description = "increases the duration \n of the power <color=yellow>"+compatibilityPower.getName+
        "</color>\n by <color=green>"+incrementPowerDuration+" seconds </color>";
    }
}
