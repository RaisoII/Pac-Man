using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTargetIMAN : MonoBehaviour
{
    [SerializeField] private float speed;
    private GameObject target;



    public void setTarget(GameObject target, float speed)
    {
        this.target = target;
        this.speed = speed;
        StartCoroutine(goTarget());
    }

    private IEnumerator goTarget()
    {
        while(HasReachedDestination(target.transform.position))
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = target.transform.position;
    }

    public void stopCoroutine() => StopAllCoroutines();

    private bool HasReachedDestination(Vector2 target) => Vector3.Distance(transform.position, target) > 0.1f;
}
