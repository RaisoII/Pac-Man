using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Awake()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        GameObject inter = GameObject.Find("interface");
        
        if(gameManager != null)
            Destroy(gameManager);
            
        if(inter != null)
            Destroy(inter);
    }
    public void listenerPlay()
    {
        SceneManager.LoadScene("Level1");
    }
}
