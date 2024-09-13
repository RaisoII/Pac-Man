using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacDots : MonoBehaviour
{
    [SerializeField] private bool activeFrightened;

    public bool getactiveFrightened() => activeFrightened;
    public void destroy() => Destroy(gameObject);
}
