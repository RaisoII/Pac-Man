using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationControllerGhost : MonoBehaviour
{
    private Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void setScared(bool scared) => animator.SetBool("scary", scared);
    
    public void setDead(bool dead) => animator.SetBool("dead",dead);
}

