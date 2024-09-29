using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioLoop;
    [SerializeField] private AudioClip selectOption;
    public void Awake()
    {
        audioLoop.Stop();
        GameObject gameManager = GameObject.Find("GameManager");
        GameObject inter = GameObject.Find("interface");
        GameObject pacMan = GameObject.FindWithTag("PacMan");
        
        if(gameManager != null)
            Destroy(gameManager);
        
        if(inter != null)
            Destroy(inter);
        
        if(pacMan != null)
            Destroy(pacMan);
    }

    private void Update()
    {
        checkKeys();
    }

    private void checkKeys()
    {
        
        if(Input.GetKeyDown(KeyCode.Return))
        {
            ManagerSound.instance.PlaySFX(selectOption,false);
            StartCoroutine(waitingOneSecond());
        }
    }

    private IEnumerator waitingOneSecond()
    {
        yield return new WaitForSeconds(1);
        listenerPlay();
    }

    public void listenerPlay()
    {
        SceneManager.LoadScene("Level2");
    }
}
