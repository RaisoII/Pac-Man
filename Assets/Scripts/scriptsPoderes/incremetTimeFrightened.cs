using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NuevaMejoraPacman", menuName = "Mejora/tiempoAtemorizado")]
public class incremetTimeFrightened : Buff
{
    
    public int timeFrightened;

    public override void generateParametersRandoms()
    {
        timeFrightened = Random.Range(1,10);
        description =  "increases time Frightened by <color=green>"+timeFrightened+"% </color>";
    }

    public override void aplyEffect(Power power, int pos)
    {
        GameObject ObjectGameManager = GameObject.Find("GameManager");
        GameObject objectScriptsGenerales = GameObject.Find("scriptsGenerales");

        GameManager manager = ObjectGameManager.GetComponent<GameManager>();
        GeneratorPowers generator = objectScriptsGenerales.GetComponent<GeneratorPowers>();
        
        manager.percentageTimeFrightened += timeFrightened;
        generator.showInfoAtributes();
    }
}
