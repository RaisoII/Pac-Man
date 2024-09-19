using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class selectorPower : MonoBehaviour
{
    [SerializeField] private GeneratorPowers generatorPowers;
    [SerializeField] private GameObject[] newPowerContainer;
    [SerializeField] private GameObject[] powerContainer;
    [SerializeField] private GameObject powerDescription;
    [SerializeField] private TextMeshProUGUI textDescription;
    [SerializeField] private float displacementText;
    [SerializeField] private float sizeContainer;
    [SerializeField] private float colorValue;
    private GameObject currentContainer;
    private GameObject[] currentArrayContainer;
    private GameObject previousContainer;
    private int indexContainer;
    private InterfaceIcon currentIcon;

    private void Start()
    {
        indexContainer = 0;
        currentArrayContainer = newPowerContainer;
        currentContainer = newPowerContainer[0];
        focusContainer();
    }

    private void Update()
    {
        checkKeys();
    }

    private void checkKeys()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
            selectContainer(-1);
        else if(Input.GetKeyDown(KeyCode.RightArrow))
            selectContainer(1);
        else if(Input.GetKeyDown(KeyCode.Return))
            checkEnter();
        
    }

    private void checkEnter()
    {
        if(currentArrayContainer == newPowerContainer)
        {
            currentIcon.aplyEffect(indexContainer);
        }
    }

    private void selectContainer(int value)
    {
        if(value == 0)
            return;

        indexContainer += value;
        if(indexContainer == -1)
            indexContainer = currentArrayContainer.Length - 1;
        else if(indexContainer == currentArrayContainer.Length)
            indexContainer = 0;
        
        currentContainer = currentArrayContainer[indexContainer];
        focusContainer();
    }

    private void focusContainer()
    {
        // actual
        RectTransform rectCurrent =  currentContainer.GetComponent<RectTransform>();

        rectCurrent.localScale = new Vector2(rectCurrent.localScale.x + sizeContainer,
                                                    rectCurrent.localScale.y + sizeContainer);
    
        Image currentImage = currentContainer.GetComponent<Image>();
        
        currentImage.color = new Color(currentImage.color.r + colorValue,currentImage.color.g + colorValue,
                                        currentImage.color.b + colorValue ,1);
        
        currentImage = currentContainer.transform.GetChild(0).gameObject.GetComponent<Image>();
        currentImage.color = new Color(currentImage.color.r + colorValue,currentImage.color.g + colorValue,
                                        currentImage.color.b + colorValue ,1);

        // anterior
        if(previousContainer != null)
        {
            RectTransform rectPrevious =  previousContainer.GetComponent<RectTransform>();
            
            rectPrevious.localScale = new Vector2(rectPrevious.localScale.x - sizeContainer,
                                                        rectPrevious.localScale.y - sizeContainer);

            Image imagePrevious = previousContainer.GetComponent<Image>();
            imagePrevious.color = new Color(imagePrevious.color.r - colorValue,imagePrevious.color.g - colorValue,
                                            imagePrevious.color.b - colorValue ,1);
            
            imagePrevious = previousContainer.transform.GetChild(0).gameObject.GetComponent<Image>();
            imagePrevious.color = new Color(imagePrevious.color.r - colorValue,imagePrevious.color.g - colorValue,
                                            imagePrevious.color.b - colorValue ,1);
        }

        // descripcion
        RectTransform rectDescription =  powerDescription.GetComponent<RectTransform>();
        rectDescription.anchoredPosition = new Vector2(rectCurrent.anchoredPosition.x,
                                                        rectCurrent.anchoredPosition.y + displacementText);
        
        previousContainer = currentContainer;

        currentIcon = generatorPowers.getInterfaceIcon(indexContainer);
        textDescription.text ="     "+currentIcon.getName +"\n\n"+currentIcon.getDescription;
    }
}