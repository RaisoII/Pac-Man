using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InterfaceIcon
{
    public string getName { get; }
    public Sprite getIcon { get; }
    public string getDescription { get; }

    public Power GetPower{get; }

    public void aplyEffect(int pos);
}
