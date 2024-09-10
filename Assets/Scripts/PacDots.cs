using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacDots : MonoBehaviour
{
    [SerializeField] private bool activeFrightened;
    private levelManager level;

    private void Start()
    {
        level = GameObject.Find("GeneralScripts").GetComponent<levelManager>();
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("PacMan"))
        {
            level.deletePacDot(activeFrightened);
        }
        Destroy(gameObject);
    }
}
