using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderIMAN : MonoBehaviour
{
    [SerializeField] private float speedPacDots;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("pacDots"))
        {
            GoToTargetIMAN goTarget = col.gameObject.GetComponent<GoToTargetIMAN>();
            
            if(goTarget == null)
            {
                goTarget =  col.gameObject.AddComponent<GoToTargetIMAN>();
                goTarget.setTarget(gameObject.transform.parent.gameObject,speedPacDots);
            }
        }
    }
}
