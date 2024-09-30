using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerINFERNOPATH : MonoBehaviour,powerObject
{

    [SerializeField] private GameObject prefabFire;
    [SerializeField] private float timeFire;
    [SerializeField] private float maxSpawnDelay = 1f; // Tiempo máximo entre spawns (velocidad baja)
    [SerializeField] private float minSpawnDelay = 0.1f; // Tiempo mínimo entre spawns (velocidad alta)

    private float speedPacMan;
    private float time;
    
    private IEnumerator powerActivate()
    {
        while(time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
    public void setTime(float time)
    {
        this.time = time;
        StartCoroutine(powerActivate());
        StartCoroutine(spawnFire());
    }

    private IEnumerator spawnFire()
    {
        MovPacMan movPac = GameObject.FindWithTag("PacMan").GetComponent<MovPacMan>(); 
        speedPacMan = movPac.getSpeed();
        float spawnDelay = Mathf.Lerp(maxSpawnDelay, minSpawnDelay, speedPacMan); // Interpolación entre los delays
        
        while (true)
        {
            Vector2 posFire =new Vector2(transform.position.x, transform.position.y) - .5f* movPac.getDirection();
            GameObject fire = Instantiate(prefabFire,posFire, Quaternion.identity);
            fire.GetComponent<ColisionFire>().setTimeFire(timeFire);
            
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
