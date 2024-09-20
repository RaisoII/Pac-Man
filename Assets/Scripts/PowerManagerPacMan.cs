using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerManagerPacMan : MonoBehaviour
{
    [SerializeField] private Power p1,p2,p3;
    [SerializeField] private GameObject powerContainer;
    [SerializeField] private ManagerKeys managerKeys;
    [SerializeField] private Image coolDownFirstImage,coolDownSecondImage,coolDownThirdImage;
    private Dictionary<int,Power> dictionaryPower;
    private float timeCoolDownFirstPower,timeCoolDownSecondPower,timeCoolDownThirdPower;
    private float timeCoolDownFirstPowerAux,timeCoolDownSecondPowerAux,timeCoolDownThirdPowerAux;
    private float timeDurationFirstPower,timeDurationSecondPower,timeDurationThirdPower;

    private void Start()
    {
        timeCoolDownFirstPower = timeCoolDownSecondPower = timeCoolDownThirdPower = 0;
        timeCoolDownFirstPowerAux = timeCoolDownSecondPowerAux = timeCoolDownThirdPowerAux = 0; 
        
        dictionaryPower = new Dictionary<int,Power>();
        setPower(p1,0);
        setPower(p2,1);
        setPower(p3,2);
        checkPower();
    }
    public void setPower(Power p, int pos)
    {
        if(p == null) // para pruebas, esto se borra luego. nunca puede ser nuloo
            return;

        if(dictionaryPower.ContainsKey(pos))
        {
            dictionaryPower.Remove(pos);
        }

        dictionaryPower.Add(pos,p);

        KeyCode key = KeyCode.Alpha0;

        if(pos == 0)
        {
            key = KeyCode.Z;
            timeCoolDownFirstPower = p.getTimeCoolDown();
            timeDurationFirstPower = p.getTimeDuration();
            timeCoolDownFirstPowerAux = timeCoolDownFirstPower;

        }
        else if(pos == 1)
        {
            key = KeyCode.X;
            timeCoolDownSecondPower = p.getTimeCoolDown();
            timeDurationSecondPower = p.getTimeDuration();
            timeCoolDownSecondPowerAux = timeCoolDownSecondPower;
        }
        else if(pos == 2)
        {
            key = KeyCode.C;
            timeCoolDownThirdPower = p.getTimeCoolDown();
            timeDurationThirdPower = p.getTimeDuration();
            timeCoolDownThirdPowerAux = timeCoolDownThirdPower;
        }

        managerKeys.setKeyPower(key);
    }

    public void checkPower()
    {
        Transform TpowerContainer = powerContainer.transform;
        for(int i = 0; i < 3;i++)
        {
            if(dictionaryPower.ContainsKey(i))
            {
                Power p = dictionaryPower[i];
                GameObject container = TpowerContainer.GetChild(i).gameObject;
                container.SetActive(true);
                GameObject imageChild = container.transform.GetChild(0).gameObject;
                Image image = imageChild.GetComponent<Image>();
                image.sprite = p.getIcon;
            }
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
        if(timeCoolDownFirstPowerAux > 0)
            startTimeCoolDown(KeyCode.Z);
        
        if(timeCoolDownSecondPowerAux > 0)
            startTimeCoolDown(KeyCode.X);
            
        if(timeCoolDownThirdPowerAux > 0)
            startTimeCoolDown(KeyCode.C);
    
    }

    public bool canActivate(KeyCode key)
    {
        float timeAux = 0;
        if(key == KeyCode.Z)
            timeAux = timeCoolDownFirstPowerAux;
        else if(key == KeyCode.X)
            timeAux = timeCoolDownSecondPowerAux;
        else if(key == KeyCode.C)
            timeAux = timeCoolDownThirdPowerAux;

        return timeAux == 0;
    }

    public void activatePower(KeyCode key)
    {
        if(key == KeyCode.Z)
        {
            dictionaryPower[0].Activate(timeDurationFirstPower);
            timeCoolDownFirstPowerAux = timeCoolDownFirstPower;
        }
        else if(key == KeyCode.X)
        {
            dictionaryPower[1].Activate(timeDurationSecondPower);
            timeCoolDownSecondPowerAux = timeCoolDownSecondPower;
        }
        else if(key == KeyCode.C)
        {
            dictionaryPower[2].Activate(timeDurationThirdPower);
            timeCoolDownThirdPowerAux = timeCoolDownThirdPower;
        }
    }

    public void stopCoroutinesTimeCoolDown() => StopAllCoroutines();

    public void startTimeCoolDown(KeyCode key)
    {
        if (key == KeyCode.Z)
        {
            StartCoroutine(timeCoolDownRutine(() => timeCoolDownFirstPowerAux, value => timeCoolDownFirstPowerAux = value,
                                                timeCoolDownFirstPower,coolDownFirstImage));
        }
        else if (key == KeyCode.X)
        {
            StartCoroutine(timeCoolDownRutine(() => timeCoolDownSecondPowerAux, value => timeCoolDownSecondPowerAux = value,
                                                timeCoolDownSecondPower,coolDownSecondImage));
        }
        else if (key == KeyCode.C)
        {
            StartCoroutine(timeCoolDownRutine(() => timeCoolDownThirdPowerAux, value => timeCoolDownThirdPowerAux = value,
                                                timeCoolDownThirdPower,coolDownThirdImage));
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

    public void incrementTimePower(int cant, int pos)
    {
        if(pos == 0)
            timeDurationFirstPower += cant;
        else if(pos == 1)
            timeDurationSecondPower += cant;
        else if(pos == 2)
            timeDurationThirdPower += cant;
    }

    public void reduceTimeCoolDownPower(float cant, int pos)
    {
        if(pos == 0)
            timeCoolDownFirstPower -= cant;
        else if(pos == 1)
            timeCoolDownSecondPower -= cant;
        else if(pos == 2)
            timeCoolDownThirdPower -= cant;
    }

    public Dictionary<int,Power> getPowers() => dictionaryPower;
}
