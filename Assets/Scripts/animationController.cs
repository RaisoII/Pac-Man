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


    void Start() {
        animator = GetComponent<Animator>();
  
    }

    void Update(){// Crear el vector de movimiento
    }

    void FixedUpdate()
    {
        direction = movPacMan.getDirection();
        if ((Mathf.Abs(direction.x) == 1) && (direction.y == 0)){
            animator.SetFloat("horizontal", direction.x);
            animator.SetFloat("vertical", 0);
        }
        if ((direction.x == 0) && (Mathf.Abs(direction.y) == 1)){
            animator.SetFloat("horizontal", 0);
            animator.SetFloat("vertical", direction.y);
        }
    }
}
