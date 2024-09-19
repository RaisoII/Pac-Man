using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NuevoPoder", menuName = "Poder")]
public class Power : ScriptableObject,InterfaceIcon
{

    public string namePower;
    public float duration;
    public float cooldown;
    public Sprite icon;
    
    [TextArea(1, 10)] //(lineasMinimas,lineasMaximas)
    public string description;
    public GameObject prefab;

    public Sprite getIcon => icon;

    public string  getDescription => description;
    public string getName => namePower;
    
    public Power GetPower => this;

    public void aplyEffect(int pos)
    {
        GameObject GameManager = GameObject.Find("GameManager");
        PowerManagerPacMan powerManager = GameManager.GetComponent<PowerManagerPacMan>();
        powerManager.setPower(this,pos);
    }

    public void Activate()
    {
        Debug.Log("se activó poder: "+namePower);
    }

    public float getTimeCoolDown() => cooldown;
    public float getTimeDuration() => duration;
}
