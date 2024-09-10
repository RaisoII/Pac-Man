using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Ghost : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] protected float timeWaiting,timeScatter,timeChasing,timeFrightened;
    protected Node currentNode,startNode,endNode; // buclean entre estos dos al principio 
    protected MovPacMan movPacMan;
    protected Vector2 targetVector;
    protected Vector2 direction = Vector2.zero;
    protected Grid grid;
    protected List<Node> startPath;
    protected Node[] patrolPath;
    protected Coroutine currentRutine;
    public event Action OnReachedDestination,checkDistancePacMan;

    public enum GhostState
    {
        Chasing,      // Persecución del jugador
        Frightened,   // Estado asustado (evita al jugador)
        Scatter       // Dispersión (se mueve a una esquina o área específica)
    }
    
    protected GhostState currentState;
    protected virtual void Awake()
    {
        startPath = new List<Node>();
        grid = GameObject.Find("GeneralScripts").GetComponent<Grid>();
        Invoke("startWaiting",1f);
    }

    private void startWaiting()
    {
        endNode = startNode.getNeightbor(Vector2.up);
        StartCoroutine(WaitingHouse());
    } 

    protected virtual void Move(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
    // Comportamiento común de persecución
    protected virtual IEnumerator Chase()
    {
        float time = 0;
        while(time < timeChasing)
        {
            Node nextNode = calculateNextNode(true);

            while(HasReachedDestination(nextNode.transform.position))
            {
                Move(nextNode.transform.position);
                time += Time.deltaTime; 
                yield return null;
            }

            transform.position = nextNode.transform.position;
            currentNode = nextNode;
            OnReachedDestination?.Invoke();
        }

        ChangedState(GhostState.Scatter);
    }

    //comportamiento dispersión
    protected virtual IEnumerator Scatter()
    {
        // ir hacia el punto de la esquina
        targetVector = patrolPath[0].transform.position;
        float time = 0;

        while(HasReachedDestination(targetVector) && time < timeScatter)
        {
            Node nextNode = calculateNextNode(false);

            while(HasReachedDestination(nextNode.transform.position))
            {
                Move(nextNode.transform.position);
                time += Time.deltaTime; 
                yield return null;
            }
            transform.localPosition = nextNode.transform.position;
            currentNode = nextNode;
        }

        // bucle patrulla

        int i = 1;

        
        while(time < timeScatter)
        {
            Node nextNode = patrolPath[i];

            while(HasReachedDestination(nextNode.transform.position))
            {
                Move(nextNode.transform.position);
                time += Time.deltaTime; 
                yield return null;
            }

            transform.localPosition = nextNode.transform.position;
            currentNode = nextNode;
            checkDistancePacMan?.Invoke(); // para el maldito Clyde
            i++;

            if(i == patrolPath.Length)
                i =0;
        }

        OnReachedDestination?.Invoke();
        ChangedState(GhostState.Chasing);
    }

    // Comportamiento común cuando el fantasma está asustado
    protected virtual IEnumerator Frightened()
    {
        float time = 0;
        
        // en primera instancia tengo que regresar a mi nodo anterior (para una pos exacta)
        while( time < timeFrightened)
        {
            while(HasReachedDestination(currentNode.transform.localPosition))
            {
                time += Time.deltaTime;
                Move(currentNode.transform.localPosition);
                yield return null;
            }
            
            break;
        }

        transform.position = currentNode.transform.position;

        // en segundo elegir el vecino más distante al pacman, ir al nodo y repetir
        while(time < timeFrightened)
        {
            Node [] neightbors = currentNode.getNeightbors();
            
            float minimalDistance = 0; // acá hay que tomar el mayor de las distancias
            Vector2 posPacMan = movPacMan.transform.position;
            
            int intX = Mathf.RoundToInt(posPacMan.x);
            int intY = Mathf.RoundToInt(posPacMan.y); 
            posPacMan = new Vector2(intX,intY);
            
            Node nextNode = null;
            
            foreach(Node neightbor in neightbors)
            {
                float distanceForPacMan = getDistance(posPacMan,neightbor.transform.localPosition);
                if(distanceForPacMan > minimalDistance)
                {
                    minimalDistance = distanceForPacMan;
                    nextNode = neightbor; 
                }
            }

            while(HasReachedDestination(nextNode.transform.localPosition))
            {
                time += Time.deltaTime;
                Move(nextNode.transform.localPosition);
                yield return null;
            }

            transform.position = nextNode.transform.position;
            currentNode = nextNode;
        }

        // una vez que sale tendría que seguir la rutina anterior?
        ChangedState(GhostState.Chasing);
    }
    
    protected IEnumerator WaitingHouse()
    {
        Coroutine rutineWaiting = StartCoroutine(WaitingPath());
        yield return new WaitForSeconds(timeWaiting);
        StopCoroutine(rutineWaiting);
        StartCoroutine(exitHouse());
    }

    protected IEnumerator exitHouse()
    {
        foreach (Node node in startPath)
        {
            while(HasReachedDestination(node.transform.position))
            {
                Move(node.transform.position);
                yield return null;    
            }
            transform.position = node.transform.position;
        }
        
        currentNode = startPath[startPath.Count - 1];
        ChangedState(GhostState.Scatter);
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
            
            while (HasReachedDestination(target.transform.position))
            {
                Move(target.transform.position);
                yield return null; 
            }

            goingToEnd = !goingToEnd;
        }
    }

    protected void ChangedState(GhostState newState)
    {
        currentState = newState;
        OnStateChanged(newState);
    }

    protected virtual void OnStateChanged(GhostState newState)
    {
        if(currentRutine != null)
            StopCoroutine(currentRutine);
        
        switch (newState)
        {
            case GhostState.Chasing:
                currentRutine = StartCoroutine(Chase());
                break;
            case GhostState.Frightened:
                currentRutine =  StartCoroutine(Frightened());
                break;
            case GhostState.Scatter:
                currentRutine = StartCoroutine(Scatter());
                break;
        }
    }

    protected Node calculateNextNode(bool equalsDirection)
    {
        Node[] neightbors = currentNode.getNeightbors();
        Node idealNode = null;
        float minimalDistance = float.MaxValue;
        Vector2 newDirection = Vector2.zero;
        Vector2 directionAux = direction;
        
        foreach(Node neightbor in neightbors)
        {
            if(equalsDirection)
            {
                Vector2 directionNode = currentNode.getDirectionNode(neightbor);
                if(directionNode != -1*direction) // si la direccion del nodo es distinta a la direccion de la que vengo
                    newDirection = directionNode;
                else
                    continue; 
            }
            
            float distance = getDistance(neightbor.gameObject.transform.localPosition,targetVector);

            if(distance < minimalDistance)
            {
                directionAux = newDirection;
                idealNode = neightbor;
                minimalDistance = distance; 
            }
        }

        direction = directionAux;

        return idealNode;
    }

    public void ChangedStateFrightened()
    {
        if(currentState == GhostState.Frightened)
            timeFrightened += 10;
        else
        {
            ChangedState(GhostState.Frightened);
        }
    }
    protected float getDistance(Vector2 pos,Vector2 target)
    {
        float dx = Mathf.Abs(pos.x - target.x);
        float dy =  Mathf.Abs(pos.y - target.y);
            
        return  dx + dy;
    }

    public void setParametrerInitial(Node ini,Node[] patrolPath)
    {
        startNode = ini;
        currentNode = ini;
        this.patrolPath = patrolPath;
    }

    private bool HasReachedDestination(Vector2 target) => Vector3.Distance(transform.localPosition, target) > 0.1f;

    public void setPacman(GameObject pacMan) => movPacMan = pacMan.GetComponent<MovPacMan>();
}
