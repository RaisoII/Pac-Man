using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchNeighbords : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask; // Asigna el layer que contenga los pacDots y los otros nodos.
    [SerializeField] private Node thisNode;
     private float distanceRay = 1.0f; // Distancia entre cada chequeo del rayo
    private GameObject neighborup;
    private GameObject neighborDown;
    private GameObject neighborLeft;
    private GameObject neighborRight;
    private void Awake()
    {
        searchNeighbor(Vector2.up, ref neighborup);     // Dirección arriba
        searchNeighbor(Vector2.down, ref neighborDown);   // Dirección abajo
        searchNeighbor(Vector2.left, ref neighborLeft); // Dirección izquierda
        searchNeighbor(Vector2.right, ref neighborRight);  // Dirección derecha
        StartCoroutine(waitingFrame());
    }

    private IEnumerator waitingFrame()
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject.GetComponent<BoxCollider2D>());
        thisNode.searchDirections();
        Destroy(this);
    }

    private void searchNeighbor(Vector2 dir, ref GameObject neighbor)
    {
        Vector2 currentPos = (Vector2) transform.position + 0.5f*dir; // para que no choque con el mismo nodo o pacDot
        bool neighborFound = false;
        while (!neighborFound)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPos, dir, distanceRay, layerMask);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("pacDots"))
                {
                    currentPos = hit.point + 0.5f*dir; // Actualizamos la posición para continuar desde el pacDot
                }
                else if (hit.collider.CompareTag("Node"))
                {
                    
                    // Encontramos un Nodo, lo guardamos como vecino y terminamos la búsqueda
                    neighbor = hit.collider.gameObject;
                    thisNode.setNeighbor(neighbor.GetComponent<Node>());
                    neighborFound = true;

                    //Debug.Log("Vecino encontrado en dirección " + direccion + ": " + vecino.name);
                }
                else
                {
                    // Chocó con algo que no es pacDot ni Nodo, termina la búsqueda
                    //Debug.Log("No hay vecino válido en dirección " + direccion);
                    neighborFound = true;
                }
            }
            else
            {
                // Si el rayo no choca con nada, termina la búsqueda
               // Debug.Log("No hay vecino en dirección " + direccion);
                neighborFound = true;
            }
        }
    }
}
