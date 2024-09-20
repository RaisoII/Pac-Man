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

    public void Activate(float time)
    {
        GameObject pacMan = GameObject.FindWithTag("PacMan");
        GameObject powerObject = Instantiate(prefab,pacMan.transform.position,Quaternion.identity,pacMan.transform);
        powerObject interfacePower = powerObject.GetComponent<powerObject>();
        interfacePower.setTime(time);
    }

    public float getTimeCoolDown() => cooldown;
    public float getTimeDuration() => duration;
}
