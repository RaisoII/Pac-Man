using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorPowers : MonoBehaviour
{
    [SerializeField] private List<Power> listPower;
    [SerializeField] private List<Buff> listBuffs;
    private List<InterfaceIcon> listIconic;
    [SerializeField] private GameObject[] newPowerContainer;
    [SerializeField] private GameObject[] powerContainer;
    [SerializeField] private Sprite emptyIcon;
    private GameObject pacMan;
    private int cantPowersOfBuffs;
    private List<Power> powersUsed;
    private List<Power> powerPacMan;
    private PowerManagerPacMan managerPower;

    private void Awake()
    {
        GameObject interfaceGame = GameObject.Find("interface");
        pacMan = GameObject.FindGameObjectWithTag("PacMan"); // para averiguar que poderes ya tiene
        managerPower = GameObject.Find("GameManager").GetComponent<PowerManagerPacMan>();
        
        interfaceGame.SetActive(false);
        pacMan.SetActive(false);
        
        cantPowersOfBuffs = 0;
        
        powersUsed = new List<Power>();
        powerPacMan = new List<Power>();
        listIconic =  new List<InterfaceIcon>();
        
        searchPowerForPacMan();
        generatePowersOrBuffs();
    }

    private void searchPowerForPacMan()
    {
        Dictionary<int,Power> dictionary =  managerPower.getPowers();
        
        foreach(GameObject container in powerContainer)
        {
            Image imageContainer = container.transform.GetChild(0).gameObject.GetComponent<Image>();
            imageContainer.sprite = emptyIcon;
        }

        foreach (KeyValuePair<int, Power> entry in dictionary)
        {
            int key = entry.Key;
            Power power = entry.Value; 
            GameObject container =  powerContainer[key];
            Image imageContainer = container.transform.GetChild(0).gameObject.GetComponent<Image>();
            imageContainer.sprite = power.icon;
            powerPacMan.Add(power);
        }
    }

    private void generatePowersOrBuffs()
    {
        while(cantPowersOfBuffs  < 3)
        {
            int random = Random.Range(0,100);
            if(random < 50)
                generateBuff();
            else// if(random < 75)
                generatePower();
            //else
              //  generateBuffPower();
        }
    } 
    
    private void generateBuff()
    {
        Buff newBuff = null;
        
        while(newBuff == null)
        {
            newBuff = listBuffs[Random.Range(0,listBuffs.Count)];
            if(newBuff.GetPower != null)
                newBuff = null;
            else
                newBuff.generateParametersRandoms();
        }

        setContainer(cantPowersOfBuffs,newBuff);
        cantPowersOfBuffs++;
    }

    private void generateBuffPower()
    {
        if(powerPacMan.Count == 0)
            return;
    }

    private void generatePower()
    {
        if(powerPacMan.Count == listPower.Count)
            return;

        Power newPower = null;
        
        while(newPower == null)
        {
            newPower =  listPower[Random.Range(0,listPower.Count)];
            if(powersUsed.Contains(newPower))
                newPower = null;
            else
                powersUsed.Add(newPower);
        }

        setContainer(cantPowersOfBuffs,newPower);
        cantPowersOfBuffs++;
    }

    private void setContainer(int index,InterfaceIcon p)
    {
        GameObject container = newPowerContainer[index];
        Image imageContainer = container.transform.GetChild(0).gameObject.GetComponent<Image>();
        imageContainer.sprite = p.getIcon;
        listIconic.Add(p);
    }

    public InterfaceIcon getInterfaceIcon(int index) => listIconic[index];
}
