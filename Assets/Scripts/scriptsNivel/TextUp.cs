using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextMeshPro text =  gameObject.GetComponent<TextMeshPro>();
        text.color = Color.blue; 
        text.fontSize = text.fontSize + 2;
        StartCoroutine(textUpCoroutine());   
    }

    private IEnumerator textUpCoroutine()
    {
        yield return new WaitForSeconds(.1f);

        Vector2 posTarget = (Vector2) transform.position + Vector2.up;

        Vector2 startPos = transform.position; // Posición inicial

        float duration = .3f; // Duración de la interpolación
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector2.Lerp(startPos, posTarget, elapsedTime / duration); // Interpolación de la posición
            elapsedTime += Time.deltaTime; // Incrementa el tiempo transcurrido
            yield return null; // Espera hasta el siguiente frame
        }

        transform.position = posTarget; // Asegura que llega a la posición final
    }

}
