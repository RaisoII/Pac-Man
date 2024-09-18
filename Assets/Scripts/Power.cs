using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    public string namePower;
    public float duration;
    public float cooldown;

    public Power(string namePower, float duration, float cooldown)
    {
        this.namePower = namePower;
        this.duration = duration;
        this.cooldown = cooldown;
    }

    public void powerUp()
    {

    }

    public void Activate()
    {
        Debug.Log("se activó poder: "+namePower);
    }

    public float getTimeCoolDown() => cooldown;
}
