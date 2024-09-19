using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NuevoPoder", menuName = "Poder")]
public class Power : ScriptableObject 
{

    public string namePower;
    public float duration;
    public float cooldown;
    public Sprite icon;
    public string description;
    public GameObject prefab;

    public void powerUp()
    {

    }

    public void Activate()
    {
        Debug.Log("se activó poder: "+namePower);
    }

    public float getTimeCoolDown() => cooldown;
}
