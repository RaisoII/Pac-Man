using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] protected float timeWaiting;
    protected Node targetNode;
    protected Node startNode,endNode; // buclean entre estos dos al principio 
    protected Node exitNode;
    protected Vector2 direction;
    
    protected virtual void Awake()
    {
        Invoke("startWaiting",1f);
    }

    private void startWaiting()
    {
        endNode = startNode.getNeightbor(Vector2.up);
        
        if(endNode == null)
            Debug.Log("endNode es nulo: "+gameObject.name);
        
        if(startNode == null)
            Debug.Log("startNode es nulo: "+gameObject.name);
        
        StartCoroutine(WaitingHouse());
    } 

    protected virtual void Move()
    {
        // Comportamiento común de movimiento
    }

    protected virtual void Chase()
    {
        // Comportamiento común de persecución
    }

    protected virtual void Scatter()
    {
        // Comportamiento común de dispersión
    }

    protected virtual void Frightened()
    {
        // Comportamiento común cuando el fantasma está asustado
    }

    protected IEnumerator WaitingHouse()
    {
        Coroutine rutineWaiting = StartCoroutine(WaitingPath());
        yield return new WaitForSeconds(timeWaiting);
        StopCoroutine(rutineWaiting);
        Debug.Log("sale: "+ transform.position);
        //StartCoroutine(exitHouse());

    }

    protected IEnumerator exitHouse()
    {
         // Movimiento hacia el punto de salida
        while (Vector3.Distance(transform.position, exitNode.transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, exitNode.transform.position, Time.deltaTime);
            yield return null;
        }
    }

    protected IEnumerator WaitingPath()
    {
        bool goingToEnd = true; 
        
        while (true) 
        {
            Transform target;
            if(goingToEnd)
                target = endNode.transform;
            else
                target = startNode.transform;
            
            while (Vector3.Distance(transform.position, target.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                yield return null; 
            }

            goingToEnd = !goingToEnd;
        }
    }

    public void setParametrerInitial(Node ini,Node[] patrolRoad)
    {
        startNode = ini;
    }    
}
