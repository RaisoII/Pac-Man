using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationController : MonoBehaviour {
    
    private Animator animator,animBocaAbierta,animBocaCerrada;
    [SerializeField] private Vector2 direction;
    [SerializeField] private GameObject bocaAbierta, bocaCerrada;

    void Awake() {
        
        animator = GetComponent<Animator>();
        animBocaAbierta = bocaAbierta.GetComponent<Animator>();
        animBocaCerrada = bocaCerrada.GetComponent<Animator>();  
        direction = Vector2.left;
    }

    public void changedAnimationEating(bool eating)
    {
        if(direction != Vector2.up)
        {
            bocaAbierta.SetActive(eating);
            bocaCerrada.SetActive(!eating);
        }
        
        changedPositionEat();
    }

    private void changedPositionEat()
    {
        animBocaAbierta.SetFloat("horizontal", direction.x);
        animBocaAbierta.SetFloat("vertical", direction.y);
        animBocaCerrada.SetFloat("horizontal", direction.x);
        animBocaCerrada.SetFloat("vertical", direction.y);
    }

    public void changedAnimationDirection(Vector2 direction)
    {
        this.direction = direction;
        if ((Mathf.Abs(direction.x) == 1) && (direction.y == 0))
        {
            animator.SetFloat("horizontal", direction.x);
            animator.SetFloat("vertical", 0);
            animBocaAbierta.gameObject.SetActive(true);
            animBocaCerrada.gameObject.SetActive(true);

        }else
        {
            animator.SetFloat("horizontal", 0);
            animator.SetFloat("vertical", direction.y);

            if ((direction.x == 0) && (direction.y == 1))
            {
                bocaAbierta.gameObject.SetActive(false);
                bocaCerrada.gameObject.SetActive(false);
            }
            else
            {
                bocaAbierta.gameObject.SetActive(true);
                bocaCerrada.gameObject.SetActive(true);
            }
        }

        changedPositionEat();
    }

    // entra al iniciar y reiniciar el nivel

    public void deathPacMan(bool dead)
    {
        if(dead)
        {
            bocaAbierta.SetActive(false);
            bocaCerrada.SetActive(false);
        }
        else
        {
            direction = Vector2.left;
            bocaCerrada.SetActive(true);
            changedAnimationDirection(direction);
        }

        animator.SetBool("dead", dead);
    }
}
