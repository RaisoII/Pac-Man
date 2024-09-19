using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Buff : ScriptableObject, InterfaceIcon
{
    public string nameBuff;
    public Power compatibilityPower; // El poder al que se aplicará la mejora (si no es a ningun poder tiene que ser nulo)
    public Sprite icon;
    public float probability;
    protected string description; // no es necesario escribirlo en el inspector
    public Sprite getIcon => icon;
    public string  getDescription => description;
    public string getName => nameBuff;

    public Power GetPower => compatibilityPower;

    public abstract void aplyEffect(int pos); // la pos no se usa en este caso

    public abstract void generateParametersRandoms();
}
