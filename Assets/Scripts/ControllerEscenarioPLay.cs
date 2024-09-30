using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerEscenarioPLay : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float velocidad;
    [SerializeField] private float distancia;
    [SerializeField] private int index;
    [SerializeField] private Vector2[] recorrrido;
    [SerializeField] private GameObject[] listaObj;
    [SerializeField] private string nombre;

    void Start()
    {
        listaObj = new GameObject[recorrrido.Length];
        transform.position = recorrrido[0];
        for (int i = 0; i < recorrrido.Length; i++) {
            listaObj[i] = new GameObject(nombre + "_" + i);
            listaObj[i].transform.position = recorrrido[i];
        }
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 targetPosition = recorrrido[index];
        transform.position = Vector2.MoveTowards(transform.position, targetPosition * distancia, velocidad * Time.deltaTime);
         //Si el objeto ha llegado a la posición objetivo, cambiar a la siguiente
        if (Vector2.Distance((Vector2)transform.position,targetPosition) < 1){
            index = (index + 1) % recorrrido.Length;  // Avanzar al siguiente índice y repetir el ciclo
        }
    }
}
