using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class selectorPower : MonoBehaviour
{
    [SerializeField] private AudioClip moveUI,selectOption;
    [SerializeField] private GameObject interfacePower;
    // son putas listas porque el "boton" de continuar estará y... no estará. porque de lo contrario usaría arreglos
    [SerializeField] private GeneratorPowers generatorPowers; 
    [SerializeField] private List<GameObject> listNewPowerContainer;
    [SerializeField] private List<GameObject> listPowerContainer;
    [SerializeField] private List<GameObject> listReplaceOptions;
    [SerializeField] private GameObject replaceOptions;
    [SerializeField] private GameObject objectPowerInsert; // lo uso en el caso que eliga un poder
    [SerializeField] private GameObject objectContinue;
    [SerializeField] private GameObject powerDescription;
    [SerializeField] private TextMeshProUGUI textDescription;
    [SerializeField] private float displacementText;
    [SerializeField] private float sizeContainer;
    [SerializeField] private float colorValue;
    [SerializeField] private Sprite emptySprite;
    private GameObject objectPowerInsertAux; 
    private GameManager gamerManager;
    private GameObject currentContainer;
    private List<GameObject> listCurrentContainer;
    private GameObject previousContainer;
    private int indexContainer;
    private int indexContainerAux;
    private InterfaceIcon currentIcon;
    private bool powerIsSelected;
    private Power newPower;

    private void Start()
    {
        powerIsSelected = false;
        gamerManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        indexContainer = 0;
        listCurrentContainer = listNewPowerContainer;
        currentContainer = listNewPowerContainer[0];
        currentIcon = generatorPowers.getInterfaceIcon(indexContainer);
        
        RectTransform rectCurrent =  currentContainer.GetComponent<RectTransform>();
        RectTransform rectDescription =  powerDescription.GetComponent<RectTransform>();
    
        rectDescription.anchoredPosition = new Vector2(rectCurrent.anchoredPosition.x,
                                                        rectCurrent.anchoredPosition.y + displacementText);
        
        textDescription.text ="     "+currentIcon.getName +"\n\n"+currentIcon.getDescription;
        focusContainer(false);
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
        else if(Input.GetKeyDown(KeyCode.Escape))
            selectAgainNewPower();
    }

    private void selectAgainNewPower()
    {
        if(listCurrentContainer == listReplaceOptions)
            return;

        if(currentIcon != null && !powerIsSelected)
        {
            if(listCurrentContainer == listPowerContainer)
            {
                unfocusContainer();
                objectPowerInsert.SetActive(false);
                listCurrentContainer = listNewPowerContainer;
                currentContainer = listNewPowerContainer[0];
                currentIcon = generatorPowers.getInterfaceIcon(indexContainer);
                indexContainer = 0;
                focusContainer(false);
            }
        }
        else if(currentIcon != null)
        {
            GameObject container = listCurrentContainer[indexContainerAux];
            Image imagePower = container.transform.GetChild(0).GetComponent<Image>();
            imagePower.sprite = currentIcon.getIcon;
            objectPowerInsert.SetActive(false);
            listPowerContainer.Add(objectContinue);
            currentIcon = null;
        }
    }

    private void checkEnter()
    {
        // si apreto enter en la seccion de elegir poder
        if(listCurrentContainer == listNewPowerContainer)
        {
            unfocusContainer();
            setPower();
            ManagerSound.instance.PlaySFX(selectOption,false);
        }
        else // si estoy en la seccion de acomodar poderes 
        {
            if(currentIcon != null)
            {
                if(listCurrentContainer == listPowerContainer && !powerIsSelected) // estoy en la seccion de poderes
                    checkPositionNewPower();
                else if(listCurrentContainer == listPowerContainer)
                    changedPositionPower();
                else
                    checkDecision(); // estoy en la seccion de reemplazo
            }
            else if(currentContainer == objectContinue) // si es sobre el continue
            {
                interfacePower.SetActive(false);
                gamerManager.nextLevel();
                ManagerSound.instance.PlaySFX(selectOption,false);
            }
            else
                changedPositionPower();
        }
    }

    private void checkPositionNewPower()
    {
        Power p = generatorPowers.getPower(indexContainer);
        
        if(p == null)
        {
            powerIsSelected = true;
            generatorPowers.addPower(newPower,indexContainer);
            generatorPowers.showInfoPower();
            Image currentImage = currentContainer.transform.GetChild(0).GetComponent<Image>();  
            currentImage.sprite = objectPowerInsert.transform.GetChild(0).GetComponent<Image>().sprite;
            objectPowerInsert.SetActive(false);
            listPowerContainer.Add(objectContinue);
            currentContainer = listPowerContainer[listPowerContainer.Count - 1];
            indexContainer = listPowerContainer.Count - 1;
            focusContainer(true);
            currentIcon = null;
        }
        else
        {
            replaceOptions.SetActive(true);
            listCurrentContainer = listReplaceOptions;
            currentContainer = listReplaceOptions[0];
            indexContainerAux = indexContainer;
            indexContainer = 0;
            objectPowerInsertAux = Instantiate(objectPowerInsert,objectPowerInsert.transform.position,
                                                objectPowerInsert.transform.rotation,objectPowerInsert.transform.parent);
            objectPowerInsert.SetActive(false);
            focusContainer(false);
        }
        
        ManagerSound.instance.PlaySFX(moveUI,false);
    }

    private void checkDecision()
    {
        bool reeplace = currentContainer == listCurrentContainer[1];
        unfocusContainer();
        replaceOptions.SetActive(false);
        Destroy(objectPowerInsertAux);
        listCurrentContainer = listPowerContainer;
        currentContainer = listCurrentContainer[indexContainerAux];
        previousContainer = currentContainer;
        indexContainer = indexContainerAux;

        if(reeplace)
        {
            powerIsSelected = true;
            Image imageContainer = currentContainer.transform.GetChild(0).GetComponent<Image>();
            imageContainer.sprite = currentIcon.getIcon;
            generatorPowers.addPower(newPower,indexContainer);
            generatorPowers.showInfoPower();
            listPowerContainer.Add(objectContinue);
            currentIcon = null;
        }
        else
        {
            RectTransform rect = objectPowerInsert.GetComponent<RectTransform>();
            RectTransform rectCurrent = currentContainer.GetComponent<RectTransform>();
            
            rect.anchoredPosition = new Vector2(rectCurrent.anchoredPosition.x,rectCurrent.anchoredPosition.y - 100);
            objectPowerInsert.SetActive(true);
        }
        
        ManagerSound.instance.PlaySFX(selectOption,false);
    }

    private void setPower()
    {
        listCurrentContainer = listPowerContainer;

        if(currentIcon.GetPower != null) // puede ser buff de poder o poder
        {
            if(!currentIcon.isPowerBuff())
            {
                newPower = currentIcon.GetPower;
                int posEmpty = searchEmptySlot();
                objectPowerInsert.SetActive(true);
                Image imagePowerInsert = objectPowerInsert.transform.GetChild(0).GetComponent<Image>();
                imagePowerInsert.sprite = currentIcon.GetPower.getIcon; 
                currentContainer = listCurrentContainer[posEmpty];
                indexContainer = posEmpty;
            }
            else
            {
                int posPower = generatorPowers.getPosPower(currentIcon.GetPower);
                currentIcon.aplyEffect(generatorPowers.getPower(posPower),posPower); 
                changedListPower();
            }
        }
        else
        {
            currentIcon.aplyEffect(null,-1); 
            changedListPower();
        }

        previousContainer = null;
        focusContainer(true);
    }

    private void changedListPower()
    {
        powerIsSelected = true;
        listPowerContainer.Add(objectContinue);
        currentContainer = listCurrentContainer[listCurrentContainer.Count - 1];
        indexContainer = listCurrentContainer.Count - 1;
        currentIcon = null;
    }

    private int searchEmptySlot()
    {
        for(int i = 0; i < 3; i++)
        {
            Power p = generatorPowers.getPower(i);
            if(p == null)
                return i;
        }

        return 0;
    }

    private void selectContainer(int value)
    {
        indexContainer += value;
        if(indexContainer == -1)
            indexContainer = listCurrentContainer.Count - 1;
        else if(indexContainer == listCurrentContainer.Count)
            indexContainer = 0;
        
        currentContainer = listCurrentContainer[indexContainer];
        focusContainer(true);

        if(listCurrentContainer == listNewPowerContainer)
        {
            currentIcon = generatorPowers.getInterfaceIcon(indexContainer);
            textDescription.text ="            "+currentIcon.getName +"\n"+currentIcon.getDescription;
        }

        ManagerSound.instance.PlaySFX(moveUI,false);
    }

    private void unfocusContainer()
    {
        // vuelvo el color grisaseo a el "boton" que apreté
        RectTransform rectCurrent =  currentContainer.GetComponent<RectTransform>();

        rectCurrent.localScale = new Vector2(rectCurrent.localScale.x - sizeContainer,
                                                    rectCurrent.localScale.y - sizeContainer);
    
        Image currentImage = currentContainer.GetComponent<Image>();
        
        currentImage.color = new Color(currentImage.color.r - colorValue,currentImage.color.g - colorValue,
                                        currentImage.color.b - colorValue ,1);
        
        currentImage = currentContainer.transform.GetChild(0).gameObject.GetComponent<Image>();
        currentImage.color = new Color(currentImage.color.r - colorValue,currentImage.color.g - colorValue,
                                        currentImage.color.b - colorValue ,1);
    
        powerDescription.SetActive(false);
    }

    private void changedPositionPower()
    {
        if(generatorPowers.getPower(indexContainer) == null && currentIcon == null)
            return;

        Debug.Log("pasa");

        if(currentIcon == null)
        {   
            currentIcon = generatorPowers.getPower(indexContainer);
            Debug.Log("es nulo: "+currentIcon.getName +  " , "+indexContainer);
            indexContainerAux = indexContainer;
            Image currentImage = currentContainer.transform.GetChild(0).GetComponent<Image>();
            objectPowerInsert.transform.GetChild(0).GetComponent<Image>().sprite = currentImage.sprite;
            currentImage.sprite = emptySprite;
            objectPowerInsert.SetActive(true);
            listPowerContainer.Remove(objectContinue);
        }
        else
        {
            Power p = generatorPowers.getPower(indexContainer);
            if(p == null)
            {
                currentContainer = listPowerContainer[indexContainer];
                Image imageContainer = currentContainer.transform.GetChild(0).GetComponent<Image>();
                imageContainer.sprite = currentIcon.getIcon;
                generatorPowers.addPower(currentIcon.GetPower,indexContainer);
                generatorPowers.removePower(indexContainerAux);
                generatorPowers.showInfoPower();
                currentIcon = null;
            }
            else
            { 
                if(p == (Power)currentIcon)
                {
                    listPowerContainer.Add(objectContinue);
                    Image imageCurrentContainer = currentContainer.transform.GetChild(0).GetComponent<Image>();
                    imageCurrentContainer.sprite =  currentIcon.getIcon;
                    objectPowerInsert.SetActive(false);
                    currentIcon = null;
                    return;
                }

                Debug.Log("hace switch");
                currentContainer = listPowerContainer[indexContainer];
                GameObject containerPowerSwitch = listPowerContainer[indexContainerAux];
                
                Image imageContainer = currentContainer.transform.GetChild(0).GetComponent<Image>();
                Image imageContainerSwitch = containerPowerSwitch.transform.GetChild(0).GetComponent<Image>();
                
                Sprite spriteAux = imageContainer.sprite;
                imageContainer.sprite = imageContainerSwitch.sprite;
                imageContainerSwitch.sprite = spriteAux;

                objectPowerInsert.SetActive(false);
                imageContainer.sprite = currentIcon.getIcon;
                Power p1 = generatorPowers.getPower(indexContainer);
                Power p2 = generatorPowers.getPower(indexContainerAux);
    
                generatorPowers.addPower(p1,indexContainerAux);
                generatorPowers.addPower(p2,indexContainer);
                generatorPowers.showInfoPower();
    
                currentIcon = null;
            }
            
            listPowerContainer.Add(objectContinue);
            objectPowerInsert.SetActive(false);
        }

        ManagerSound.instance.PlaySFX(moveUI,false);
    }

    private void focusContainer(bool unfocusPrevious)
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
        if(unfocusPrevious && previousContainer != null)
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
        
        objectPowerInsert.GetComponent<RectTransform>().anchoredPosition = new Vector2(rectCurrent.anchoredPosition.x,rectCurrent.anchoredPosition.y - 75);
        previousContainer = currentContainer;
    }
}