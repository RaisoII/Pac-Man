using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class GeneratorPowers : MonoBehaviour
{
    [SerializeField] private Transform infoPowerContainer;
    [SerializeField] private List<Power> listPower;
    [SerializeField] private List<Buff> listBuffs;
    [SerializeField] private GameObject[] newPowerContainer;
    [SerializeField] private GameObject[] powerContainer;
    [SerializeField] private Sprite emptyIcon;
    [SerializeField] private TextMeshProUGUI textSpeedPacMan,textSpeedGhost,textTimeFrightered;
    private List<InterfaceIcon> listIconic;
    private GameObject pacMan;
    private int cantPowersOfBuffs;
    private List<InterfaceIcon> powersUsed;
    private Dictionary<int,Power> powerPacman;
    private PowerManagerPacMan managerPower;
    private GameManager gameManager;

    private void Awake()
    {
        GameObject interfaceGame = GameObject.Find("interface");
        pacMan = GameObject.FindGameObjectWithTag("PacMan"); // para averiguar que poderes ya tiene
        GameObject ObjectGameManager =  GameObject.Find("GameManager");
        managerPower = ObjectGameManager.GetComponent<PowerManagerPacMan>();
        gameManager = ObjectGameManager.GetComponent<GameManager>();
        
        interfaceGame.SetActive(false);
        pacMan.transform.position = new Vector2(-100,-100);
        
        cantPowersOfBuffs = 0;
        
        powersUsed = new List<InterfaceIcon>();
        listIconic =  new List<InterfaceIcon>();
        
        searchPowerForPacMan();
        generatePowersOrBuffs();
        showInfoPower();
        showInfoAtributes();
    }

    private void searchPowerForPacMan()
    {
        powerPacman =  managerPower.getPowers();
        
        foreach(GameObject container in powerContainer)
        {
            Image imageContainer = container.transform.GetChild(0).gameObject.GetComponent<Image>();
            imageContainer.sprite = emptyIcon;
        }

        foreach (KeyValuePair<int, Power> entry in powerPacman)
        {
            int key = entry.Key;
            Power power = entry.Value; 
            GameObject container =  powerContainer[key];
            Image imageContainer = container.transform.GetChild(0).gameObject.GetComponent<Image>();
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
        }
    } 
    
    private void generateBuff()
    {
        Buff newBuff = null;
        
        while(newBuff == null)
        {
            newBuff = listBuffs[Random.Range(0,listBuffs.Count)];
            if(newBuff.GetPower != null || listIconic.Contains(newBuff))
                newBuff = null;
            else
                newBuff.generateParametersRandoms();
        }

        setContainer(cantPowersOfBuffs,newBuff);
        cantPowersOfBuffs++;
    }

    private void generateBuffPower()
    {
        if(powerPacman.Count == 0)
            return;
        
        Buff buffPower = null;
        
        int i = 0;
        while(buffPower == null && i < 1000)
        {
            buffPower = listBuffs[Random.Range(0,listBuffs.Count)];
            
            if(powersUsed.Any(p => p.getName == buffPower.getName))
            {
                buffPower = null;
                continue;
            }
            
            if(buffPower.compatibilityPower == null || !powerPacman.Values.Any(p => p.getName == buffPower.compatibilityPower.getName))
            {
                buffPower = null;
                continue;
            }

            buffPower.generateParametersRandoms();
            powersUsed.Add(buffPower);
            setContainer(cantPowersOfBuffs,buffPower);
            cantPowersOfBuffs++;
            i++;
        }
    }

    private void generatePower()
    {
        if(listPower.Count == powerPacman.Count + powersUsed.Count) 
            return;

        Power newPower = null;
        
        while(newPower == null)
        {
            int random = Random.Range(0,listPower.Count);
            newPower =  listPower[random];
            if(powersUsed.Any(p => p.getName == newPower.getName) || powerPacman.Values.Any(p => p.getName == newPower.getName))
                newPower = null;
            else
                powersUsed.Add(newPower);
        }
        
        Power powerClone = newPower.Clone();
        setContainer(cantPowersOfBuffs,powerClone);
        cantPowersOfBuffs++;
    }

    private void setContainer(int index,InterfaceIcon p)
    {
        GameObject container = newPowerContainer[index];
        Image imageContainer = container.transform.GetChild(0).gameObject.GetComponent<Image>();
        imageContainer.sprite = p.getIcon;
        listIconic.Add(p);
    }

    public void showInfoAtributes()
    {
        textSpeedPacMan.text = gameManager.percentageSpeedPacMan+" %";
        textSpeedGhost.text  = gameManager.percentageSpeedGhost+" %";
        textTimeFrightered.text = gameManager.percentageTimeFrightened+ " s";

        if(gameManager.percentageSpeedPacMan > 0)
            textSpeedPacMan.color = Color.green;
            
        if(gameManager.percentageSpeedGhost > 0)
            textSpeedGhost.color = Color.red;
            
        if(gameManager.percentageTimeFrightened > 0)
            textTimeFrightered.color = Color.green;
    }

    public Power getPower(int pos)
    {
        if(powerPacman.ContainsKey(pos))
            return powerPacman[pos];
        
        return null;
    }

    public void addPower(Power p, int pos)
    {
        if(powerPacman.ContainsKey(pos))
        {
            powerPacman.Remove(pos);
        }

        powerPacman.Add(pos,p);
        managerPower.setPower(p,pos);
    }

    public int getPosPower(Power p)
    {
        foreach (KeyValuePair<int, Power> entry in powerPacman)
        {
            int key = entry.Key;
            Power power = entry.Value; 
            if(power.getName == p.GetPower.getName)
                return key;
        }

        return -1;
    }

    public void removePower(int pos)
    {
        if(powerPacman.ContainsKey(pos))
        {   
            powerPacman.Remove(pos);
        }
    }

    public void showInfoPower()
    {
        if(powerPacman.Count == 0)
            return;
        
        infoPowerContainer.gameObject.SetActive(true);
        
        for(int i = 0; i < 3; i++)
        {
            Transform child = infoPowerContainer.GetChild(i);
            if(!powerPacman.ContainsKey(i))
                child.gameObject.SetActive(false);
            else
            {
                child.gameObject.SetActive(true);
                Power p = powerPacman[i];
                child.GetChild(0).GetComponent<Image>().sprite = p.getIcon;
                TextMeshProUGUI text = child.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
                text.text = "";
                text.text = p.getStats();
            }
        }
    }

    public void refreshStats(string parameter, float cant, int pos)
    {
        Transform child = infoPowerContainer.GetChild(pos);
        TextMeshProUGUI text = child.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();

        // El texto actual en el componente
        string currentText = text.text;

        // Dependiendo del parámetro, reemplaza el valor correspondiente en el texto
        string updatedText = currentText;

        // Reemplazar el parámetro correspondiente con el nuevo valor
        switch (parameter.ToLower())
        {
            case "cooldown":
                updatedText = currentText.Replace("{Cooldown}", cant.ToString("F1"));
                break;
            case "duration":
                updatedText = currentText.Replace("{Duration}", cant.ToString("F1"));
                break;
            case "radious":
                updatedText = currentText.Replace("{Radious}", cant.ToString("F1"));
                break;
            default:
                Debug.LogWarning("El parámetro especificado no es válido.");
                break;
        }

        text.text = updatedText;
    }

    public InterfaceIcon getInterfaceIcon(int index) => listIconic[index];
}
