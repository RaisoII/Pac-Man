using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBlink : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text; 
    private bool active;


    public void Start()
    {
        active = true;
        StartCoroutine(disabledText());
    }

    private IEnumerator disabledText()
    {
        while(true)
        {
            yield return new WaitForSeconds(.5f);
            active = !active;
            text.enabled = active;
        }
    }
}
