using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ColisionPacMan : MonoBehaviour
{
    private GameManager gameManager;
    private LevelManager levelManager;
    [SerializeField] private MovPacMan movPacMan;
    [SerializeField] private bool eating;
    [SerializeField] private float time_eating, eat;
    [SerializeField] private GameObject boca_abierta, boca_cerrada;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnEnable()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    void FixedUpdate()
    {
        if (eating)
        {
            time_eating -= Time.deltaTime; 
        }
        if(time_eating <= 0.0f)
        {
            eating = false;
        }
        boca_abierta.SetActive(eating && (movPacMan.getDirection().y != 1) && !movPacMan.GetComponent<Animator>().GetBool("dead"));
        boca_cerrada.SetActive(!eating && (movPacMan.getDirection().y != 1) && !movPacMan.GetComponent<Animator>().GetBool("dead"));
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("pacDots"))
        {
            time_eating = eat;
            eating = true;
            bool activeFrightened = col.gameObject.GetComponent<PacDots>().getactiveFrightened();
            levelManager.deletePacDot(activeFrightened);
            Destroy(col.gameObject);
        }
        else if (col.CompareTag("Ghost"))
        {
            Ghost ghost = col.gameObject.GetComponent<Ghost>();
            if (ghost.getGhostState() == Ghost.GhostState.Frightened)
            {
                time_eating = eat;
                eating = true;
                col.gameObject.GetComponent<Ghost>().deathGhost();
            }
            else
            {
                levelManager.deathPacMan();
                movPacMan.GetComponent<Animator>().SetBool("dead", true);
            }
        }
    }
}
