using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePrefabs : MonoBehaviour
{
    [SerializeField] private Node nodePacMan, nodeBlinky,nodeInky,nodeClyde,nodePinky;
    [SerializeField] GameObject prefabPacMan,prefabBlinky,prefabInky,prefabClyde,prefabPinky;
    private GameObject PacMan,Blinky,Inky,Clyde,Pinky;
    [SerializeField] private Node[] patrolPathBlinky,patrolPathInky,patrolPathClyde,patrolPathPinky;

    private void Awake()
    {
        instantiatePlayers();
    }

    private void instantiatePlayers()
    {
        PacMan = Instantiate(prefabPacMan,nodePacMan.transform.position,Quaternion.identity);
        if(prefabBlinky != null)
        {
            Blinky = Instantiate(prefabBlinky,nodeBlinky.transform.position,Quaternion.identity);
            instantiateNodes(Blinky,nodeBlinky,patrolPathBlinky);
        }

        if(prefabInky != null)
        {
            Inky = Instantiate(prefabInky,nodeInky.transform.position,Quaternion.identity);
            instantiateNodes(Inky,nodeInky,patrolPathInky);
        }

        if(prefabClyde != null)
        {
            Clyde = Instantiate(prefabClyde,nodeClyde.transform.position,Quaternion.identity);
            instantiateNodes(Clyde,nodeClyde,patrolPathClyde);
        }

        if(prefabPinky != null)
        {
            Pinky = Instantiate(prefabPinky,nodePinky.transform.position,Quaternion.identity);
            instantiateNodes(Pinky,nodePinky,patrolPathPinky);
        }
    }

    private void instantiateNodes(GameObject ghostPrefab,Node initialNode,Node[] patrolPath)
    {
        Ghost ghost = ghostPrefab.GetComponent<Ghost>();
        ghost.setParametrerInitial(initialNode,patrolPath);
        ghost.setPacman(PacMan);
    }
}
