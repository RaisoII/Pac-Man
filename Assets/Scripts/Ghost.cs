using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Ghost : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] protected float timeWaiting,timeMaxScatter,timeMaxChasing;
    [SerializeField] protected SpriteRenderer render;
    protected Node currentNode,nextNode,startNode,endNode; // buclean entre estos dos al principio 
    protected MovPacMan movPacMan;
    protected Vector2 targetVector;
    protected Vector2 direction = Vector2.zero;
    protected List<Node> startPath;
    protected Node[] patrolPath;
    protected int previousState; // 0-persecusión 1-dispersion 
    protected Coroutine currentRutine;
    protected Color originalColor;
    protected bool inGhostHouse;
    public event Action OnReachedDestination,checkDistancePacMan;

    public enum GhostState
    {
        Chasing,      // Persecución del jugador
        Frightened,   // Estado asustado (evita al jugador)
        Scatter       // Dispersión (se mueve a una esquina o área específica)
    }
    
    protected GhostState currentState;

    // por ahora solo para desactivar el script
    private void Start()
    {
        
    }
    protected virtual void Awake()
    {
        originalColor = render.color;
        startPath = new List<Node>();
    }

    //es invocado en el awake
    public virtual void startWaiting()
    {
        endNode = startNode.getNeightbor(Vector2.up);
        StartCoroutine(WaitingHouse());
    } 

    protected void Move(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
    // Comportamiento común de persecución
    protected virtual IEnumerator Chase()
    {
        float time = 0;
        while(time < timeMaxChasing)
        {
            nextNode = calculateNextNode(true);
            while(HasReachedDestination(nextNode.transform.position))
            {
                Move(nextNode.transform.position);
                time += Time.deltaTime; 
                yield return null;
            }

            transform.localPosition = nextNode.transform.position;
            Node portalNode = nextNode.getPortalNode();
            
            if(portalNode != null)
            {
                transform.position = portalNode.transform.position;
                currentNode = portalNode;
            }
            else
                currentNode = nextNode;
            
            OnReachedDestination?.Invoke();
        }

        ChangedState(GhostState.Scatter);
    }

    //comportamiento dispersión
    protected  IEnumerator Scatter()
    {
        // ir hacia el punto de la esquina
        targetVector = patrolPath[0].transform.position;
        float time = 0;

        while(HasReachedDestination(targetVector) && time < timeMaxScatter)
        {
            nextNode = calculateNextNode(false);
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
        
        while(time < timeMaxScatter)
        {
            nextNode = patrolPath[i];
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
    // es desactivada desde el manager porque no todos los fantasmas lo desactivan al mismo tiempo
    protected  IEnumerator Frightened()
    {
        while(true)
        {
            Node [] neightbors = currentNode.getNeightbors();
            
            float minimalDistance = 0; // acá hay que tomar el mayor de las distancias
            Vector2 posPacMan = movPacMan.transform.position;
            
            int intX = Mathf.RoundToInt(posPacMan.x);
            int intY = Mathf.RoundToInt(posPacMan.y); 
            posPacMan = new Vector2(intX,intY);
            
            nextNode = null;
            
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
                Move(nextNode.transform.localPosition);
                yield return null;
            }
            
            transform.position = nextNode.transform.position;
            
            Node portalNode = nextNode.getPortalNode();
            
            if(portalNode != null)
            {
                transform.position = portalNode.transform.position;
                currentNode = portalNode;
            }
            else
                currentNode = nextNode;
        }
    }
    
    protected IEnumerator WaitingHouse()
    {
        inGhostHouse = true;
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
                yield return new WaitForEndOfFrame();   
            }
            transform.position = node.transform.position;
        }
        
        currentNode = startPath[startPath.Count - 1];
        
        if(currentState == GhostState.Frightened)
            ChangedState(GhostState.Frightened);
        else
            ChangedState(GhostState.Scatter);
        
        inGhostHouse = false;
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
        if(currentRutine != null)
            StopCoroutine(currentRutine);
        
        currentState = newState;
        currentRutine = StartCoroutine(waitingOneFrame());
    }

    private IEnumerator waitingOneFrame()
    {
        for(int i =0 ; i < 2; i++)
            yield return new WaitForEndOfFrame();
        
        verifyStateChange();
    }

    protected void verifyStateChange()
    {
        if(nextNode == null)
            OnStateChanged();
        else if(!HasReachedDestination(nextNode.transform.position))
        {
            transform.position = nextNode.transform.position;
            OnStateChanged();
        }
        else if(!HasReachedDestination(currentNode.transform.position))
        {
            transform.position = currentNode.transform.position;
            OnStateChanged();
        }
        else
            currentRutine = StartCoroutine(goPosInteger());
    }

    // paso intermedio para pasar entre estados cuadno no me encuentro en una pos exacta
    private IEnumerator goPosInteger()
    {        
        if(currentState == GhostState.Frightened)
        {
            float distanceNextNode = getDistance(nextNode.transform.position,movPacMan.transform.position);
            float distanceCurrentNode = getDistance(currentNode.transform.position,movPacMan.transform.position);
            if(distanceNextNode < distanceCurrentNode)
                nextNode = currentNode;
        }
        else if(currentState == GhostState.Scatter)
        {
            float distanceNextNode = getDistance(nextNode.transform.position,patrolPath[0].transform.position);
            float distanceCurrentNode = getDistance(currentNode.transform.position,patrolPath[0].transform.position);
            
            if(distanceNextNode < distanceCurrentNode)
                nextNode = currentNode;
        }

        while (HasReachedDestination(nextNode.transform.position))
        {
            Move(nextNode.transform.position);
            yield return null; 
        }

        transform.position = nextNode.transform.position;
        
        Node portalNode = nextNode.getPortalNode();
        if(portalNode != null)
        {
            transform.position = portalNode.transform.position;
            currentNode = portalNode;
        }
        else
            currentNode = nextNode;

        nextNode = null;
    
        OnStateChanged();
    }

    private void OnStateChanged()
    {
        switch (currentState)
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

    public void ChangedStateFrightened(bool active)
    {
        if(active)
        {
            render.color = Color.blue;
            if(!inGhostHouse)
                ChangedState(GhostState.Frightened);
            else
                currentState = GhostState.Frightened;
            speed = speed / 2f;
        }
        else
        {
            GhostState previousState = currentState;
            OnReachedDestination?.Invoke();
            render.color = originalColor;
            if(previousState == currentState) // el maldito clyde puede cambiar antes a chasing, lo invocaría dos veces
                ChangedState(GhostState.Chasing);
            speed = speed * 2f;
        }
    }

    public void deathGhost()
    {
        if(currentRutine != null)
            StopCoroutine(currentRutine);
        
        speed = speed * 4f;
        render.color = originalColor;

    }

    protected float getDistance(Vector2 pos,Vector2 target)
    {
        float dx = Mathf.Abs(pos.x - target.x);
        float dy =  Mathf.Abs(pos.y - target.y);
            
        return  dx + dy;
    }

    // tiene que parar todas las rutinas por si algun fantasma aún está en la casa, si es el caso hay dos rutinas corriendo
    public void stopStates() => StopAllCoroutines();

    public void setParametrerInitial(Node ini,Node[] patrolPath)
    {
        startNode = ini;
        currentNode = ini;
        this.patrolPath = patrolPath;
    }

    private bool HasReachedDestination(Vector2 target) => Vector3.Distance(transform.position, target) > 0.1f;

    public void setPacman(GameObject pacMan) => movPacMan = pacMan.GetComponent<MovPacMan>();
}
