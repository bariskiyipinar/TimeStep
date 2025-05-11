using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarasaAI : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float patrolSpeed = 2f;
    public float detectionRange = 5f;
    public Transform frog;
    private Transform targetPoint;


    private void Start()
    {
        targetPoint = pointB;
    }

    private void Update()
    {
       Patrol();
        Frog();
    }

    void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            targetPoint = targetPoint == pointA ? pointB : pointA;
        }
    }

    void Frog()
    {
        Vector2 direction = targetPoint.position - frog.position;

        if (direction.x > 0)
        {
           
            frog.localScale = new Vector3(Mathf.Abs(frog.localScale.x), frog.localScale.y, frog.localScale.z);
        }
        else if (direction.x < 0)
        {
            frog.localScale = new Vector3(-Mathf.Abs(frog.localScale.x), frog.localScale.y, frog.localScale.z);
        }
    }



}
