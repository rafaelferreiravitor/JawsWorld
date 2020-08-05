using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{

    [SerializeField] GameObject target;


    void Start()
    {
        GetComponent<NavMeshAgent>().speed *= 2;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            MoveToCursor();
        }

        
    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray,out hit);
        Debug.DrawRay(ray.origin, ray.direction * 100);
        if (hasHit)
        {
            GetComponent<NavMeshAgent>().SetDestination(hit.point);
        }
        
    }
}
