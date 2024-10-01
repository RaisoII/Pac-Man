using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NuevaMejoraPacman", menuName = "Mejora/velocidadFantasmas")]
public class ReduceSpeedGhost : Buff
{
    public float speedGhost;

    public override void generateParametersRandoms()
    {
        speedGhost = getBiasedRandom(1,5);
        description =  "reduces the speed of ghosts by <color=red>"+speedGhost+"% </color>";
    }

    public override void aplyEffect(Power power,int pos)
    {
        GameObject ObjectGameManager = GameObject.Find("GameManager");
        GameObject objectScriptsGenerales = GameObject.Find("scriptsGenerales");

        GameManager manager = ObjectGameManager.GetComponent<GameManager>();
        GeneratorPowers generator = objectScriptsGenerales.GetComponent<GeneratorPowers>();
        
        manager.percentageSpeedGhost += speedGhost;
        generator.showInfoAtributes();
    }
}
