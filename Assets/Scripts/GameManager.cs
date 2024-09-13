using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject [] lifes;
    private LevelManager levelManager;
    private int cantDeaths;
    private static GameManager instance;

    private void Awake()
    {
        // Implementación del patrón Singleton para evitar duplicados
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Evita que el GameObject se destruya al cambiar de escena
        }
        else
            Destroy(gameObject); // Destruye cualquier duplicado que se cree
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        searchLevelManager(); 
        cantDeaths = 0;
    }

     // Método que se llama cada vez que se carga una nueva escena
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        searchLevelManager();
        Debug.Log("entra OnScene");
    }
    

    private void searchLevelManager()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
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
            gameOverPanel.SetActive(true);
            StartCoroutine(returnMenu());
        }
        return continueGame;
    }
}
