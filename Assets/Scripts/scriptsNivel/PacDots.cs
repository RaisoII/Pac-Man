using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacDots : MonoBehaviour
{
    [SerializeField] private bool activeFrightened;
    private Vector2 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    public void restartPosition() => transform.position = startPos;

    public bool getactiveFrightened() => activeFrightened;
    public void destroy() => Destroy(gameObject);
}
