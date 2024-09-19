using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorPowers : MonoBehaviour
{
    [SerializeField] private List<Power> listPower;
    [SerializeField] private GameObject[] powerContainer;
    private GameObject pacMan;
    private int cantPowersOfBuffs;
    private List<Power> powersUsed;
    private PowerManagerPacMan managerPower;

    private void Awake()
    {
        GameObject interfaceGame = GameObject.Find("interface");
        interfaceGame.SetActive(false);
        powersUsed = new List<Power>();
        cantPowersOfBuffs = 0;
        pacMan = GameObject.FindGameObjectWithTag("PacMan"); // para averiguar que poderes ya tiene
        managerPower = GameObject.Find("GameManager").GetComponent<PowerManagerPacMan>();
        searchPowerForPacMan();
        generatePowersOrBuffs();
    }

    private void searchPowerForPacMan()
    {
        Dictionary<int,Power> dictionary =  managerPower.getPowers();
        
        foreach(GameObject container in powerContainer)
        {
            Image imageContainer = container.GetComponent<Image>();
            imageContainer.sprite = null;
        }

        foreach (KeyValuePair<int, Power> entry in dictionary)
        {
            int key = entry.Key;
            Power power = entry.Value; 
            GameObject container =  powerContainer[key];
            Image imageContainer = container.GetComponent<Image>();
            imageContainer.sprite = power.icon;
        }
    }

    private void generatePowersOrBuffs()
    {
        while(cantPowersOfBuffs  < 3)
        {
            int random = Random.Range(0,100);
            if(random < 50)
                generateBuff();
            else if(random < 75)
                generatePower();
            else
                generateBuffPower();
            cantPowersOfBuffs++;
        }
    } 
    
    private void generateBuff()
    {

    }

    private void generateBuffPower()
    {

    }

    private void generatePower()
    {
        for(int i = 0; i < 3; i++)
        {
            Power newPower = null;
            
            while(newPower == null)
            {
                newPower =  listPower[Random.Range(0,listPower.Count)];
                if(powersUsed.Contains(newPower))
                    newPower = null;
            }
        }
    }
}
