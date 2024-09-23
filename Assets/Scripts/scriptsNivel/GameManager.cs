using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private GameObject gameInterface;
    [SerializeField] private GameObject panelFinishLevel;
    [SerializeField] private GameObject [] lifes;
    [SerializeField] private PowerManagerPacMan powerManager;
    private int cantDeaths;
    private static GameManager instance;
    private int currentLevel;
    public int cantTimeFrightened; // se suma
    public float cantSpeedPacman; // se suma
    public float cantSpeedGhost; //se resta;

    private void Awake()
    {
        // Implementación del patrón Singleton para evitar duplicados
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Evita que el GameObject se destruya al cambiar de escena
             // Verificamos si hay otra instancia de gameInterface ya en DontDestroyOnLoad
            DontDestroyOnLoad(gameInterface);
        }
        else
            Destroy(gameObject); // Destruye cualquier duplicado que se cree
        
        cantTimeFrightened = 0;
        cantSpeedPacman = 0;
        cantSpeedGhost = 0;
        currentLevel = 1;
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        cantDeaths = 0;
    }

     // Método que se llama cada vez que se carga una nueva escena (por ahora no le doy uso)
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Level"))
        {
            GameObject[] gameInterfaces = FindObjectsByName("interface");
        
            foreach(GameObject inter in gameInterfaces)
            {
                if(gameInterface != inter)
                    Destroy(inter);
            }

            gameInterface.SetActive(true);
            powerManager.checkPower();
        }
    }

    GameObject[] FindObjectsByName(string name)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> objectsWithName = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == name)
                objectsWithName.Add(obj);
        }

        return objectsWithName.ToArray();
    }

    private IEnumerator returnMenu()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Menu");
    }

    public void incrementScore()
    {
        int valueScore = int.Parse(score.text);
        score.text = ""+(valueScore + 10);
    }

    public bool removeOneLive()
    {
        bool continueGame = true;
        lifes[cantDeaths].GetComponent<Image>().enabled = false;
        cantDeaths++;
        if(cantDeaths == lifes.Length)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            continueGame = false;
            panelFinishLevel.SetActive(true);
            panelFinishLevel.transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(returnMenu());
        }
        return continueGame;
    }

    public void startGame() => powerManager.checkTimeCoolDown();

    public void stopGame() => powerManager.stopCoroutinesTimeCoolDown();

    public void existNextLevel()
    {
        stopGame();
        currentLevel++;
        string nameNextEscene = "Level"+currentLevel;

        panelFinishLevel.SetActive(true);

        if (Application.CanStreamedLevelBeLoaded(nameNextEscene))
        {
            panelFinishLevel.transform.GetChild(1).gameObject.SetActive(true);
            nameNextEscene = "seleccionarPowerUp";
        }
        else
        {
            nameNextEscene = "Menu";
            panelFinishLevel.transform.GetChild(2).gameObject.SetActive(true);
        }

        StartCoroutine(nextLevelRutine(nameNextEscene));
    }

    public void nextLevel() => SceneManager.LoadScene("Level"+currentLevel);

    private IEnumerator nextLevelRutine(string nextScene)
    {
        yield return new WaitForSeconds(3f);
        panelFinishLevel.SetActive(false);
        
        foreach(Transform children in panelFinishLevel.transform)
            children.gameObject.SetActive(false);

        SceneManager.LoadScene(nextScene);
    }
}
