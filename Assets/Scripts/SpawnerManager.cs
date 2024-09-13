using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private Node nodePacMan, nodeBlinky,nodeInky,nodeClyde,nodePinky;
    [SerializeField] GameObject prefabPacMan,prefabBlinky,prefabInky,prefabClyde,prefabPinky;
    private GameObject PacMan,Blinky,Inky,Clyde,Pinky;
    [SerializeField] private Node[] patrolPathBlinky,patrolPathInky,patrolPathClyde,patrolPathPinky;
    private List<GameObject> listGhost; 

    public void instantiatePrefabs() => instantiatePlayers();
    private void instantiatePlayers()
    {
        PacMan = GameObject.FindGameObjectWithTag("PacMan");
        
        if(PacMan == null)
        {
            PacMan = Instantiate(prefabPacMan,nodePacMan.transform.position,Quaternion.identity);
            PacMan.GetComponent<MovPacMan>().setNodeIni(nodePacMan);
        } 
        
        listGhost = new List<GameObject>();
        

        if(prefabBlinky != null)
        {
            Blinky = Instantiate(prefabBlinky,nodeBlinky.transform.position,Quaternion.identity);
            instantiateNodes(Blinky,nodeBlinky,patrolPathBlinky);
            listGhost.Add(Blinky);
        }

        if(prefabInky != null)
        {
            Inky = Instantiate(prefabInky,nodeInky.transform.position,Quaternion.identity);
            instantiateNodes(Inky,nodeInky,patrolPathInky);
            listGhost.Add(Inky);
        }

        if(prefabClyde != null)
        {
            Clyde = Instantiate(prefabClyde,nodeClyde.transform.position,Quaternion.identity);
            instantiateNodes(Clyde,nodeClyde,patrolPathClyde);
            listGhost.Add(Clyde);
        }

        if(prefabPinky != null)
        {
            Pinky = Instantiate(prefabPinky,nodePinky.transform.position,Quaternion.identity);
            instantiateNodes(Pinky,nodePinky,patrolPathPinky);
            listGhost.Add(Pinky);
        }
    }

    public List<GameObject> getListGhost() => listGhost;

    private void instantiateNodes(GameObject ghostPrefab,Node initialNode,Node[] patrolPath)
    {
        Ghost ghost = ghostPrefab.GetComponent<Ghost>();
        ghost.setParametrerInitial(initialNode,patrolPath);
        ghost.setPacman(PacMan);
    }

    public void resetPositions()
    {
        PacMan.transform.position = nodePacMan.transform.position;
        if(prefabBlinky != null)
            Blinky.transform.position = nodeBlinky.transform.position;
        
        if(prefabInky != null)
            Inky.transform.position = nodeInky.transform.position;
        
        if(prefabClyde != null)
            Clyde.transform.position = nodeClyde.transform.position;
    
        if(prefabPinky != null)
            Pinky.transform.position = nodePinky.transform.position;
    }

    public GameObject getPacMan() => PacMan;
}