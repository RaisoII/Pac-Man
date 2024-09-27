using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class animationController : MonoBehaviour {
    //[SerializeField] public GameObject heart, heartB, eyes, eyesClosed;
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private MovPacMan movPacMan;
    [SerializeField] private Vector2 direction;
    [SerializeField] private GameObject bocaAbierta, bocaCerrada;
    [SerializeField] private bool eating;

    void Start() {
        animator = GetComponent<Animator>();
        bocaCerrada.SetActive(true);
        bocaAbierta.SetActive(false);   
    }

    void Update(){// Crear el vector de movimiento
    }

    void FixedUpdate()
    {
        eating = movPacMan.getEating();
        direction = movPacMan.getDirection();
        if ((Mathf.Abs(direction.x) == 1) && (direction.y == 0)){
            animator.SetFloat("horizontal", direction.x);
            animator.SetFloat("vertical", 0);
        }
        if ((direction.x == 0) && (Mathf.Abs(direction.y) == 1)){
            animator.SetFloat("horizontal", 0);
            animator.SetFloat("vertical", direction.y);
        }
        if(direction.y == 1 && direction.x == 0)
        {
            bocaAbierta.SetActive(false);
            bocaCerrada.SetActive(false);
        }
        else
        {
            bocaAbierta.SetActive(eating);
            bocaCerrada.SetActive(!eating);
        }
       
            
        bocaAbierta.GetComponent<Animator>().SetFloat("horizontal", direction.x);
        bocaAbierta.GetComponent<Animator>().SetFloat("vertical", direction.y);
        bocaCerrada.GetComponent<Animator>().SetFloat("horizontal", direction.x);
        bocaCerrada.GetComponent<Animator>().SetFloat("vertical", direction.y);
    }
}
