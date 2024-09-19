using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerManagerPacMan : MonoBehaviour
{
    [SerializeField] private GameObject powerContainer;
    [SerializeField] private ManagerKeys managerKeys;
    [SerializeField] private Image coolDownFirstImage,coolDownSecondImage,coolDownThirdImage;
    private Dictionary<int,Power> dictionaryPower;
    private float timeFirstPower,timeSecondPower,timeThirdPower;
    private float timeFirstPowerAux,timeSecondPowerAux,timeThirdPowerAux;

    private void Start()
    {
        timeFirstPower = timeSecondPower = timeThirdPower = 0;
        timeFirstPowerAux = timeSecondPowerAux = timeThirdPowerAux = 0; 
        
        dictionaryPower = new Dictionary<int,Power>();
        //setPower(p1,0);
        //setPower(p2,1);
        //setPower(p3,2);
        checkPower();
    }
    public void setPower(Power p, int pos)
    {
        dictionaryPower.Add(pos,p);

        KeyCode key = KeyCode.Alpha0;

        if(pos == 0)
        {
            key = KeyCode.Z;
            timeFirstPower = p.getTimeCoolDown();
            timeFirstPowerAux = timeFirstPower;
        }
        else if(pos == 1)
        {
            key = KeyCode.X;
            timeSecondPower = p.getTimeCoolDown();
            timeSecondPowerAux = timeSecondPower;
        }
        else if(pos == 2)
        {
            key = KeyCode.C;
            timeThirdPower = p.getTimeCoolDown();
            timeThirdPowerAux = timeThirdPower;
        }

        managerKeys.setKeyPower(key);
    }

    public void checkPower()
    {
        Transform TpowerContainer = powerContainer.transform;
        for(int i = 0; i < 3;i++)
        {
            if(dictionaryPower.ContainsKey(i))
                TpowerContainer.GetChild(i).gameObject.SetActive(true);
            else
                TpowerContainer.GetChild(i).gameObject.SetActive(false);
        }

        if(dictionaryPower.Count == 0)
            managerKeys.enabled = false;
        else
            managerKeys.enabled = true;   
    }

    public void checkTimeCoolDown()
    {
        if(timeFirstPowerAux > 0)
            startTimeCoolDown(KeyCode.Z);
        
        if(timeSecondPowerAux > 0)
            startTimeCoolDown(KeyCode.X);
            
        if(timeThirdPowerAux > 0)
            startTimeCoolDown(KeyCode.C);
    
    }

    public bool canActivate(KeyCode key)
    {
        float timeAux = 0;
        if(key == KeyCode.Z)
            timeAux = timeFirstPowerAux;
        else if(key == KeyCode.X)
            timeAux = timeSecondPowerAux;
        else if(key == KeyCode.C)
            timeAux = timeThirdPowerAux;

        return timeAux == 0;
    }

    public void activatePower(KeyCode key)
    {
        if(key == KeyCode.Z)
        {
            dictionaryPower[0].Activate();
            timeFirstPowerAux = timeFirstPower;
        }
        else if(key == KeyCode.X)
        {
            dictionaryPower[1].Activate();
            timeSecondPowerAux = timeSecondPower;
        }
        else if(key == KeyCode.C)
        {
            dictionaryPower[2].Activate();
            timeThirdPowerAux = timeThirdPower;
        }
    }

    public void stopCoroutinesTimeCoolDown() => StopAllCoroutines();

    public void startTimeCoolDown(KeyCode key)
    {
        if (key == KeyCode.Z)
        {
            StartCoroutine(timeCoolDownRutine(() => timeFirstPowerAux, value => timeFirstPowerAux = value,
                                                timeFirstPower,coolDownFirstImage));
        }
        else if (key == KeyCode.X)
        {
            StartCoroutine(timeCoolDownRutine(() => timeSecondPowerAux, value => timeSecondPowerAux = value,
                                                timeSecondPower,coolDownSecondImage));
        }
        else if (key == KeyCode.C)
        {
            StartCoroutine(timeCoolDownRutine(() => timeThirdPowerAux, value => timeThirdPowerAux = value,
                                                timeThirdPower,coolDownThirdImage));
        }
    }

    private IEnumerator timeCoolDownRutine(System.Func<float> getTime, System.Action<float> setTime,float coolDownTime,Image imageCoolDown)
    {
        while (getTime() > 0)
        {
            yield return new WaitForSeconds(1);
            setTime(getTime() - 1); // Reduce el tiempo en 1 segundo y lo actualiza
            imageCoolDown.fillAmount = getTime() / coolDownTime;
        }
    }

    public Dictionary<int,Power> getPowers() => dictionaryPower;
}
