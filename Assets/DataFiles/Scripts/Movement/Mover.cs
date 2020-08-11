using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{


    public class Mover : MonoBehaviour
    {

        [SerializeField] GameObject target;


        void Start()
        {
            //GetComponent<NavMeshAgent>().speed *= 2;
        }

        // Update is called once per frame
        void Update()
        {
        
            //if (Input.getmousebutton(0))
            //{
            //    MoveToCursor();
            //}


            UpdateAnimator();
        
        }

        private void UpdateAnimator()
        {
            //Getting the global direction
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            //convert global to local direction
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("fowardSpeed", speed);

        }



        public void MoveTo(Vector3 destination)
        {
            GetComponent<NavMeshAgent>().SetDestination(destination);
        }
    }
}
