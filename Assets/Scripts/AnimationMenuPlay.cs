using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationMenuPlay : MonoBehaviour
{
    [SerializeField] private Image targetImage;      // El componente Image que vas a cambiar
    [SerializeField] private Sprite[] sprites;       // Arreglo de sprites asignados desde el inspector
    [SerializeField] private float changeInterval = 2f;  // Intervalo de tiempo entre cambios de imagen
    private int currentIndex = 0;
    private float timer = 0f;
    void Start()
    {
        if (sprites.Length > 0){
            targetImage.sprite = sprites[currentIndex];  
        }
    }

    void Update() {
        timer += Time.deltaTime;
        if (timer >= changeInterval)
        {
            timer = 0f; 
            currentIndex = (currentIndex + 1) % sprites.Length;  
            targetImage.sprite = sprites[currentIndex]; 
        }
    }
}
