using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationControllerDetailsGhost : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private Ghost ghost;
    [SerializeField] private Vector2 direction;

    void Start()
    {
        direction.x = 1;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        
        direction = ghost.getDirection();
        if ((Mathf.Abs(direction.x) == 1) && (direction.y == 0))
        {
            animator.SetFloat("horizontal", direction.x);
            animator.SetFloat("vertical", 0);
        }
        if ((direction.x == 0) && (Mathf.Abs(direction.y) == 1))
        {
            animator.SetFloat("horizontal", 0);
            animator.SetFloat("vertical", direction.y);
        }
    }
}