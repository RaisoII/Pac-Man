using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerINVISIBLEMAN : MonoBehaviour,powerObject
{
    [SerializeField] private float blinkDuration; 
    
    private float duration;
    private SpriteRenderer spriteRenderer; 
    private GameObject pacMan;
    private LevelManager levelManager;
    private IEnumerator powerActivate()
    {
        float normalDuration = duration - blinkDuration;
        float timer = 0f;

        Color color = spriteRenderer.color;
        color.a = 0.5f;

        spriteRenderer.color = color;

        while (timer < normalDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f; 
        while (timer < blinkDuration)
        {
            timer += Time.deltaTime;
            Debug.Log("estoy parpadeando xd");
            // Alternar entre alfa 0.5 y 1.
            color.a = Mathf.PingPong(Time.time * 2f, 0.5f) + 0.5f;
            spriteRenderer.color = color;

            yield return null;
        }

        Color finalColor = spriteRenderer.color;
        finalColor.a = 1;
        spriteRenderer.color = finalColor;
        Destroy(pacMan.GetComponent<colisionPacManINVISIBLE>());
        ColisionPacMan col = pacMan.AddComponent<ColisionPacMan>();
        col.setLevelManager(levelManager);
        Destroy(gameObject);
    }

    public void setTime(float time)
    {
        this.duration = time;
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        
        pacMan = gameObject.transform.parent.gameObject;
        Destroy(pacMan.GetComponent<ColisionPacMan>());
        
        colisionPacManINVISIBLE colINVISIBLE = pacMan.AddComponent<colisionPacManINVISIBLE>();
        colINVISIBLE.setLevelManager(levelManager);
        spriteRenderer = pacMan.GetComponent<SpriteRenderer>();
        StartCoroutine(powerActivate());
    }
}
