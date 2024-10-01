using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerEscenarioPLay : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private RectTransform[] path;
    [SerializeField] private RectTransform rectGhost;
    private int index;
    private Vector2 target;

    void Start()
    {
        index = 1;
        StartCoroutine(travel());
    }
    private IEnumerator travel()
    {
        while (true)
        {
            target = path[index].anchoredPosition;
            while (hisTraveling())
            {
                rectGhost.anchoredPosition = Vector2.MoveTowards(rectGhost.anchoredPosition,target,speed * Time.deltaTime);
                yield return null;
            }

            rectGhost.anchoredPosition = target;
            index = (index + 1) % path.Length; 

            yield return null;
        }
    }
    private bool hisTraveling() => Vector2.Distance(rectGhost.anchoredPosition, target) > 0.1f;
}
