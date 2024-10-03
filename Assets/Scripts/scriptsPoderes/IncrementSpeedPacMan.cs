using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NuevaMejoraPacman", menuName = "Mejora/velocidadPacman")]
public class IncrementSpeedPacMan : Buff
{
    public float speedPacMan;

    public override void generateParametersRandoms()
    {
        speedPacMan = getBiasedRandom(1,8);
        description =  "increases PacMan speed by <color=green>"+speedPacMan+"%</color>";
    }

    public override void aplyEffect(Power power,int pos)
    {
        GameObject ObjectGameManager = GameObject.Find("GameManager");
        GameObject objectScriptsGenerales = GameObject.Find("scriptsGenerales");

        GameManager manager = ObjectGameManager.GetComponent<GameManager>();
        GeneratorPowers generator = objectScriptsGenerales.GetComponent<GeneratorPowers>();
        GameObject pacMan = GameObject.FindGameObjectWithTag("PacMan");
        MovPacMan movPac = pacMan.GetComponent<MovPacMan>();
        movPac.setSpeed(movPac.getSpeed() +  (speedPacMan*movPac.getSpeed() )/ 100f);
        manager.percentageSpeedPacMan += speedPacMan;
        generator.showInfoAtributes();
    }
}
