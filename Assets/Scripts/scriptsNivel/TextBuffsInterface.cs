using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextBuffsInterface : MonoBehaviour
{
    [SerializeField] private float fadeInDuration = 1f; // Tiempo de entrada
    [SerializeField] private float fadeOutDuration = 1f; // Tiempo de salida
    [SerializeField] private float displayDuration = 2f; // Tiempo que se queda visible
    [SerializeField] private List<TextMeshProUGUI> listText; // Referencia al texto que querés animar
    [SerializeField] private LevelManager level;
    [SerializeField] private GameObject interfaceBuff;
    
    IEnumerator FadeTextList()
    {
        // Aplica el efecto de aparición/desaparición a cada texto
        foreach (TextMeshProUGUI text in listText)
        {
            yield return StartCoroutine(FadeIn(text));
            yield return new WaitForSeconds(displayDuration); // Espera mientras el texto se queda visible
            yield return StartCoroutine(FadeOut(text));
        }

        interfaceBuff.SetActive(false);
        level.startGame();
    }

    IEnumerator FadeIn(TextMeshProUGUI text)
    {
        float elapsedTime = 0f;
        Color color = text.color;
        while (elapsedTime < fadeInDuration)
        {
            color.a = Mathf.Lerp(0, 1, elapsedTime / fadeInDuration); // Aumenta la opacidad
            text.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        color.a = 1;
        text.color = color;
    }

    IEnumerator FadeOut(TextMeshProUGUI text)
    {
        float elapsedTime = 0f;
        Color color = text.color;
        while (elapsedTime < fadeOutDuration)
        {
            color.a = Mathf.Lerp(1, 0, elapsedTime / fadeOutDuration); // Reduce la opacidad
            text.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        color.a = 0;
        text.color = color;
    }

    public void showBuffs()
    {
        interfaceBuff.SetActive(true);
        StartCoroutine(FadeTextList());
    } 
}

