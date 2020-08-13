﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{


    public class Mover : MonoBehaviour
    {

        [SerializeField] Vector3 targetDestination;
        NavMeshAgent navMeshAgent;

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateAnimator();
            
            //print(navMeshAgent.steeringTarget);
        }

        private void UpdateAnimator()
        {
            //Getting the global direction
            Vector3 velocity = navMeshAgent.velocity;
            //convert global to local direction
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("fowardSpeed", speed);

        }

        private void FaceTarget()
        {
            float speed = 100;
            Vector3 direction = (targetDestination - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
        }



        public void MoveTo(Vector3 destination, float stopRange = 0)
        {
            targetDestination = destination;
            navMeshAgent.stoppingDistance = stopRange;
            navMeshAgent.SetDestination(destination);
            if (stopRange > 0)
            {
                FaceTarget();
            }
        }
    }
}
