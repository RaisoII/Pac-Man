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
    public float radious;
    public Sprite icon;
    
    [TextArea(1, 10)] //(lineasMinimas,lineasMaximas)
    public string description;
    private string stats;
    public GameObject prefab;

    public Sprite getIcon => icon;

    public string  getDescription => description;
    public string getName => namePower;
  
    public Power GetPower => this;

    public void aplyEffect(Power power,int pos)
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

    public string getStats()
    {
        stats = "";
        if (cooldown != -1)
          stats += $"Cooldown: {cooldown} s \n";

        if (duration != -1)
          stats += $"Duration: {duration} s\n";

        if (radious != -1)
          stats += $"Radious: {radious} m\n";

        return stats;
    }

    public Power Clone()=> ScriptableObject.Instantiate(this);

    public void setTimeCoolDown(float time)
    {
      if(cooldown > 0)
        cooldown = time;
      else
        cooldown = 1;
    } 
    public void setTimeDuration(float duration) => this.duration = duration;
    public void setRadious(float radious) => this.radious = radious;
    public float getTimeCoolDown() => cooldown;
    public float getTimeDuration() => duration;
    public bool isPowerBuff() => false;
}
