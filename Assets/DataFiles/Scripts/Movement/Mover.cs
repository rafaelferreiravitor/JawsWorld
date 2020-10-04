using RPG.Combat;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using System.Collections.Generic;
using RPG.Resources;

namespace RPG.Movement
{


    public class Mover : MonoBehaviour, IAction, ISaveable
    {

        [SerializeField] Vector3 targetDestination;
        NavMeshAgent navMeshAgent;
        Health health;
        float maximumSpeed = 6;

        
        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = transform.GetComponent<Health>();
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
            /*float speed = 100;
            Vector3 direction = (targetDestination - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);*/
        }



        public void MoveTo(Vector3 destination, float fractionSpeed, float stopRange = 0)
        {
            if (!CanMove())
                return;

            navMeshAgent.speed = maximumSpeed * fractionSpeed;
            targetDestination = destination;
            navMeshAgent.isStopped = false;
            navMeshAgent.stoppingDistance = stopRange;
            navMeshAgent.SetDestination(destination);
        }

        public bool CanMove()
        {
            navMeshAgent.enabled = health.GetIsAlive();
            return health.GetIsAlive();

        }


        public void StartAction(Vector3 destination, float fractionSpeed, float stopRange = 0)
        {
            
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination,fractionSpeed, stopRange);
        }

        public void Cancel()
        {
            if(navMeshAgent.enabled)
                navMeshAgent.isStopped = true;
        }

        [System.Serializable]
        struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }


        public object CaptureState()
        {
            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            MoverSaveData data = (MoverSaveData) state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
