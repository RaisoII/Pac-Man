using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerKeys : MonoBehaviour
{
    [SerializeField] private PowerManagerPacMan powerManager;
    private Dictionary<KeyCode,bool> dictionaryPowerKey;
    void Awake()
    {
        dictionaryPowerKey = new Dictionary<KeyCode, bool>
        {   { KeyCode.Z, false },
            { KeyCode.X, false },
            { KeyCode.C, false }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) powerActivate(KeyCode.Z);
        if (Input.GetKeyDown(KeyCode.X)) powerActivate(KeyCode.X);
        if (Input.GetKeyDown(KeyCode.C)) powerActivate(KeyCode.C);
    }

    private void powerActivate(KeyCode key)
    {
        if (dictionaryPowerKey[key] && powerManager.canActivate(key))
        {
            powerManager.activatePower(key);
            powerManager.startTimeCoolDown(key);
        }
    }

    public void setKeyPower(KeyCode key) => dictionaryPowerKey[key] = true;
}
