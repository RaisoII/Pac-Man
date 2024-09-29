using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerINVISIBLEMAN : MonoBehaviour,powerObject
{
    [SerializeField] private float blinkDuration; 
    private float duration;
    private SpriteRenderer spriteRendererPacMan,spriteRendererMouthOpen,spriteRendererMouthClose; 
    private GameObject pacMan;
    private IEnumerator powerActivate()
    {
        float normalDuration = duration - blinkDuration;
        float timer = 0f;

        Color colorPacMan = spriteRendererPacMan.color;
        Color colorMouthOpen = spriteRendererMouthOpen.color;
        Color colorMouthClose = spriteRendererMouthClose.color;
        colorPacMan.a = 0.5f;
        colorMouthOpen.a = .5f;
        colorMouthClose.a = .5f;

        spriteRendererPacMan.color = colorPacMan;
        spriteRendererMouthOpen.color = colorMouthOpen;
        spriteRendererMouthClose.color = colorMouthClose;

        while (timer < normalDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f; 
        while (timer < blinkDuration)
        {
            timer += Time.deltaTime;
            // Alternar entre alfa 0.5 y 1.
            float alphaValue = Mathf.PingPong(Time.time * 2f, 0.5f) + 0.5f;
            colorPacMan.a = alphaValue;
            colorMouthOpen.a = alphaValue;
            colorMouthClose.a = alphaValue;
            
            spriteRendererPacMan.color = colorPacMan;
            spriteRendererMouthOpen.color = colorMouthOpen;
            spriteRendererMouthClose.color = colorMouthClose;

            yield return null;
        }
        
        ColisionPacMan colPacman = pacMan.GetComponent<ColisionPacMan>();
        colPacman.setTag("Ghost");

        Color finalColor = spriteRendererPacMan.color;
        finalColor.a = 1;
        spriteRendererPacMan.color = finalColor;
        Destroy(gameObject);
    }

    public void setTime(float time)
    {
        this.duration = time;
        
        pacMan = gameObject.transform.parent.gameObject;
        ColisionPacMan colPacman = pacMan.GetComponent<ColisionPacMan>();
        colPacman.removeTag("Ghost");
        
        spriteRendererPacMan = pacMan.GetComponent<SpriteRenderer>();
        spriteRendererMouthOpen = pacMan.transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRendererMouthClose = pacMan.transform.GetChild(1).GetComponent<SpriteRenderer>();

        StartCoroutine(powerActivate());
    }
}
