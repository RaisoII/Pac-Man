using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NuevaMejoraPacman", menuName = "Mejora/enfriamientoPoder")]
public class PowerCoolDown : Buff
{
    public float decrementCoolDown;

    public override void aplyEffect(Power power,int pos)
    {
        GameObject gameManager = GameObject.Find("GameManager");
        GameObject generalScript = GameObject.Find("scriptsGenerales");

        GeneratorPowers generatorPower = generalScript.GetComponent<GeneratorPowers>();
        PowerManagerPacMan powerManager = gameManager.GetComponent<PowerManagerPacMan>();

        powerManager.reduceTimeCoolDownPower(decrementCoolDown,pos);
        power.setTimeCoolDown(power.getTimeCoolDown() - decrementCoolDown);

        generatorPower.showInfoPower();
    }
    public override void generateParametersRandoms()
    {
        decrementCoolDown = getBiasedRandom(1,12);
        
        description = "reduces power cooldown \n<color=yellow>"+compatibilityPower.getName+
                        "</color> by <color=green>"+decrementCoolDown+" seconds </color>";
    }
}
