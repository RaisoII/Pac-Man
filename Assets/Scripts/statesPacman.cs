using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class statesPacman : MonoBehaviour
{
    [SerializeField] private bool eating;
    // Start is called before the first frame update
    void Start()
    {
        eating = false;

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        eating = (col.CompareTag("pacDots") || col.CompareTag("Ghost"));
        Debug.Log("comiendo");
    }
}
