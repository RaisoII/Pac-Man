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

    public abstract void aplyEffect(Power power,int pos); // la pos no se usa en este caso

    public abstract void generateParametersRandoms();

    protected int getBiasedRandom(int min, int max)
    {
        float randomValue = Random.Range(0f, 1f); // Obtén un valor aleatorio entre 0 y 1
        float biasedValue = Mathf.Pow(randomValue, 3); // Aplica una función cúbica para reducir la probabilidad de valores altos
        return Mathf.FloorToInt(min + (max - min) * biasedValue); // Escala el valor al rango deseado
    }

    public bool isPowerBuff() => true;
}
