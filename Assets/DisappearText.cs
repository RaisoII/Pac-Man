using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisappearText : MonoBehaviour
{
    [SerializeField] private float visibleTime;
    [SerializeField] private TextMeshPro text;

    private void Start()
    {
        StartCoroutine(disappearText());
    }

    private IEnumerator disappearText()
    {
        float time = 0; 
        Color colorText = text.color;
        while (time < visibleTime)
        {
            time += Time.deltaTime;
            // Podés ir haciendo que el texto sea más transparente
            float alpha = Mathf.Lerp(1, 0, time / visibleTime); 
            text.color = new Color(colorText.r, colorText.g, colorText.b, alpha);
            yield return null;
        }
    
        Destroy(gameObject);
    }

    public void setCant(int cant) => text.text = ""+cant;
}
