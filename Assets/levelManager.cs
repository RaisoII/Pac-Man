using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelManager : MonoBehaviour
{
    [SerializeField] private Node nodePacMan, nodeBlinky,nodeInky,nodeClyde,nodePinky;
    [SerializeField] GameObject prefabPacMan,prefabBlinky,prefabInky,prefabClyde,prefabPinky;
    private GameObject PacMan,Blinky,Inky,Clyde,Pinky;
    [SerializeField] private Node[] patrolRoadBlinky,patrolRoadInky,patrolRoadClyde,patrolRoadPinky;
    [SerializeField] private Node exitHouse;

    private void Awake()
    {
        instantiatePlayers();
    }

    private void instantiatePlayers()
    {
        PacMan = Instantiate(prefabPacMan,nodePacMan.transform.position,Quaternion.identity);
        Blinky = Instantiate(prefabBlinky,nodeBlinky.transform.position,Quaternion.identity);
        Inky = Instantiate(prefabInky,nodeInky.transform.position,Quaternion.identity);
        Clyde = Instantiate(prefabClyde,nodeClyde.transform.position,Quaternion.identity);
        Pinky = Instantiate(prefabPinky,nodePinky.transform.position,Quaternion.identity);

        instantiateNodes(Blinky,nodeBlinky,patrolRoadBlinky);
        instantiateNodes(Inky,nodeInky,patrolRoadInky);
        instantiateNodes(Clyde,nodeClyde,patrolRoadClyde);
        instantiateNodes(Pinky,nodePinky,patrolRoadPinky);
    }

    private void instantiateNodes(GameObject ghostPrefab,Node initialNode,Node[] patrolRoad)
    {
        Ghost ghost = ghostPrefab.GetComponent<Ghost>();
        ghost.setParametrerInitial(initialNode,patrolRoad);
    }
}
