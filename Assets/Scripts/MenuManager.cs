using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Awake()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        if(gameManager != null)
            Destroy(gameManager);
    }
    public void listenerPlay()
    {
        SceneManager.LoadScene("Level1");
    }
}
